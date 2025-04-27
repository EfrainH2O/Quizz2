using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class PostQuizResult : MonoBehaviour
{
    public static PostQuizResult Instance { get; private set; }
    [SerializeField]
    private string postUrl = "http://localhost:5011/QuizEstudiante/Quiz"; // url de la api
     private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // aguante cambios de escenas
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
        // Esperar a que TokenManager esté listo (por seguridad)
        while (string.IsNullOrEmpty(TokenManager.Instance.Token) || TokenManager.Instance.CursoId == 0)
        {
            Debug.Log("[PostQuizResult] Esperando Token y CursoId...");
            yield return new WaitForSeconds(0.5f);
        }

        QuizResultData data = new QuizResultData
        {
            cal = calificacion,
            cursoId = TokenManager.Instance.CursoId
        };

        string json = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(postUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 🔥 Agregamos también el Authorization con el token
        request.SetRequestHeader("Authorization", "Bearer " + TokenManager.Instance.Token);

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
        public int cal;
        public int cursoId;
    }
}
