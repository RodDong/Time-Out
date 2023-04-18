using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : Triggerable
{

    private int count = 0;

    private void OnTriggerEnter(Collider other) {
        if (count != 0 || other.tag == "Ground") { return; }

        count++;
        triggered = true;

        Vector3 v = transform.position;
        Vector3 s = transform.localScale;
        transform.position = new Vector3(v.x, v.y - s.y / 2, v.z);
    }

    private void OnTriggerExit(Collider other) {
        if (count == 0) { return; }

        triggered = false;
        count--;
        Vector3 v = transform.position;
        Vector3 s = transform.localScale;
        transform.position = new Vector3(v.x, v.y + s.y / 2, v.z);
    }
}
