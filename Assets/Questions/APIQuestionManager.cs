using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class APIQuestionManager : MonoBehaviour
{
    public static APIQuestionManager Instance { get; private set; }

    [Header("API Configuration")]
    [SerializeField] private string apiUrl = "http://10.21.28.5:5011/Quiz?id_curso=1&id_alumno=2";

    private List<Question> questions = new List<Question>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            StartCoroutine(FetchQuestionsFromAPI(apiUrl)); //* Cargar preguntas apenas se instancie este script
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator FetchQuestionsFromAPI(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string wrappedJson = "{\"preguntas\":" + request.downloadHandler.text + "}";

                QuestionAPIListWrapper wrapper = JsonUtility.FromJson<QuestionAPIListWrapper>(wrappedJson);
                questions.Clear();

                foreach (QuestionAPI qApi in wrapper.preguntas)
                {
                    questions.Add(ConvertToInternalQuestion(qApi));
                }

                Debug.Log($"API Loaded: {questions.Count} questions fetched."); //* Confirmación en consola
            }
            else
            {
                Debug.LogError($"API Connection Error: {request.error}\nURL: {url}");
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
            Option o = new Option
            {
                OptionText = opt.texto,
                IsCorrect = opt.correcta
            };
            q.Options.Add(o);
        }

        return q;
    }

    public List<Question> GetQuestions()
    {
        return questions;
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
