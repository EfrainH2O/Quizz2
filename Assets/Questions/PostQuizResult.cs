using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class PostQuizResult : MonoBehaviour
{
    public static PostQuizResult Instance { get; private set; }

    [SerializeField]
    private string postUrl = "http://130.213.216.127:5011/QuizEstudiante/Quiz"; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SendResult(int correctas, int totalPreguntas)
    {
        int calificacion = Mathf.RoundToInt(((float)correctas / totalPreguntas) * 100f);
        Debug.Log($"[PostQuizResult] Calculando calificación: {correctas}/{totalPreguntas} = {calificacion}");

        StartCoroutine(PostResult(calificacion));
    }

    private IEnumerator PostResult(int calificacion)
    {
        QuizResultData data = new QuizResultData
        {
            id_curso = TokenManager.Instance.CursoId, // Ahora sacamos el curso de TokenManager
            cal = calificacion
        };

        string json = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        Debug.Log($"[PostQuizResult] Mandando resultado al servidor: {json}");

        UnityWebRequest request = new UnityWebRequest(postUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //  Mandamos el Token como autorización
        string authToken = TokenManager.Instance.Token;
        if (!string.IsNullOrEmpty(authToken))
        {
            request.SetRequestHeader("Authorization", "Bearer " + authToken);
        }
        else
        {
            Debug.LogWarning("[PostQuizResult] No se encontró token, enviando sin autorización.");
        }

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("[PostQuizResult] Resultado enviado correctamente: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("[PostQuizResult] Error enviando resultado: " + request.error);
        }
    }

    [System.Serializable]
    private class QuizResultData
    {
        public int id_curso;
        public int cal;
    }
}
