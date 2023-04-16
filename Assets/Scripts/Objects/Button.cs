using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Triggerable
{
    // Start is called before the first frame update
    void Start()
    {
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyUp(KeyCode.E)) {
            triggered = true;
        }
    }
}
