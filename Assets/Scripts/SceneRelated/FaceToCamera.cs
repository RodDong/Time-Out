using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    Transform[] children;

    // Start is called before the first frame update
    void Start()
    {
        children = transform.GetComponentsInChildren<Transform>();   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempRotation = Camera.main.transform.eulerAngles;
        foreach (var child in children.Skip(1))
        {
            tempRotation.x = child.eulerAngles.x;
            tempRotation.z = child.eulerAngles.z;
            child.eulerAngles = tempRotation;
        }
    }
}
