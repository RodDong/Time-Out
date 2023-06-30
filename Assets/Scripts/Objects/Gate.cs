using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using UnityEngine.Android;

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

    [SerializeField] Color color;
    [SerializeField] List<GameObject> colorStrips;

    public bool GetOpened()
    {
        return opened;
    }

    // Start is called before the first frame update
    void Start()
    {
        closedYPos = transform.localPosition.y;
        openedYPos = closedYPos - transform.GetComponent<Renderer>().bounds.size.y;
        opened = false;

        GameEvents.current.onTriggerEnter += Open;
        GameEvents.current.onTriggerExit += Close;
        gateOpen = AudioManager.instance.CreateEventInstance(FModEvents.instance.gateOpen);

        foreach (var item in colorStrips) {
            item.GetComponent<Renderer>().material.color = color;
        }
       
    }

    private void OnLevelWasLoaded(int level)
    {
        opened = false;
    }

    // Update is called once per frame
    void Update()
    {
        // bool triggered = inverted ? !controller.triggered : controller.triggered;
        bool triggered = inverted ^ opened;

        Vector3 curPos = transform.localPosition;
        if (triggered && curPos.y > openedYPos) {
            transform.localPosition = new Vector3(curPos.x, curPos.y - speed * Time.deltaTime, curPos.z);
            UpdateSound();
        } else if (!triggered && curPos.y < closedYPos) {
            transform.localPosition = new Vector3(curPos.x, curPos.y + speed * Time.deltaTime, curPos.z);
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
