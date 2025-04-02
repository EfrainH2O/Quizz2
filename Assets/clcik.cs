using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class clcik : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip clickSound;       // Sonido al hacer clic
    private AudioSource audioSource;                     // Componente para reproducir audio
     [Header("Movement Settings")]
     public float floatSpeed = 4f; // Velocidad del movimiento flotante
    public float floatAmount = 4f; // Distancia del movimiento arriba/abajo
    public float hoverScaleMultiplier = 1.2f; // Tamaño al hacer hover
    public float scaleSpeed = 5f; // Velocidad de la animación de escala

    private Vector3 originalPosition;
    private Vector3 originalScale;





    void Start()
    {
        // Inicializar el componente de audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 10f;
             originalPosition = transform.position;
        originalScale = transform.localScale;

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
    }
     public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleObject(originalScale));
    }

    private IEnumerator ScaleObject(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
            yield return null;
        }
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        // Reproducir sonido de clic
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
    public void PlayClickSound()
{
    OnPointerClick(null);
}

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (clickSound == null)
            Debug.LogWarning($"Click sound no asignado en {gameObject.name}");
    }
    #endif
    
}