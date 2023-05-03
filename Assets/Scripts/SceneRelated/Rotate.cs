using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1.0f;
    private void Update()
    {
        Vector3 temp = gameObject.transform.eulerAngles;
        temp.y += Time.deltaTime * rotationSpeed;
        gameObject.transform.eulerAngles = temp;
    }
}
