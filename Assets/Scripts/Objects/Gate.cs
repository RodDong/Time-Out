using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    // [SerializeField] Triggerable controller;
    
    public int id;
    private bool opened;
    [SerializeField] bool inverted;
    private float speed = 2;
    private float closedYPos;
    private float openedYPos;

    // Start is called before the first frame update
    void Start()
    {
        closedYPos = transform.position.y;
        openedYPos = closedYPos - transform.GetComponent<Renderer>().bounds.size.y;
        opened = false;

        GameEvents.current.onTriggerEnter += Open;
        GameEvents.current.onTriggerExit += Close;
    }

    // Update is called once per frame
    void Update()
    {
        // bool triggered = inverted ? !controller.triggered : controller.triggered;
        bool triggered = inverted ^ opened;

        Vector3 curPos = transform.position;
        if (triggered && curPos.y > openedYPos) {
            transform.position = new Vector3(curPos.x, curPos.y - speed * Time.deltaTime, curPos.z);
        } else if (!triggered && curPos.y < closedYPos) {
            transform.position = new Vector3(curPos.x, curPos.y + speed * Time.deltaTime, curPos.z);
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
}
