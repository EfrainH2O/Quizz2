using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class APIQuestionManager : MonoBehaviour
{
    public static APIQuestionManager Instance { get; private set; }

    [Header("API Configuration")]
    [SerializeField] private string apiUrl = "http://localhost:5011/Quiz?id_curso=1&id_alumno=2";

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
        Debug.Log("[API] Start ejecutado.");
        StartCoroutine(FetchQuestionsFromAPI(apiUrl));
    }

    private IEnumerator FetchQuestionsFromAPI(string url)
    {
        Debug.Log("[API] Intentando conectar con: " + url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
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

    // Modelos del API
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
