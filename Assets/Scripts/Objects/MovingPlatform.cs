using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // [SerializeField] Triggerable controller;
    [SerializeField] Vector3 startingPos;
    [SerializeField] Vector3 endPos;
    private float step;
    [SerializeField] float duration;
    [SerializeField] bool repeat;
    [SerializeField] bool inverted;
    private bool reversing;
    private bool triggered;
    public int id;

    private TimeFreeze tf;

    [SerializeField] private float addhesiveness = 1.0f;

    [SerializeField] Color color;
    [SerializeField] GameObject colorStrip;

    // Start is called before the first frame update
    void Start()
    {
        tf = FindObjectOfType<TimeFreeze>();
        step = 0;
        reversing = false;

        GameEvents.current.onTriggerEnter += Start;
        GameEvents.current.onTriggerExit += Stop;
        if(colorStrip != null)
        {
            colorStrip.GetComponent<Renderer>().material.color = color;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tf.freezed)
            return;

        bool triggered = inverted ^ this.triggered;
        if (id != 0 && !triggered) {
            return; 
        }

        if (step > duration && !repeat) {
            return;
        }

        if (step > duration) {
            step = duration;
            reversing = true;
        } else if (step < 0) {
            step = 0;
            reversing = false;
        }

        step += (reversing ? -1 : 1) * Time.deltaTime;

        transform.localPosition = Vector3.Lerp(startingPos, endPos, step / duration);
    }

    private void Start(int id) {
        if (id == this.id) {
            triggered = true;
        }
    }

    private void Stop(int id) {
        if (id == this.id) {
            triggered = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.tag == "Barrel" && !tf.freezed && triggered)
        {
            if (step > duration && !repeat)
            {
                return;
            }
            Vector3 movingDirection = (endPos - startingPos).normalized;
            float dist = (endPos - startingPos).magnitude;
            collision.transform.position += movingDirection
                              * dist/duration * Time.deltaTime
                              * addhesiveness * (reversing ? -1 : 1);
        }
    }
}
