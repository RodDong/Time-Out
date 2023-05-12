using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Triggerable : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public bool triggered;
    public int id;

    void Start()
    {
        triggered = false;
    }

    
}
