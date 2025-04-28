using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

    //Questions Refered Variables
    
    public List<Question> Questions;
    //Singleton
    public static DataManager Instance;
    //Tracking Data
    private bool inQuestions;
    public int score;
    private int questionIndex;
    public int questionsCount;
    //public bool usarAPI = true; //*
    public int timeLeft;
    [SerializeField]
    private int MaxTimeAnswer;
    //UI to FIll

    [SerializeField]
    private TextMeshProUGUI TimerUI;
    [SerializeField]
    private TextMeshProUGUI ScoreUI;
    [SerializeField]
    private TextMeshProUGUI QuestionCountUI;
    [SerializeField]
    private GameObject FinalMessage;
    [SerializeField]

    private Scoreboard scoreboard;
    //Objects to interact
    private ObjectManager objectM;

    void Awake()
    {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
        objectM = GetComponent<ObjectManager>();
        
    }
    public void Start()
    {
        FinalMessage.SetActive(false);
        StartQuestionsEmpty(); 
        StartCoroutine(InitializeWithAPI());
        
    }

    private IEnumerator InitializeWithAPI()
    {
        //Questions?.Clear(); 
        // Esperar a que el API esté listo
        while (APIQuestionManager.Instance == null || !APIQuestionManager.Instance.IsReady)
        {
            yield return null;
        }

        Questions = APIQuestionManager.Instance.GetQuestions();


        if (Questions == null || Questions.Count == 0)
        {
            Debug.LogError("[DataManager] No se cargaron preguntas del API.");
            yield break;
        }

        Debug.Log($"[DataManager] Se cargaron {Questions.Count} preguntas del API.");
        questionsCount = Questions.Count;
        Debug.Log($"[DataManager] Nuevo total de preguntas: {questionsCount}");

        //objectM.SetupObjects(); 
        
        StartQuestions();
    }


    public void StartQuestionsEmpty(){
        inQuestions = true;
        score = 0;
        questionIndex = 0;
        questionsCount = 0;
        //Questions?.Clear(); 
        //QuestionaryManager.Instance.NextQuestion(Questions[questionIndex]);
        //objectM.StartReparation();
    }

    public void StartQuestions(){
        inQuestions = true;
        score = 0;
        questionIndex = 0;
        //questionsCount = Questions.Count;
        //Questions?.Clear(); 

        Debug.Log($"[DataManager] Iniciando juego con {questionsCount} preguntas.");

        objectM.SetupObjects();
        objectM.StartReparation();

        QuestionaryManager.Instance.NextQuestion(Questions[questionIndex]);
        // objectM.StartReparation();
    }

    
    public void StartTimer(){
        timeLeft = MaxTimeAnswer;
        StartCoroutine(CountDown());
    }
    IEnumerator CountDown(){
        while(timeLeft > 0){
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
        SubmitAnswer(false);
    }
    public void SubmitAnswer(bool isCorrect)
    {
        questionIndex = questionIndex < questionsCount? questionIndex+1 : questionIndex ;
        if(isCorrect){
            score ++ ;
        }
        StopAllCoroutines();
        QuestionaryManager.Instance.ResetData();
        objectM.RepairResult(isCorrect);
        if(questionIndex == questionsCount){
            QuestionaryManager.Instance.NextQuestion(null);
            StartCoroutine(FinalCountDown());
            Debug.Log("End Game");
            return;
        }else{
            QuestionaryManager.Instance.NextQuestion(Questions[questionIndex]);
            //objectM.StartReparation();
        }
    }
    private IEnumerator FinalCountDown()
    {
        yield return new WaitForSeconds(3f);
        inQuestions = false;

        // ✅ Nuevo: Enviar resultado al servidor
        if (PostQuizResult.Instance != null)
        {
            PostQuizResult.Instance.SendResult(score, questionsCount);
            Debug.Log("[DataManager] Resultado enviado al servidor al finalizar el quiz.");
        }
        else
        {
            Debug.LogWarning("[DataManager] PostQuizResult no encontrado.");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(inQuestions){
            TimerUI.text = timeLeft.ToString();
            ScoreUI.text = "Correctas: "+ score + " / "  + questionsCount ;
            QuestionCountUI.text ="Preguntas: "+ questionIndex + " / " + questionsCount;
        }else{
            TimerUI.gameObject.SetActive(false);
            ScoreUI.gameObject.SetActive(false);
            QuestionCountUI.gameObject.SetActive(false);
            scoreboard.max = questionsCount;
            scoreboard.correct = score;
            scoreboard.transform.parent.gameObject.SetActive(true);
            FinalMessage.gameObject.SetActive(true);
            FinalMessage.GetComponent<TextMeshProUGUI>().text = "Correcto: "+score +" / "+ questionsCount;

            gameObject.SetActive(false);
        }
        
    }
}