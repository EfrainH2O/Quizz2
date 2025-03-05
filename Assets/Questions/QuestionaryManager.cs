
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class QuestionaryManager : MonoBehaviour
{
    //Question Refered Variables
    private Question ActualQuestion;
    //Speed Variables
    [SerializeField]
    private float textSpeed;
    [SerializeField]
    private float optionSpeed;
    //textGameObjects
    [SerializeField]
    private TextMeshProUGUI questionArea;

    private List<GameObject> optionsAreas;
    //Singleton
    public static QuestionaryManager Instance;

    private int score;

    void Awake()
    {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
        optionsAreas = new List<GameObject>();
        foreach (Transform child in transform.GetChild(1)){
            optionsAreas.Add(child.gameObject);
        }
        
        textSpeed = 1/ textSpeed;
        optionSpeed = 1/ optionSpeed;
    }
    void Start()
    {
        score = 0;
    }

    public void NextQuestion(Question item)
    {
        foreach (GameObject option in optionsAreas)
        {
            option.SetActive(false);
        }
        ActualQuestion = item;
        if(ActualQuestion != null){
            StartCoroutine(ShowQuestion(ActualQuestion));
        }else{
            questionArea.gameObject.SetActive(false);
        }
    }

    IEnumerator ShowQuestion(Question item)
    {
       
        questionArea.gameObject.SetActive(true);
        questionArea.GetComponent<TextMeshProUGUI>().text = "";
        foreach (char letter in item.QuestionText.ToCharArray())
        {
             Debug.Log("Showing Question");
             Debug.Log(textSpeed);
            questionArea.GetComponent<TextMeshProUGUI>().text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        item.Options.Shuffle();
        for(int i = 0; i < 4;i++){
            yield return new WaitForSeconds(optionSpeed);
            if(i < item.Options.Count){
                optionsAreas[i].SetActive(true);
            }else{
                continue;
            }
            optionsAreas[i].GetComponentInChildren<TextMeshProUGUI>().text = item.Options[i].OptionText;
            optionsAreas[i].GetComponent<ButtonAsign>().IsCorrect = item.Options[i].IsCorrect;
        }
        DataManager.Instance.StartTimer();
    }

}
 [System.Serializable]
public class Question{
   [SerializeField]
    public string QuestionText;
    [SerializeField]
    public List< Option> Options = new List<Option>();


}
[System.Serializable]
public class Option{
    [SerializeField]
    public string OptionText;
    [SerializeField]
    public bool IsCorrect;
}


public static class IListExtensions {
	/// <summary>
	/// Shuffles the element order of the specified list.
	/// </summary>
	public static void Shuffle<T>(this IList<T> ts) {
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = UnityEngine.Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}
}