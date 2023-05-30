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
        if (inRange && Input.GetKeyUp(KeyCode.E)) {
            GameEvents.current.TriggerEnter(id);
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
