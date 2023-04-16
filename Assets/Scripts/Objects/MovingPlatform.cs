using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Triggerable controller;
    [SerializeField] Vector3 startingPos;
    [SerializeField] Vector3 endPos;
    private float step;
    [SerializeField] float duration;
    private bool reversing;

    // Start is called before the first frame update
    void Start()
    {
        step = 0;
        reversing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller != null && !controller.triggered) { return; }

        if (step > duration) {
            reversing = true;
        } else if (step < 0) {
            reversing = false;
        }

        step += (reversing ? -1 : 1) * Time.deltaTime;

        transform.localPosition = Vector3.Lerp(startingPos, endPos, step / duration);
    }
}
