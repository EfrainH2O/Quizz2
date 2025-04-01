using System.Collections;
using System.Data;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CloneChar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private  float vel;    public Vector3 MaxScale;
    private float MaxTime = 120f;


    void Start()
    {    
        StartCoroutine(Hello());
        
    }


    private IEnumerator Hello(){
        vel = (MaxScale.magnitude - transform.localScale.magnitude)/MaxTime;
        while(transform.localScale.x  < MaxScale.x &&
                 transform.localScale.y < MaxScale.y && 
                    transform.localScale.z < MaxScale.z  ){
            transform.localScale += Vector3.one*vel;
            yield return null;
        }
    }

    
  

    public void FinalAction(bool result){
        
        if(result){
            //Efectos Sonoros
            //Efecto visual de estela
        }else{
            //Efecto sonoro malo
            
        } 
        StartCoroutine(AutoDestruction());

    }

    public IEnumerator AutoDestruction(){
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
