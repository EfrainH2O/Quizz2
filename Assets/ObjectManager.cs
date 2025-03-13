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
    public float moveDuration = 0.5f; // para que se muevan suavemente
    public float startHeightOffset = -2f; // Altura de bajada
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
        StartCoroutine(HideObjects());
    }

    public void RepairResult(bool result)
    {
        if (result)
        {
            electrodomesticos[actualItem].MoveUp(); // Subir el objeto original a su posición inicial
            correctParticle.Stop();
            Debug.Log(correctParticle.isPlaying);

            correctParticle.Play();
        
        }
        else
        {
            // Efecto de daño (Aquí puedes agregar algún efecto visual o de sonido)
            wrongParticle.Stop();
            Debug.Log(wrongParticle.isPlaying);
            wrongParticle.Play();
        }

        // Destruir copia (clon) al contestar la pregunta
        if (tempClone != null)
        {
            Destroy(tempClone);
            
        }

        // Siguiente objeto
        actualItem++;
        StartReparation();
    }

    private IEnumerator HideObjects()
    {
        foreach (Reparable r in electrodomesticos)
        {
            r.MoveDown(); // Mueve el objeto hacia abajo
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator ShowItem()
    {
        yield return new WaitForSeconds(0.6f);

        if (actualItem < electrodomesticos.Count)
        {
            tempClone = Instantiate(electrodomesticos[actualItem].gameObject, clonePosition, Quaternion.identity);
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
        }
    }

    public void StartReparation()
    {
        StartCoroutine(ShowItem());
    }
}
