using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actionable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyUp(KeyCode.E)) {
            
        }
    }

    abstract public void ExecuteAction();
}
