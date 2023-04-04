using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            GetComponent<MeshRenderer>().material.color = Color.red;
            gameObject.tag = "hit";
        }
        
    }
}
