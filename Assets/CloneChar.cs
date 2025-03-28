using System.Collections;
using UnityEngine;

public class CloneChar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody rg;
    private Collider bx;
    public PhysicsMaterial NBouncy;

    bool Ofirst;
    void Awake()
    {
        Ofirst = true;
        rg = GetComponent<Rigidbody>();
        bx = GetComponent<Collider>();


    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Stop(){
        if(Ofirst){
            rg.useGravity = false;
            rg.linearDamping = 0.8f;
            rg.angularDamping = 0.7f;
            Ofirst = false;
        }
        
    }

    public void FinalAction(bool result){
        
        if(result){
            //Efectos Sonoros
            //Efecto visual de estela
            rg.mass = 5f;
            rg.linearDamping = 0f;
            rg.linearVelocity = Vector3.zero;
            rg.angularVelocity = Vector3.zero;
            rg.AddForce(new Vector3(0,80f,0), ForceMode.Impulse);
        }else{
            //Efecto sonoro malo
            rg.linearDamping = 0f;
            rg.angularDamping = 0f;
            rg.automaticCenterOfMass = true;
            bx.material = NBouncy;
            rg.inertiaTensor = new Vector3(2f,15f,2f);
            rg.useGravity = true;
        } 
        StartCoroutine(AutoDestruction());

    }

    public IEnumerator AutoDestruction(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
