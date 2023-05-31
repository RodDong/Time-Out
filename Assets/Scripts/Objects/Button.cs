using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Triggerable
{
    private bool inRange = false;

    [SerializeField] Color color;
    [SerializeField] GameObject buttonBase;

    void Start() {
        buttonBase.GetComponent<Renderer>().material.color = color;
    }

    private void Update() {
        if (inRange && Input.GetKeyUp(KeyCode.E) && !triggered) {
            GameEvents.current.TriggerEnter(id);
            Vector3 buttonTopPos = gameObject.GetComponentsInChildren<Transform>()[2].position;
            gameObject.GetComponentsInChildren<Transform>()[2].position = new Vector3(buttonTopPos.x, buttonTopPos.y - 0.1f, buttonTopPos.z) ;
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
