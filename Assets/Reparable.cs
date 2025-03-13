using System.Collections;
using UnityEngine;

public class Reparable : MonoBehaviour
{
    private Vector3 originalPosition;
    private float lastMoveOffset; // Guarda cuánto bajó el objeto

    private void Start()
    {
        originalPosition = transform.position; // Guarda la posición inicial
    }

    private IEnumerator MoveObject(float offset)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(startPos.x, startPos.y + offset, startPos.z);
        float elapsedTime = 0;
        float moveDuration = 0.5f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

    public void MoveDown()
    {
        lastMoveOffset = -7f; // Baja 5 unidades (o el valor que desees)
        StartCoroutine(MoveObject(lastMoveOffset));
    }

    public void MoveUp()
    {
        StartCoroutine(MoveObject(-lastMoveOffset)); // Sube la misma cantidad que bajó
    }
}
