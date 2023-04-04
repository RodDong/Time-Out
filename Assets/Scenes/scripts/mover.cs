using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mover : MonoBehaviour
{
    [SerializeField] float yValue = 0.0f;
    float sensitivity = 5.0f ;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float xValue = Input.GetAxis("Horizontal") * sensitivity * Time.deltaTime;
        float zValue = Input.GetAxis("Vertical") * sensitivity * Time.deltaTime;
        transform.Translate(xValue,yValue,zValue);
    }
}
