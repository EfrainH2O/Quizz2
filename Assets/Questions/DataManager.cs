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
    public int score;
    private int questionIndex;
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
    public void Start()
    {
        StartQuestions();
    }

    public void StartQuestions(){
        score = 0;
        questionIndex = 0;
        questionsCount = Questions.Count;
        QuestionaryManager.Instance.NextQuestion(Questions[questionIndex]);
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
        questionIndex++;
        if(isCorrect){
            score += 200*timeLeft/MaxTimeAnswer ;
        }
        StopAllCoroutines();
        if(questionIndex == questionsCount){
            QuestionaryManager.Instance.NextQuestion(null);
            Debug.Log("End Game");
            return;
        }else{
            QuestionaryManager.Instance.NextQuestion(Questions[questionIndex]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TimerUI.text = timeLeft.ToString();
        ScoreUI.text = "Score: "+ score.ToString() ;
        QuestionCountUI.text =( questionIndex+1) + "/" + questionsCount;
    }
}
