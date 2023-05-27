using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onTriggerEnter += Release;
    }

    private void Release(int id) {
        if (id == this.id) {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

}
