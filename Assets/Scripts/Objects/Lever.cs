using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Triggerable
{
    private float rotationAngle = 30;
    private float speed = 60;
    private float interactableTimer = 0;

    void Update()
    {
        if (interactableTimer > 0) {
            interactableTimer -= Time.deltaTime;
        } 

    }

    private void OnTriggerStay(Collider other) {
        if (interactableTimer <= 0 && Input.GetKeyUp(KeyCode.E)) {
            triggered = !triggered;
            if (triggered) {
                GameEvents.current.TriggerEnter(id);
            } else {
                GameEvents.current.TriggerExit(id);
            }

            interactableTimer = 0.5f;

            Vector3 curRotation = transform.rotation.eulerAngles;
            transform.eulerAngles = new Vector3(-curRotation.x, curRotation.y, curRotation.z);
        }
    }
  
}
