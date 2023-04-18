using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollableObject : MonoBehaviour
{
    [SerializeField] Vector3 force;
    [SerializeField] float maxVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = transform.GetComponent<Rigidbody>();
        if (maxVelocity >= 0 && rb != null) {
            if (rb.velocity.magnitude > maxVelocity) {
                rb.velocity = Vector3.Normalize(rb.velocity) * maxVelocity;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            transform.GetComponent<Rigidbody>().velocity = force;
        }
    }
}
