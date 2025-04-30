using System.Collections;
using UnityEngine;
using TMPro;

public class RandomLoadingMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private float intervalo = 4f;
    [SerializeField] private float duracionTransicion = 0.5f;

    private Vector2 posicionOriginal;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private int lastIndex = -1;

    private readonly string[] frases = new string[]
    {
        "Resolver fallas empieza con saber cómo funcionan.",
        "Un técnico preparado, resuelve más rápido.",
        "Con cada respuesta correcta, afinas tus habilidades.",
        "Aprender es como diagnosticar: paso a paso.",
        "El conocimiento también se calibra. Aquí lo estás haciendo.",
        "Domina el sistema. Entiende la máquina.",
        "Las herramientas no solo están en la mano, también en la mente.",
        "Eres parte del cambio. Cada acierto te fortalece.",
        "No hay error, solo aprendizaje.",
        "Conocer el ciclo es entender la solución.",
        "Los mejores técnicos también estudian. ¡Estás en buen camino!",
        "Como el tambor de la lavadora, tú también das vueltas... ¡pero avanzas!",
        "Recuerda: lo que sabes, marca la diferencia con el cliente.",
        "Carga completa de conocimientos... casi listo."
    };

    void Start()
    {
        // Referencias necesarias
        rectTransform = texto.GetComponent<RectTransform>();

        canvasGroup = texto.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = texto.gameObject.AddComponent<CanvasGroup>();

        // Guardar posición original
        posicionOriginal = rectTransform.anchoredPosition;

        // Frase inicial para que no se vea vacío
        int indexInicial = Random.Range(0, frases.Length);
        texto.text = frases[indexInicial];
        lastIndex = indexInicial;
        canvasGroup.alpha = 1f;
        rectTransform.anchoredPosition = posicionOriginal;

        // Iniciar animación
        StartCoroutine(AnimarMensajes());
    }

    private IEnumerator AnimarMensajes()
    {
        while (true)
        {
            // Esperar entre frase y frase
            yield return new WaitForSeconds(intervalo);

            // Elegir nueva frase diferente a la anterior
            int index;
            do { index = Random.Range(0, frases.Length); }
            while (index == lastIndex);
            lastIndex = index;

            // Animar salida
            yield return StartCoroutine(TransicionOut());

            // Cambiar texto
            texto.text = frases[index];

            // Animar entrada
            yield return StartCoroutine(TransicionIn());
        }
    }

    private IEnumerator TransicionOut()
    {
        float t = 0f;
        Vector2 startPos = posicionOriginal;
        Vector2 endPos = posicionOriginal - new Vector2(0, 30);

        while (t < duracionTransicion)
        {
            t += Time.deltaTime;
            float progress = t / duracionTransicion;

            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            yield return null;
        }
    }

    private IEnumerator TransicionIn()
    {
        float t = 0f;
        Vector2 startPos = posicionOriginal + new Vector2(0, 30);
        Vector2 endPos = posicionOriginal;

        rectTransform.anchoredPosition = startPos;
        canvasGroup.alpha = 0f;

        while (t < duracionTransicion)
        {
            t += Time.deltaTime;
            float progress = t / duracionTransicion;

            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
            yield return null;
        }
    }
}
