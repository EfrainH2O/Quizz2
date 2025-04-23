using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class APIQuestionManager : MonoBehaviour
{
    #region Variables
    public static APIQuestionManager Instance { get; private set; }

    [Header("API Configuration")]
    [SerializeField] public string apiUrl = "http://localhost:5011/Quiz?id_curso=1&id_alumno=2";

    private List<Question> internalQuestions = new List<Question>();
    private int currentQuestionIndex = 0;
    #endregion

    #region Unity Lifecycle
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
        StartCoroutine(FetchQuestionsFromAPI(apiUrl));
    }
    #endregion

    #region API Communication
    private IEnumerator FetchQuestionsFromAPI(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            Debug.Log($"Requesting questions from API: {url}");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API response received: " + request.downloadHandler.text);
                
                string wrappedJson = "{\"preguntas\":" + request.downloadHandler.text + "}";
                QuestionAPIListWrapper wrapper = null;

                try
                {
                    wrapper = JsonUtility.FromJson<QuestionAPIListWrapper>(wrappedJson);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("JSON parsing error: " + e.Message);
                }

                if (wrapper != null && wrapper.preguntas != null)
                {
                    foreach (QuestionAPI qApi in wrapper.preguntas)
                    {
                        internalQuestions.Add(ConvertToInternalQuestion(qApi));
                    }

                    Debug.Log($"Loaded {internalQuestions.Count} questions from API.");

                    if (internalQuestions.Count > 0)
                    {
                        ShowNextQuestion(); // Puedes comentar esto si prefieres dejar que DataManager controle esto
                    }
                    else
                    {
                        Debug.LogWarning("No questions received from API.");
                    }
                }
                else
                {
                    Debug.LogWarning("Parsed wrapper or preguntas list is null.");
                }
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
    #endregion

    #region Question Management
    public void ShowNextQuestion()
    {
        if (currentQuestionIndex < internalQuestions.Count)
        {
            if (QuestionaryManager.Instance != null)
            {
                QuestionaryManager.Instance.NextQuestion(internalQuestions[currentQuestionIndex]);
                currentQuestionIndex++;
            }
            else
            {
                Debug.LogError("QuestionaryManager instance not found!");
            }
        }
        else
        {
            Debug.Log("No more questions available.");
        }
    }

    public void ResetQuestions()
    {
        currentQuestionIndex = 0;
    }

    public List<Question> GetQuestions()
    {
        return internalQuestions;
    }

    public Question GetCurrentQuestion()
    {
        if (currentQuestionIndex < internalQuestions.Count)
        {
            return internalQuestions[currentQuestionIndex];
        }
        return null;
    }
    #endregion

    #region Data Models
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
    #endregion
}
