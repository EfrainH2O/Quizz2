using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ObjectManager : MonoBehaviour
{
    private List<Reparable> electrodomesticos;
    private int ListSize; 
    private int actualItem;
    [SerializeField]
    private AnimatorController ac;
    [SerializeField]
    private float ObVel; 
    
    private GameObject tempClone;
    [SerializeField]
    private Transform clonePosition; 

    public ParticleSystem correctParticle;
    public ParticleSystem wrongParticle;

    private void Start()
    {

        electrodomesticos = new List<Reparable>(FindObjectsByType<Reparable>(FindObjectsSortMode.None));
        actualItem = 0;
        ListSize = DataManager.Instance.questionsCount;
        
        while (electrodomesticos.Count > ListSize)
        {
            electrodomesticos.RemoveAt(Random.Range(0, electrodomesticos.Count));
        }
        foreach(Reparable r in electrodomesticos){
            r.transform.position += new Vector3(0,clonePosition.position.y,0);
            r.ObVel = ObVel;
            r.gameObject.SetActive(false);
        }
    }


    public void RepairResult(bool result)
    {
        tempClone.GetComponent<CloneChar>()?.FinalAction(result);
        if (result)
        {
            correctParticle.Play();
            electrodomesticos[actualItem].gameObject.SetActive(true);
            electrodomesticos[actualItem].ReturnToPlace();
        }
        else
        {
            wrongParticle.Play();
        }
        
        // Siguiente objeto
        actualItem++;
        StartReparation();
    }



    private IEnumerator ShowItem()
    {
        yield return new WaitForSeconds(0.5f);

        if (actualItem < electrodomesticos.Count)
        {
            tempClone = Instantiate(electrodomesticos[actualItem].gameObject, clonePosition.position, Quaternion.identity);
            tempClone.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            tempClone.SetActive(true);

            Renderer rend = tempClone.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.enabled = true;
            }
            Destroy(tempClone.GetComponent<Reparable>());
            Animator an = tempClone.AddComponent<Animator>();
            an.runtimeAnimatorController = ac;
            CloneChar cc = tempClone.AddComponent<CloneChar>();
            cc.MaxScale = tempClone.transform.localScale * 0.8f;
            tempClone.transform.localScale = Vector3.one*0.2f;
            
            
            
           
        }
    }

    public void StartReparation()
    {
        StartCoroutine(ShowItem());
    }

}
