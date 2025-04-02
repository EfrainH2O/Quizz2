using UnityEngine;

public class panel : MonoBehaviour
{
    public GameObject instructionPanel; // Panel de instrucciones
    public GameObject btnInstrucciones; // Botón de instrucciones
    public GameObject btnEmpezar; // Botón de Start

    void Start()
    {
        instructionPanel.SetActive(false); // Aseguramos que el panel de instrucciones esté oculto al inicio
        btnInstrucciones.SetActive(true); // Botón de instrucciones visible al inicio
    }

    public void ShowPanel()
    {
        instructionPanel.SetActive(true); // Muestra el panel de instrucciones
    }

    public void Back()
    {
        instructionPanel.SetActive(false); // Oculta el panel de instrucciones
    }

    public void StartGame()
    {
        btnInstrucciones.SetActive(false); // Oculta el botón de instrucciones
        btnEmpezar.SetActive(false); // Oculta el botón de Start (opcional)
        
        // Aquí puedes agregar más lógica para iniciar el juego, como activar otros elementos
    }
}
