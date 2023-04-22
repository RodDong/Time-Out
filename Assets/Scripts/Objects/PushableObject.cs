using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }   
    }

     [SerializeField] Vector3 force;

    private void OnCollisionStay(Collision collision) {
        
        print("...");
        if (collision.gameObject.tag == "Player") {
            
            print("***");
            transform.GetComponent<Rigidbody>().velocity = force;
        }
    }
}
