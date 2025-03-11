using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    private List<Reparable> electrodomesticos;
    private int ListSize; 
    private int actualItem;
    
    public void Awake(){
       
        
    }
    public void Start()
    {
        //El codigo Reparable es donde seria buen lugar para que pongas los efectos
         electrodomesticos = new List<Reparable>(FindObjectsByType<Reparable>(FindObjectsSortMode.None));
        actualItem = 0;
        ListSize = DataManager.Instance.questionsCount;
        while(electrodomesticos.Count > ListSize){
            electrodomesticos.RemoveAt(Random.Range(0,electrodomesticos.Count));
        }
        Debug.Log(electrodomesticos.Count);
        StartCoroutine(HideObjects());
        
    }

    public void RepairResult(bool result){
        if(result){
            electrodomesticos[actualItem].gameObject.SetActive(true);
            //Efecto de reparacion

        }else{
            //Efecto de danio
        }
        //Next item
        actualItem++;
        StartReparation();
    }
    private IEnumerator HideObjects(){
        foreach(Reparable r in electrodomesticos){
            r.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator ShowItem(){
        yield return new WaitForSeconds(0.6f);
        //Aqui seria Movel el item al punto "actualItem"
        //Le puse un poco de delay para que no sea todo instantaneo
    }

    public void StartReparation(){
        StartCoroutine(ShowItem());
    }

}
