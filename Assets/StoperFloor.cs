using UnityEngine;

public class StoperFloor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<CloneChar>()?.Stop();
    }
}
