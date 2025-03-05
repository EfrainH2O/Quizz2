using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAsign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool IsCorrect;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SubmitAnswer);
    }

    private void SubmitAnswer()
    {
        DataManager.Instance.SubmitAnswer(IsCorrect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
