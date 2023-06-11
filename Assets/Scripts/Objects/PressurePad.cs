using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : Triggerable
{
    private static Dictionary<int, int> count = new Dictionary<int, int>();
    [SerializeField] Color color;

    [SerializeField] GameObject padBase;

    void Start() {
        padBase.GetComponent<Renderer>().material.color = color;
        if (!count.ContainsKey(id))
            count.Add(id, 0);
    }


    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Ground" || other.tag == "CameraCollider") {
             return; 
        }
        print(other.name);

        count[id]++;

        if (count[id] == 1) {
            
            Vector3 v = transform.position;
            Vector3 s = transform.localScale;
            transform.position = new Vector3(v.x, v.y - s.y / 2, v.z);

            GameEvents.current.TriggerEnter(id);
        }

    }

    private void OnTriggerExit(Collider other) {

        count[id]--;

        if (count[id] == 0) {
            
            triggered = false;
            
            Vector3 v = transform.position;
            Vector3 s = transform.localScale;
            transform.position = new Vector3(v.x, v.y + s.y / 2, v.z);

            GameEvents.current.TriggerExit(id);
        }
    }
}
