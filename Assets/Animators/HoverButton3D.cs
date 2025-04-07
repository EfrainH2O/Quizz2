using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class CloseButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Movement Settings")]
    public float floatSpeed = 4f;
    public float floatAmount = 4f;
    public float hoverScaleMultiplier = 1.2f;
    public float scaleSpeed = 5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip transitionSound;
    private AudioSource audioSource;

    private Vector3 originalPosition;
    private Vector3 originalScale;

    void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }

        if (clickSound == null || hoverSound == null || transitionSound == null)
        {
            Debug.LogWarning($"Faltan asignar sonidos en {gameObject.name}");
        }
    }

    void Update()
    {
        float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObject(originalScale * hoverScaleMultiplier));

        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObject(originalScale));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        if (gameObject.activeInHierarchy)
    {
        StartCoroutine(TransitionSequence());
    }

    }

    private IEnumerator TransitionSequence()
    {
        StartCoroutine(ScaleObject(originalScale * 0.9f));
        
        yield return new WaitForSeconds(0.2f);
        
        if (transitionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }
        
        yield return new WaitForSeconds(10f);
        
        StartCoroutine(ScaleObject(originalScale));
    }

    public void ActivateButton()
    {
        gameObject.SetActive(true);
        transform.localScale = originalScale;
        transform.position = originalPosition;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }

        StartCoroutine(EnableClickAfterDelay());
    }

    private IEnumerator EnableClickAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<Button>().interactable = true;
    }

    private IEnumerator ScaleObject(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
            yield return null;
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (hoverSound == null)
            Debug.LogWarning($"Hover sound no asignado en {gameObject.name}");
        if (clickSound == null)
            Debug.LogWarning($"Click sound no asignado en {gameObject.name}");
        if (transitionSound == null)
            Debug.LogWarning($"Transition sound no asignado en {gameObject.name}");
    }
    #endif
}