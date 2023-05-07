using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private bool inContact;

    void Start() {
        inContact = false;
    }

    // Start is called before the first frame update
    void Update()
    {
        if (inContact) {
            Vector3 newPos = transform.localPosition + force * Time.deltaTime;
            transform.localPosition = newPos;
        }
    }

    [SerializeField] Vector3 force;

    private void OnCollisionEnter(Collision collision) {
        
        print("...");
        if (collision.gameObject.tag == "Player") {
            inContact = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        
        print("...");
        if (collision.gameObject.tag == "Player") {
            inContact = false;
        }
    }

}
