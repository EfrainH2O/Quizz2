using UnityEngine;

public class panel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject instructionPanel; 
    void Start()
    {
        instructionPanel.SetActive(false);
    
    }
    public void ShowPanel()
    {
        instructionPanel.SetActive(true);
    }
    public void Back(){
        instructionPanel.SetActive(false);
    }

    // Update is called once per frame

    void Update()
    {
        
        
    }
}
