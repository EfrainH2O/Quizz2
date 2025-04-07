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
    
    [SerializeField] private AnimatorController ac;
    [SerializeField] private float ObVel; 
    [SerializeField] private Transform clonePosition;
    
    private GameObject tempClone;
    public ParticleSystem correctParticle;
    public ParticleSystem wrongParticle;

    private void Start()
    {
        // Initialize list and get all Reparable objects
        electrodomesticos = new List<Reparable>(FindObjectsByType<Reparable>(FindObjectsSortMode.None));
        actualItem = 0;
        ListSize = DataManager.Instance.questionsCount;
        
        // Reduce list to desired size
        while (electrodomesticos.Count > ListSize)
        {
            electrodomesticos.RemoveAt(Random.Range(0, electrodomesticos.Count));
        }

        // Setup each reparable object
        foreach(Reparable r in electrodomesticos)
        {
            r.transform.position += new Vector3(0, clonePosition.position.y, 0);
            r.ObVel = ObVel;
            r.gameObject.SetActive(false);
        }
    }

    public void RepairResult(bool result)
    {
        tempClone.GetComponent<CloneChar>()?.FinalAction(result);
        if (result)
        {
            StartCoroutine(PlayCorrectEffect());
        }
        else
        {
            StartCoroutine(PlayWrongEffect());
        }
        // actualItem++;
        // StartReparation();
        
        
    }

    private IEnumerator ShowItem()
    {
        yield return new WaitForSeconds(0.5f);

        if (actualItem < electrodomesticos.Count)
        {
            // Create and setup clone
            tempClone = Instantiate(electrodomesticos[actualItem].gameObject, clonePosition.position, Quaternion.identity);
            tempClone.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            tempClone.SetActive(true);

            // Setup renderer
            Renderer rend = tempClone.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.enabled = true;
            }

            // Setup components
            Destroy(tempClone.GetComponent<Reparable>());
            
            // Add animator
            Animator an = tempClone.AddComponent<Animator>();
            an.runtimeAnimatorController = ac;
            
            // Add clone character component
            CloneChar cc = tempClone.AddComponent<CloneChar>();
            cc.MaxScale = tempClone.transform.localScale * 0.8f;
            tempClone.transform.localScale = Vector3.one * 0.2f;
        }
    }
    private IEnumerator PlayCorrectEffect()
    {
        if (correctParticle == null) yield break;

        correctParticle.gameObject.SetActive(true);
        correctParticle.Play();

        yield return WaitForParticleToFinish(correctParticle);
        correctParticle.gameObject.SetActive(false);

        electrodomesticos[actualItem].gameObject.SetActive(true);
        electrodomesticos[actualItem].ReturnToPlace();
        //yield return new WaitForSeconds(0.5f);

        actualItem++;
        StartReparation();
    }

    private IEnumerator PlayWrongEffect()
    {
        if (wrongParticle == null) yield break;

        wrongParticle.gameObject.SetActive(true);
        wrongParticle.Play();

        yield return WaitForParticleToFinish(wrongParticle);
        
        //yield return new WaitForSeconds(0.5f);
        actualItem++;
        StartReparation();
        //wrongParticle.gameObject.SetActive(false);

    }

    //extra
    private IEnumerator WaitForParticleToFinish(ParticleSystem particle)
    {
        if (particle == null || !particle.gameObject.activeInHierarchy)
        {
            yield break; 
        }
        while (particle.isPlaying)
        {
            yield return null;
        }

    }

    public void StartReparation()
    {
        StartCoroutine(ShowItem());
    }
}
