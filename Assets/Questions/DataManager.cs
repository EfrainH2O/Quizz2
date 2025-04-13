using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

    //Questions Refered Variables
    [SerializeField]
    public List<Question> Questions;
    //Singleton
    public static DataManager Instance;
    //Tracking Data
    private bool inQuestions;
    public int score;
    private int questionIndex;
    public int questionsCount;
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
        StartQuestions();
        
    }

    public void StartQuestions(){
        // inQuestions = true;
        // score = 0;
        // questionIndex = 0;
        // questionsCount = Questions.Count;
        // QuestionaryManager.Instance.NextQuestion(Questions[questionIndex]);
        // objectM.StartReparation();
        StartCoroutine(LoadQuestions());

    }

    private IEnumerator LoadQuestions()
    {
        // 2. Esperar a que las preguntas sean cargadas desde la API
        yield return new WaitUntil(() => APIQuestionManager.Instance.GetQuestions().Count > 0);

        // 3. Asignar las preguntas obtenidas a la lista local
        Questions = APIQuestionManager.Instance.GetQuestions();

        inQuestions = true;
        score = 0;
        questionIndex = 0;
        questionsCount = Questions.Count;

        // 4. Mostrar la primera pregunta con QuestionaryManager
        QuestionaryManager.Instance.NextQuestion(Questions[questionIndex]);

        // Iniciar el proceso de reparación del objeto (si es necesario)
        objectM.StartReparation();
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
        }
    }
    private IEnumerator FinalCountDown(){
        yield return new WaitForSeconds(3f);
        inQuestions = false;
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
