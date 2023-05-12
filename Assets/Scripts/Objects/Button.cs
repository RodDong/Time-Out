using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Triggerable
{

    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyUp(KeyCode.E)) {
            GameEvents.current.TriggerEnter(id);
        }
    }
}
