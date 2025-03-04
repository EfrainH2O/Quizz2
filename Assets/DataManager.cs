using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    //Singleton
    public static DataManager Instance;
    //Tracking Data
    public int score;
    private int actualQuestionCount;
    public int questionsCount;
    public int timeLeft;
    [SerializeField]
    private int MaxTimeAnswer;

    [SerializeField]
    private TextMeshProUGUI TimerUI;
    [SerializeField]
    private TextMeshProUGUI ScoreUI;
    [SerializeField]
    private TextMeshProUGUI QuestionCountUI;

    void Awake()
    {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
        
    }

    public void StartQuestions(){
        score = 0;
        actualQuestionCount = 1;
        questionsCount = QuestionaryManager.Instance.Questions.Count;
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
        
        actualQuestionCount = actualQuestionCount == questionsCount ? actualQuestionCount : ++actualQuestionCount;
        if(isCorrect){
            score += 200*timeLeft/MaxTimeAnswer ;
        }
        StopAllCoroutines();
        QuestionaryManager.Instance.NextQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        TimerUI.text = timeLeft.ToString();
        ScoreUI.text = "Score: "+ score.ToString() ;
        QuestionCountUI.text = actualQuestionCount + "/" + questionsCount;
    }
}
