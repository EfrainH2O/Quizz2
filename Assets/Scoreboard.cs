using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    private Image barr;
    public int max, correct;
    float percent;

    [Header("Audio")]
    private AudioClip successSound;
    private AudioClip failureSound;
    private AudioSource audioSource;

    void Awake()
    {
        barr = GetComponent<Image>();
        barr.fillAmount = 0;

        // Load success sound (HappyWheels)
        #if UNITY_EDITOR
        successSound = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Questions/score unity/HappyWheels.mp3");
        failureSound = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Questions/score unity/gameOver.wav");
        #else
        successSound = Resources.Load<AudioClip>("Questions/score unity/HappyWheels");
        failureSound = Resources.Load<AudioClip>("Questions/score unity/gameOver");
        #endif

        if (successSound == null || failureSound == null)
        {
            Debug.LogError($"Could not load audio files. Check paths:\n" +
                          "Assets/Questions/score unity/HappyWheels.mp3\n" +
                          "Assets/Questions/score unity/gameOver.wav");
        }

        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
    }

    void Start()
    {
        percent = (float)correct/(float)max;
        StartCoroutine(Filling());
        
        // Play appropriate sound based on score
        if (percent >= 0.6f) // 60% or higher
        {
            if (successSound != null)
            {
                audioSource.PlayOneShot(successSound);
            }
        }
        else
        {
            if (failureSound != null)
            {
                audioSource.PlayOneShot(failureSound);
            }
        }
    }

    private IEnumerator Filling()
    {
        float fillSpeed = 1.2f;
        while(barr.fillAmount != percent)
        {
            barr.fillAmount = Mathf.Lerp(barr.fillAmount, percent, fillSpeed * Time.deltaTime);
            yield return null;
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (failureSound == null)
        {
            Debug.LogWarning($"Failure sound not assigned on {gameObject.name}");
        }
    }
    #endif
}