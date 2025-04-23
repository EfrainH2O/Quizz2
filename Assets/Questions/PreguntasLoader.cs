using UnityEngine;
using System.Collections;

public class PreguntasLoader : MonoBehaviour
{
    private bool cargando = false;

    public void AlPresionarBoton() //* Esta función se llama desde el botón de inicio
    {
        if (!cargando)
        {
            StartCoroutine(CargarYEmpezar());
        }
    }

    private IEnumerator CargarYEmpezar()
    {
        cargando = true;

        APIQuestionManager.Instance.CargarPreguntasDesdeAPI(); //* Llamamos a la API

        yield return new WaitUntil(() => APIQuestionManager.Instance.GetQuestions().Count > 0); //* Esperamos a que lleguen

        Debug.Log(" Preguntas cargadas y listo para empezar el quiz "); //* Confirmación visual

        if (DataManager.Instance != null)
        {
            Debug.Log(" DataManager encontrado, iniciando preguntas.");
            DataManager.Instance.StartQuestions();
        }
        else
        {
            Debug.LogError(" DataManager no encontrado en la escena. ¿Está presente y activo?");
        }


        cargando = false;
    }
}
