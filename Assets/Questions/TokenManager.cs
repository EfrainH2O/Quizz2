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
            DontDestroyOnLoad(gameObject); //no se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

        void Start()
    {
        // SOLO PARA PRUEBA: Forzar Token y CursoId
        Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEwMDAiLCJyb2xlIjoiMCIsIm5iZiI6MTc0NTg0MzE3NCwiZXhwIjoxNzUxMjQzMTc0LCJpYXQiOjE3NDU4NDMxNzQsImlzcyI6IllvdXJJc3N1ZXIiLCJhdWQiOiJZb3VyQXVkaWVuY2UifQ.yolFhANDLySqF2q1MLTJT-U7eRVmEvq6wFme_M-7S2A";
        CursoId = 1;

        Debug.Log($"[TokenManager] Token de prueba asignado: {Token}");
        Debug.Log($"[TokenManager] CursoId de prueba asignado: {CursoId}");
    }


    // recibe el token desde react
    public void ReceiveToken(string token)
    {
        Debug.Log($"[TokenManager] Token recibido: {token}"); 
        Token = token;
    }

    // metodo para recibir el cursoid
    public void ReceiveCursoId(string cursoIdString)
    {
        if (int.TryParse(cursoIdString, out int cursoId))
        {
            Debug.Log($"[TokenManager] Curso ID recibido: {cursoId}"); 
            CursoId = cursoId;
        }
        else
        {
            Debug.LogError("[TokenManager] Error al convertir CursoId a int.");
        }
    }
}
