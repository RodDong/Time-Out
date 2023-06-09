using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Gate : MonoBehaviour
{
    // [SerializeField] Triggerable controller;
    
    public int id;
    private bool opened;
    [SerializeField] bool inverted;
    private float speed = 2;
    private float closedYPos;
    private float openedYPos;
    private EventInstance gateOpen;

    // Start is called before the first frame update
    void Start()
    {
        closedYPos = transform.position.y;
        openedYPos = closedYPos - transform.GetComponent<Renderer>().bounds.size.y;
        opened = false;

        GameEvents.current.onTriggerEnter += Open;
        GameEvents.current.onTriggerExit += Close;
        gateOpen = AudioManager.instance.CreateEventInstance(FModEvents.instance.gateOpen);
    }

    // Update is called once per frame
    void Update()
    {
        // bool triggered = inverted ? !controller.triggered : controller.triggered;
        bool triggered = inverted ^ opened;

        Vector3 curPos = transform.position;
        if (triggered && curPos.y > openedYPos) {
            transform.position = new Vector3(curPos.x, curPos.y - speed * Time.deltaTime, curPos.z);
            UpdateSound();
        } else if (!triggered && curPos.y < closedYPos) {
            transform.position = new Vector3(curPos.x, curPos.y + speed * Time.deltaTime, curPos.z);
            UpdateSound();
        }
    }

    private void Open(int id) {
        if (id == this.id) {
            opened = true;
        }
    }

    private void Close(int id) {
        if (id == this.id) {
            opened = false;
        }
    }

    private void UpdateSound()
    {
        PLAYBACK_STATE gatePlayBackState;
        gateOpen.getPlaybackState(out gatePlayBackState);

        if (gatePlayBackState.Equals(PLAYBACK_STATE.STOPPED)){
            gateOpen.start();
        }
        
    }
}
