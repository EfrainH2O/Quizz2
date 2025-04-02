using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class FloatingButton3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Movement Settings")]
    public float floatSpeed = 4f;           // Velocidad del movimiento flotante
    public float floatAmount = 4f;          // Distancia del movimiento arriba/abajo
    public float hoverScaleMultiplier = 1.2f; // Tamaño al hacer hover
    public float scaleSpeed = 5f;           // Velocidad de la animación de escala

    [Header("Audio Settings")]
    [SerializeField] private AudioClip hoverSound;       // Sonido al pasar el mouse
    [SerializeField] private AudioClip clickSound;       // Sonido al hacer clic
    [SerializeField] private AudioClip transitionSound;  // Sonido de transición
    private AudioSource audioSource;                     // Componente para reproducir audio

    private Vector3 originalPosition;
    private Vector3 originalScale;

    void Start()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;

        // Inicializar el componente de audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }

        // Verificar sonidos
        if (clickSound == null || hoverSound == null || transitionSound == null)
        {
            Debug.LogWarning($"Faltan asignar sonidos en {gameObject.name}");
        }
    }

    void Update()
    {
        // Movimiento sutil de flotación constante
        float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObject(originalScale * hoverScaleMultiplier));

        // Reproducir sonido de hover
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
        // Reproducir sonido de clic
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Iniciar la secuencia de transición
        StartCoroutine(TransitionSequence());
    }

    private IEnumerator TransitionSequence()
    {
        // Efecto visual de presión
        StartCoroutine(ScaleObject(originalScale * 0.9f));
        
        // Esperar un momento antes de reproducir el sonido de transición
        yield return new WaitForSeconds(0.2f);
        
        // Reproducir sonido de transición
        if (transitionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }
        
        // Añadir delay de 2 segundos
        yield return new WaitForSeconds(10);
        
        // Volver al tamaño original
        StartCoroutine(ScaleObject(originalScale));
    }
    public void ActivateButton()
{
    gameObject.SetActive(true); // Activa el botón
    transform.localScale = originalScale; // Restablece la escala
    transform.position = originalPosition; // Restablece la posición flotante

    // Asegurar que el AudioSource siga funcionando
    if (audioSource == null)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
    }

    // Pequeño delay antes de permitir clics
    StartCoroutine(EnableClickAfterDelay());
}

private IEnumerator EnableClickAfterDelay()
{
    yield return new WaitForSeconds(0.1f); // Delay de 100ms
    GetComponent<Button>().interactable = true; // Habilitar interacción del botón UI
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
        // Verificar componentes en el editor
        if (hoverSound == null)
            Debug.LogWarning($"Hover sound no asignado en {gameObject.name}");
        if (clickSound == null)
            Debug.LogWarning($"Click sound no asignado en {gameObject.name}");
        if (transitionSound == null)
            Debug.LogWarning($"Transition sound no asignado en {gameObject.name}");
    }
    #endif
}