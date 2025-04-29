using UnityEngine;
using System.Collections;

public class loadingGIF : MonoBehaviour
{
    public float delayBeforeShowing = 0.5f; // tiempo antes de mostrar el GIF
    public float timeBeforeHide = 1.0f;     // tiempo antes de esconderlo (desde fuera)

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // Agrega un CanvasGroup si no lo tiene para controlar visibilidad
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f; // Oculto al inicio
    }

    private void Start()
    {
        StartCoroutine(ShowAfterDelay());
    }

     IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeShowing);
        canvasGroup.alpha = 1f; // Mostrarlo
    }

    public void Hide()
    {
        StartCoroutine(HideAnimation());
    }

    IEnumerator HideAnimation()
    {
        yield return new WaitForSeconds(timeBeforeHide);
        canvasGroup.alpha = 0f; // Ocultarlo visualmente
        yield return new WaitForSeconds(0.1f); // pequeña pausa por si acaso
        Destroy(gameObject); // opcional
    }
}
