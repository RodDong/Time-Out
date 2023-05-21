using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WaterMeshGenerator : MonoBehaviour
{
    [SerializeField] GameObject waterParent;
    private Transform[] waterParticles;
    private KdTree<Transform> waterObjects = new KdTree<Transform>();
    private List<Transform> notVisitedParticles = new List<Transform>();

    void Start()
    {
        waterParticles = waterParent.GetComponentsInChildren<Transform>();

        for (int i = 1; i < waterParticles.Length; i++)
        {
            MeshFilter curMeshFilter = waterParticles[i].gameObject.GetComponent<MeshFilter>();
            if (curMeshFilter != null)
            {
                waterObjects.Add(waterParticles[i].transform);
            }
        }
    }
    void Update()
    {
        waterObjects.UpdatePositions();
        notVisitedParticles = waterObjects.ToList();
        for (int i = 0; i < waterObjects.Count; i++)
        {
            GameObject closestParticle = waterObjects.FindClosest(waterObjects[i].position).gameObject;
            notVisitedParticles.RemoveAll(particle => particle.name == waterObjects[i].name);
            //Debug.DrawLine(waterObjects[i].GetComponent<MeshFilter>().sharedMesh.vertices[0], closestParticle.GetComponent<MeshFilter>().sharedMesh.vertices[0], Color.red);

            Debug.DrawLine(waterObjects[i].transform.position, closestParticle.transform.position, Color.red);
        }

    }
}