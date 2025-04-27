using UnityEngine;

public class TokenManager : MonoBehaviour
{
    public static TokenManager Instance { get; private set; }

    [Header("Datos recibidos")]
    public string Token { get; private set; } = "";
    public int CursoId { get; private set; } = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para recibir el token desde React
    public void ReceiveToken(string token)
    {
        Debug.Log($"[TokenManager] Token recibido: {token}"); // <-- ✅ Aquí imprime
        Token = token;
    }

    // Método para recibir el curso ID desde React
    public void ReceiveCursoId(string cursoIdString)
    {
        if (int.TryParse(cursoIdString, out int cursoId))
        {
            Debug.Log($"[TokenManager] Curso ID recibido: {cursoId}"); // <-- ✅ Aquí imprime
            CursoId = cursoId;
        }
        else
        {
            Debug.LogError("[TokenManager] Error al convertir CursoId a int.");
        }
    }
}
