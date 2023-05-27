using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempRotation = Camera.main.transform.eulerAngles;
        tempRotation.x = transform.eulerAngles.x;
        tempRotation.z = transform.eulerAngles.z;
        transform.eulerAngles = tempRotation;
        
    }
}
