using System.Collections;
using UnityEngine;

public class Reparable : MonoBehaviour
{
    private Vector3 spawn;
    public float ObVel;

    [Header("Audio Effects")]
    [SerializeField] private AudioClip createSound;
    [SerializeField] private AudioClip destroySound;
    private AudioSource audioSource;

    [Header("Visual Effects")]
    [SerializeField] private GameObject createVFX;
    [SerializeField] private GameObject destroyVFX;

    void Awake()
    {
        spawn = transform.position;
        
        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }

        PlayCreateEffect();
    }

    private void OnDestroy()
    {
        PlayDestroyEffect();
    }

    public void ReturnToPlace()
    {
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        while (transform.position.y != spawn.y)
        {
            transform.transform.position = Vector3.MoveTowards(transform.position, spawn, ObVel * Time.deltaTime);
            yield return null;
        }
        PlayCreateEffect();
    }

    private void PlayCreateEffect()
    {
        // Create a temporary audio source at this position
        if (createSound != null)
        {
            AudioSource.PlayClipAtPoint(createSound, transform.position, 1f);
        }

        // Spawn visual effect
        if (createVFX != null)
        {
            Instantiate(createVFX, transform.position, Quaternion.identity);
        }
    }

    private void PlayDestroyEffect()
    {
        // Create a temporary audio source at this position
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position, 1f);
        }

        // Spawn visual effect
        if (destroyVFX != null)
        {
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
        }
    }
    
    public IEnumerator Show()
    {
        yield return new WaitForSeconds(0.3f);
    }

    #if UNITY_EDITOR
    // private void OnValidate()
    // {
    //     if (createSound == null)
    //         Debug.LogWarning($"Create sound not assigned on {gameObject.name}");
    //     if (destroySound == null)
    //         Debug.LogWarning($"Destroy sound not assigned on {gameObject.name}");
    // }
    #endif
}