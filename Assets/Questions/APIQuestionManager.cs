using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class APIQuestionManager : MonoBehaviour
{
    public static APIQuestionManager Instance { get; private set; }

    [Header("API Configuration")]
    [SerializeField] private string baseUrl = "http://localhost:5011/Quiz"; // solo el dominio, pq el token y curso son dinamicos

    private List<Question> internalQuestions = new List<Question>();
    public bool IsReady { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        StartCoroutine(InitializeFetch());
    }

    private IEnumerator InitializeFetch()
    {
        // Espera a que TokenManager tenga datos
        while (string.IsNullOrEmpty(TokenManager.Instance.Token) || TokenManager.Instance.CursoId == 0)
        {
            yield return null;
        }

        string urlWithParams = $"{baseUrl}?id_curso={TokenManager.Instance.CursoId}";
        Debug.Log($"[API] Intentando conectar con: {urlWithParams}");

        yield return StartCoroutine(FetchQuestionsFromAPI(urlWithParams));
    }

    private IEnumerator FetchQuestionsFromAPI(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Mandar el token en Authorization
            request.SetRequestHeader("Authorization", "Bearer " + TokenManager.Instance.Token);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("[API] Conexión exitosa.");

                string wrappedJson = "{\"preguntas\":" + request.downloadHandler.text + "}";
                QuestionAPIListWrapper wrapper = JsonUtility.FromJson<QuestionAPIListWrapper>(wrappedJson);

                if (wrapper == null || wrapper.preguntas == null)
                {
                    Debug.LogError("[API] La respuesta del servidor no pudo ser parseada.");
                    yield break;
                }

                Debug.Log($"[API] Recibidas {wrapper.preguntas.Count} preguntas.");

                int index = 1;
                foreach (QuestionAPI qApi in wrapper.preguntas)
                {
                    Question q = ConvertToInternalQuestion(qApi);
                    internalQuestions.Add(q);
                    Debug.Log($"[API] Pregunta {index}: \"{q.QuestionText}\" con {q.Options.Count} opciones.");
                    index++;
                }

                IsReady = true;
                Debug.Log("[API] Preguntas listas para usar.");
            }
            else
            {
                Debug.LogError($"[API] Error de conexión: {request.error}\nURL: {url}");
            }
        }
    }

    private Question ConvertToInternalQuestion(QuestionAPI apiQuestion)
    {
        Question q = new Question
        {
            QuestionText = apiQuestion.texto,
            Options = new List<Option>()
        };

        foreach (OptionAPI opt in apiQuestion.opciones)
        {
            q.Options.Add(new Option
            {
                OptionText = opt.texto,
                IsCorrect = opt.correcta
            });
        }

        return q;
    }

    public List<Question> GetQuestions()
    {
        return internalQuestions;
    }

    [System.Serializable]
    public class QuestionAPI
    {
        public int idPregunta;
        public string texto;
        public List<OptionAPI> opciones;
    }

    [System.Serializable]
    public class OptionAPI
    {
        public int idOpcion;
        public string texto;
        public bool correcta;
    }

    [System.Serializable]
    public class QuestionAPIListWrapper
    {
        public List<QuestionAPI> preguntas;
    }
}
