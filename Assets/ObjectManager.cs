using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    private List<Reparable> electrodomesticos;
    private int ListSize; 
    private int actualItem;
    
    private GameObject tempClone;
    public Vector3 clonePosition = new Vector3(19f, 0.8f, -9.5f); 
    public float cloneScaleFactor = 0.6f; // factor de escala 
    public float moveDuration = 0.5f; // para q se muevan extraño
    public float startHeightOffset = -2f;

    
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
            StartCoroutine(MoveObject(electrodomesticos[actualItem].gameObject, startHeightOffset, 0));

        }else{
            //Efecto de danio
        }
        //destruir copia al contestar preguunta
        if (tempClone != null)
        {
            StartCoroutine(MoveObject(tempClone, 0, startHeightOffset, true));
            //Destroy(tempClone);
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
        // copia temporal del objeto antes de que aparezca
        if (actualItem < electrodomesticos.Count)
        {
            tempClone = Instantiate(electrodomesticos[actualItem].gameObject, clonePosition, Quaternion.identity);
            //que vea para enfrente
            tempClone.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            //cosillas para asegurarme q se vea
            tempClone.SetActive(true);
            //scale
            tempClone.transform.localScale = electrodomesticos[actualItem].transform.localScale * cloneScaleFactor;
            Renderer rend = tempClone.GetComponent<Renderer>();
            if (rend != null)
            {
            rend.enabled = true;
            }

            //quitar scripts
            foreach (MonoBehaviour script in tempClone.GetComponents<MonoBehaviour>())
            {
                script.enabled = false;
            }
            StartCoroutine(MoveObject(tempClone, startHeightOffset, 0));
        }
    }

    public void StartReparation(){
        StartCoroutine(ShowItem());
    }
    private IEnumerator MoveObject(GameObject obj, float startOffset, float endOffset, bool destroyAfter = false)
    {
        Vector3 startPos = obj.transform.position;
        Vector3 targetPos = new Vector3(startPos.x, startPos.y + endOffset, startPos.z);
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            obj.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = targetPos;
        
        if (destroyAfter)
        {
            Destroy(obj); // Destruir clon después de la animación
        }
    }

}
