using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Image barr;
    public int max, correct;
    float percent;
    void Awake()
    {
        barr = GetComponent<Image>();
        barr.fillAmount = 0;
    }
    void Start()
    {
        percent = (float)correct/(float)max;
        StartCoroutine(Filling());
    }
    private IEnumerator Filling(){

        while(barr.fillAmount != percent){
            barr.fillAmount = Mathf.Lerp(barr.fillAmount, percent, 1.2f * Time.deltaTime);
            yield return null;
        }
    }

}
