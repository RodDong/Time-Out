using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        foreach(var cam in Camera.allCameras)
        {
            if (cam.isActiveAndEnabled)
            {
                Vector3 objToCam = cam.transform.position - transform.position;
                Vector3 normal = transform.forward;
                objToCam.y = normal.y;

                transform.forward = objToCam;

  /*              Vector3 tempRotation = cam.transform.eulerAngles;
                tempRotation.x = transform.eulerAngles.x;
                tempRotation.z = transform.eulerAngles.z;
                transform.eulerAngles = tempRotation;*/
                break;
            }
        }
        
        
    }
}
