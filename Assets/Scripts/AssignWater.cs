using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AssignWater : MonoBehaviour
{
    [SerializeField] GenerateMetaBalls oldWater;
    [SerializeField] GameObject waterParent;
    // Start is called before the first frame update
    void Start()
    {
        
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
