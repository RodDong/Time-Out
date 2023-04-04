using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class score : MonoBehaviour
{
    int counter = 0;
    private void OnCollisionEnter(Collision other){
        if(other.gameObject.tag != "hit"){
            counter++;
            Debug.Log("bumped: "+counter+"times");
        }
        
        
    }
}
