using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Lever : Triggerable
{
    public Color color;
    private float rotationAngle = 30;
    private float speed = 60;
    private float interactableTimer = 0;
    [SerializeField] GameObject LeverStick;
    [SerializeField] GameObject LeverBase;
    private bool inRange = false;
    public bool interacted = false;

    private EventInstance lever;

    void Start() {
        LeverStick.GetComponentsInChildren<Renderer>()[1].material.color = color;
        LeverBase.GetComponent<Renderer>().material.color = color;
        lever = AudioManager.instance.CreateEventInstance(FModEvents.instance.lever);
    }

    void Update()
    {
        if (interactableTimer > 0) {
            interactableTimer -= Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.E) && inRange)
        {
            interacted = true;
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

                PLAYBACK_STATE buttonPlayBackState;
                lever.getPlaybackState(out buttonPlayBackState);

                if (triggered)
                {
                    LeverStick.transform.eulerAngles = new Vector3(curRotation.x, curRotation.y, -90);
                    lever.start();
                }
                else
                {
                    LeverStick.transform.eulerAngles = new Vector3(curRotation.x, curRotation.y, 0);
                    lever.start();
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
