using UnityEngine;

public class spawnsound : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip growthSound;
    [SerializeField] private AudioClip shrinkSound;
    private AudioSource audioSource;

    private void Start()
    {
        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }

        // Play growth sound when object appears
        PlayGrowthSound();
    }

    private void OnEnable()
    {
        PlayGrowthSound();
    }

    private void OnDisable()
    {
        PlayShrinkSound();
    }

    private void OnDestroy()
    {
        PlayShrinkSound();
    }

    private void PlayGrowthSound()
    {
        if (growthSound != null)
        {
            AudioSource.PlayClipAtPoint(growthSound, transform.position, 1f);
        }
    }

    private void PlayShrinkSound()
    {
        if (shrinkSound != null)
        {
            AudioSource.PlayClipAtPoint(shrinkSound, transform.position, 1f);
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (growthSound == null)
            Debug.LogWarning($"Growth sound not assigned on {gameObject.name}");
        if (shrinkSound == null)
            Debug.LogWarning($"Shrink sound not assigned on {gameObject.name}");
    }
    #endif
}