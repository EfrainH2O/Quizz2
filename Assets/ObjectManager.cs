using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private List<Reparable> electrodomesticos;
    private int ListSize; 
    private int actualItem;
    [SerializeField]
    private float ObVel; 
    
    private GameObject tempClone;
    [SerializeField]
    private Transform clonePosition; 
    [SerializeField ]
    private PhysicsMaterial NoBouncy;
    public float cloneScaleFactor = 0.6f; // factor de escala 
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
        Debug.Log(electrodomesticos.Count);
        foreach(Reparable r in electrodomesticos){
            r.transform.position += new Vector3(0,clonePosition.position.y,0);
            r.ObVel = ObVel;
            r.gameObject.SetActive(false);
        }
    }


    public void RepairResult(bool result)
    {
        tempClone.GetComponent<CloneChar>().FinalAction(result);
        if (result)
        {
            correctParticle.Play();
            electrodomesticos[actualItem].gameObject.SetActive(true);
            electrodomesticos[actualItem].ReturnToPlace();
        }
        else
        {
            Debug.Log(wrongParticle.isPlaying);
            wrongParticle.Play();
        }
        
        // Siguiente objeto
        actualItem++;
        StartReparation();
    }



    private IEnumerator ShowItem()
    {
        yield return new WaitForSeconds(0.2f);

        if (actualItem < electrodomesticos.Count)
        {
            tempClone = Instantiate(electrodomesticos[actualItem].gameObject, clonePosition.position, Quaternion.identity);

            tempClone.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            tempClone.SetActive(true);
            tempClone.transform.localScale = electrodomesticos[actualItem].transform.localScale * cloneScaleFactor;

            Renderer rend = tempClone.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.enabled = true;
            }

            foreach (MonoBehaviour script in tempClone.GetComponents<MonoBehaviour>())
            {
                script.enabled = false;
            }
            tempClone.transform.Rotate(0,20f,5f);
            Rigidbody rg = tempClone.AddComponent<Rigidbody>();
            rg.useGravity = true;
            rg.mass = 10f;
            tempClone.AddComponent<CloneChar>();
            tempClone.GetComponent<CloneChar>().NBouncy = NoBouncy;
            Destroy(tempClone.GetComponent<Reparable>());
        }
    }

    public void StartReparation()
    {
        StartCoroutine(ShowItem());
    }

}
