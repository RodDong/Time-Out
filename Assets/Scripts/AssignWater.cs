using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class AssignWater : MonoBehaviour
{
    [SerializeField] GenerateMetaBalls oldWater;
    [SerializeField] GameObject waterParent;
    private MCBlob waterParentMCBlob;
    // Start is called before the first frame update
    void Start()
    {
        waterParentMCBlob = waterParent.GetComponent<MCBlob>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        
        if (other.tag == "WaterBall") {
            
            if (!waterParent.activeSelf) {
                waterParent.SetActive(true);
            }

            other.transform.parent = waterParent.transform;



            int x = waterParent.transform.childCount;
            waterParentMCBlob.isoLevel = 0.0094f * Mathf.Pow(x, 3) - 0.1043f * Mathf.Pow(x, 2) + 0.4807f * x + 2.0178f;
            Debug.Log("numChild" + x + "isoLevel" + waterParentMCBlob.isoLevel);
            
            int index = 0;
            for (int i = 0; i < oldWater.BlobObjectsLocations.Length; i++) {
                if (oldWater.BlobObjectsLocations[i] == (SphereCollider)other) {
                    index = i;
                    print(index);
                    break;
                }
            }

            oldWater.BlobObjectsLocations = oldWater.BlobObjectsLocations.Skip(index).ToArray();
        }
    }
}
