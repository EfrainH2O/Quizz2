using UnityEngine;

public class CloneChar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Stop(){
        Rigidbody rg = GetComponent<Rigidbody>();
        rg.useGravity = false;
        rg.linearDamping = 0.5f;
        rg.angularDamping = 0.5f;
    }
}
