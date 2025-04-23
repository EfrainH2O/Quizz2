using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    [SerializeField] public List<Question> Questions;

    public static DataManager Instance;

    private bool inQuestions;
    public int score;
    private int questionIndex;
    public int questionsCount;
    public int timeLeft;

    [SerializeField] private int MaxTimeAnswer;

    [SerializeField] private TextMeshProUGUI TimerUI;
    [SerializeField] private TextMeshProUGUI ScoreUI;
    [SerializeField] private TextMeshProUGUI QuestionCountUI;
    [SerializeField] private GameObject FinalMessage;

    [SerializeField] private Scoreboard scoreboard;

    [SerializeField] private Button startButton; //* ← Asignar en el Inspector si quieres activar solo cuando cargue

    private ObjectManager objectM;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        objectM = GetComponent<ObjectManager>();
    }

    public void OnStartGamePressed() //* ← Se llama desde el botón
    {
        FinalMessage.SetActive(false);

        if (APIQuestionManager.Instance.GetQuestions().Count > 0)
        {
            StartQuestions();
        }
        else
        {
            Debug.LogWarning("Aún no se cargan las preguntas...");
        }
    }

    public void StartQuestions()
    {
        Questions = APIQuestionManager.Instance.GetQuestions();

        if (Questions == null || Questions.Count == 0)
        {
            Debug.LogError("No hay preguntas cargadas desde la API.");
            return;
        }

        inQuestions = true;
        score = 0;
        questionIndex = 0;
        questionsCount = Questions.Count;

        QuestionaryManager.Instance.NextQuestion(Questions[questionIndex]);
        objectM.StartReparation();
    }

    public void StartTimer()
    {
        timeLeft = MaxTimeAnswer;
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        SubmitAnswer(false);
    }

    public void SubmitAnswer(bool isCorrect)
    {
        if (isCorrect) score++;

        StopAllCoroutines();
        QuestionaryManager.Instance.ResetData();
        objectM.RepairResult(isCorrect);

        if (questionIndex == questionsCount - 1)
        {
            QuestionaryManager.Instance.NextQuestion(Questions[questionIndex]);
            StartCoroutine(TriggerFinalAfterDelay());
        }
        else
        {
            questionIndex++;
            QuestionaryManager.Instance.NextQuestion(Questions[questionIndex]);
        }
    }

    private IEnumerator FinalCountDown()
    {
        yield return new WaitForSeconds(3f);
        inQuestions = false;
    }

    void Update()
    {
        //* Activa el botón cuando ya hay preguntas disponibles
        if (!startButton.interactable && APIQuestionManager.Instance.GetQuestions().Count > 0)
        {
            startButton.interactable = true;
        }

        if (inQuestions)
        {
            TimerUI.text = timeLeft.ToString();
            ScoreUI.text = "Correctas: " + score + " / " + questionsCount;
            QuestionCountUI.text = "Preguntas: " + questionIndex + " / " + questionsCount;
        }
        else
        {
            TimerUI.gameObject.SetActive(false);
            ScoreUI.gameObject.SetActive(false);
            QuestionCountUI.gameObject.SetActive(false);

            scoreboard.max = questionsCount;
            scoreboard.correct = score;
            scoreboard.transform.parent.gameObject.SetActive(true);

            FinalMessage.SetActive(true);
            FinalMessage.GetComponent<TextMeshProUGUI>().text = "Correcto: " + score + " / " + questionsCount;

            gameObject.SetActive(false);
        }
    }

    private IEnumerator TriggerFinalAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(FinalCountDown());
        Debug.Log("End Game");
    }
}
