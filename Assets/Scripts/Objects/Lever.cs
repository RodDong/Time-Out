using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Triggerable
{
    private float rotationAngle = 30;
    private float speed = 60;
    private float interactableTimer = 0;
    [SerializeField] GameObject LeverStick;
    private bool inRange = false;

    void Update()
    {
        if (interactableTimer > 0) {
            interactableTimer -= Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.E) && inRange)
        {

            triggered = !triggered;
            if (triggered)
            {
                GameEvents.current.TriggerEnter(id);
            }
            else
            {
                GameEvents.current.TriggerExit(id);
            }

            interactableTimer = 0.5f;

            if (!LeverStick)
            {
                Debug.LogError("LeverStick is missing");
            }
            else
            {
                Vector3 curRotation = LeverStick.transform.rotation.eulerAngles;
                if (triggered)
                {
                    LeverStick.transform.eulerAngles = new Vector3(curRotation.x, curRotation.y, -90);
                }
                else
                {
                    LeverStick.transform.eulerAngles = new Vector3(curRotation.x, curRotation.y, 0);
                }

            }

        }

    }

    private void OnTriggerEnter(Collider other) {
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }

}
