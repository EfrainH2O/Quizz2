using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Agregamos esta referencia

[RequireComponent(typeof(Button))] // Aseguramos que haya un botón
public class HoverButtonSoundQuiz : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    private AudioSource audioSource;
    private Button button;

    void Start()
    {
        // Inicializar componentes
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        // Obtener referencia al botón
        button = GetComponent<Button>();
        
        // Agregar el listener para el clic
        button.onClick.AddListener(() => PlayClickSound());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSound();
    }

    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            Debug.Log("Reproduciendo sonido de clic"); // Debug para verificar
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("Falta asignar el sonido de clic o el AudioSource");
        }
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (clickSound == null)
        {
            Debug.LogWarning("No hay sonido de clic asignado en " + gameObject.name);
        }
    }
    #endif
}
