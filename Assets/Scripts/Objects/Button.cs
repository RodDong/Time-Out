using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
public class Button : Triggerable
{
    private bool inRange = false;

    [SerializeField] Color color;
    [SerializeField] GameObject buttonBase;
    
    private EventInstance button;

    void Start() {
        buttonBase.GetComponent<Renderer>().material.color = color;
        button = AudioManager.instance.CreateEventInstance(FModEvents.instance.button);
    }

    private void Update() {
        PLAYBACK_STATE buttonPlayBackState;
        button.getPlaybackState(out buttonPlayBackState);

        if (inRange && Input.GetKeyUp(KeyCode.E) && !triggered) {
            GameEvents.current.TriggerEnter(id);
            Vector3 buttonTopPos = gameObject.GetComponentsInChildren<Transform>()[2].position;
            gameObject.GetComponentsInChildren<Transform>()[2].position = new Vector3(buttonTopPos.x, buttonTopPos.y - 0.1f, buttonTopPos.z);
            triggered = true;

            if (buttonPlayBackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                button.start();
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
