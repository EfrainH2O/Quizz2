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

    [Header("Audio Effects")]
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;

    private void Start()
    {
        // // Initialize list and get all Reparable objects
        // electrodomesticos = new List<Reparable>(FindObjectsByType<Reparable>(FindObjectsSortMode.None));
        // actualItem = 0;
        // ListSize = DataManager.Instance.questionsCount;
        // Debug.Log($"[ObjectManager] Clones esperados: {ListSize}");
        
            
        // // Reduce list to desired size
        // while (electrodomesticos.Count > ListSize)
        // {
        //     electrodomesticos.RemoveAt(Random.Range(0, electrodomesticos.Count));
        // }

        // // Setup each reparable object
        // foreach(Reparable r in electrodomesticos)
        // {
        //     r.transform.position += new Vector3(0, clonePosition.position.y, 0);
        //     r.ObVel = ObVel;
        //     r.gameObject.SetActive(false);
        // }

        // if (correctSound == null || wrongSound == null)
        // {
        //     Debug.LogWarning($"Missing sound effects on {gameObject.name}");
        // }
    }


    public void SetupObjects()
    {
        ListSize = DataManager.Instance.questionsCount;
        Debug.Log($"[ObjectManager] Clones esperados (preguntas): {ListSize}");

        electrodomesticos = new List<Reparable>(FindObjectsByType<Reparable>(FindObjectsSortMode.None));
        Debug.Log($"[ObjectManager] Objetos Reparable encontrados en escena: {electrodomesticos.Count}");

        actualItem = 0;

        int objetosADesaparecer = ListSize;
        int desaparecidos = 0;

        for (int i = 0; i < electrodomesticos.Count; i++)
        {
            if (desaparecidos < objetosADesaparecer)
            {
                electrodomesticos[i].transform.position += new Vector3(0, clonePosition.position.y, 0);
                electrodomesticos[i].ObVel = ObVel;
                electrodomesticos[i].gameObject.SetActive(false);
                desaparecidos++;
            }
            else
            {
                electrodomesticos[i].gameObject.SetActive(true);
            }
        }

        Debug.Log($"[ObjectManager] Total de objetos ocultados: {desaparecidos}");
        Debug.Log($"[ObjectManager] Total de objetos visibles restantes: {electrodomesticos.Count - desaparecidos}");

        if (correctSound == null || wrongSound == null)
        {
            Debug.LogWarning($"[ObjectManager] Missing sound effects on {gameObject.name}");
        }

        Debug.Log("[ObjectManager] Setup de clones terminado.");
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
    }

    private IEnumerator ShowItem()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (actualItem >= DataManager.Instance.questionsCount)
        {
            Debug.Log("[ObjectManager] Ya no se generan más clones. Todos los reparables fueron usados.");
            yield break;
        }

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
            tempClone.transform.localScale = Vector3.one * 0.2f;
        }
    }

    private IEnumerator PlayCorrectEffect()
    {
        // Play correct sound
        if (correctSound != null)
        {
            AudioSource.PlayClipAtPoint(correctSound, Camera.main.transform.position, 1f);
        }

        if (correctParticle == null) yield break;

        correctParticle.gameObject.SetActive(true);
        correctParticle.Play();

        yield return WaitForParticleToFinish(correctParticle);
        correctParticle.gameObject.SetActive(false);

        electrodomesticos[actualItem].gameObject.SetActive(true);
        electrodomesticos[actualItem].ReturnToPlace();

        actualItem++;
        StartReparation();
    }

    private IEnumerator PlayWrongEffect()
    {
        // Play wrong sound
        if (wrongSound != null)
        {
            AudioSource.PlayClipAtPoint(wrongSound, Camera.main.transform.position, 1f);
        }

        if (wrongParticle == null) yield break;

        wrongParticle.gameObject.SetActive(true);
        wrongParticle.Play();

        yield return WaitForParticleToFinish(wrongParticle);
        wrongParticle.gameObject.SetActive(false);
        
        actualItem++;
        StartReparation();
    }

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


    



    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (correctSound == null)
            Debug.LogWarning($"Correct sound not assigned on {gameObject.name}");
        if (wrongSound == null)
            Debug.LogWarning($"Wrong sound not assigned on {gameObject.name}");
    }
    #endif
}