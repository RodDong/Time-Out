using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaBallTimeFreeze : MonoBehaviour
{
    private TimeFreeze tf;
    private Vector3 velocity;
    private Vector3 angularVelocity;
    private bool frozen;
    private RigidbodyConstraints initialContraints;
    // Start is called before the first frame update
    void Start()
    {
        velocity = gameObject.GetComponent<Rigidbody>().velocity;
        angularVelocity = gameObject.GetComponent<Rigidbody>().angularVelocity;
        tf = GameObject.FindAnyObjectByType<TimeFreeze>();
        frozen = false;
        initialContraints = gameObject.GetComponent<Rigidbody>().constraints;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (tf.freezed && !frozen) {
            velocity = rb.velocity;
            angularVelocity = rb.angularVelocity;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            frozen = true;
        } else if (frozen && !tf.freezed) {
            rb.constraints = initialContraints;
            rb.velocity = velocity;
            rb.angularVelocity = angularVelocity;
            frozen = false;
        }
    }
}
