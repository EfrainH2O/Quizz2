using System.Collections;
using UnityEngine;

public class Reparable : MonoBehaviour
{
    private Vector3 spawn;

    public float ObVel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spawn = transform.position;
    }

    public void ReturnToPlace(){
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        while (transform.position.y != spawn.y)
        {
            transform.transform.position = Vector3.MoveTowards(transform.position, spawn, ObVel*Time.deltaTime);
            yield return null;
        }
        //Aqui pones que se efectue un efecto en esta posicion de sonido y visual
    }
    
    public IEnumerator Show(){
        yield return new WaitForSeconds (0.3f);
    }

}
