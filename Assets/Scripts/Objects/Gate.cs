using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] Triggerable controller;
    private float speed = 1;
    private float closedYPos;
    private float openedYPos;

    // Start is called before the first frame update
    void Start()
    {
        closedYPos = transform.position.y;
        openedYPos = closedYPos - transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        if (controller.triggered && curPos.y > openedYPos) {
            transform.position = new Vector3(curPos.x, curPos.y - speed * Time.deltaTime, curPos.z);
        } else if (!controller.triggered && curPos.y < closedYPos) {
            transform.position = new Vector3(curPos.x, curPos.y + speed * Time.deltaTime, curPos.z);
        }
    }
}
