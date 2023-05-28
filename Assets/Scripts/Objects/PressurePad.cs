using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : Triggerable
{
    private int count = 0;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Ground") {
             return; 
        }

        count++;

        if (count == 1) {
            
            Vector3 v = transform.position;
            Vector3 s = transform.localScale;
            transform.position = new Vector3(v.x, v.y - s.y / 2, v.z);

            GameEvents.current.TriggerEnter(id);
        }

    }

    private void OnTriggerExit(Collider other) {

        count--;

        if (count == 0) {
            
            triggered = false;
            
            Vector3 v = transform.position;
            Vector3 s = transform.localScale;
            transform.position = new Vector3(v.x, v.y + s.y / 2, v.z);

            GameEvents.current.TriggerExit(id);
        }
    }
}
