using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WaterMeshGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject waterParent;
    private Transform[] waterParticles;
    private KdTree<MeshFilter> waterParticleMesheFilters = new KdTree<MeshFilter>();
    private KdTree<Transform> waterObjects = new KdTree<Transform>();
    private List<Transform> notVisitedParticles = new List<Transform>();
    
    void Start()
    {
        waterParticles = waterParent.GetComponentsInChildren<Transform>();

        for(int i = 1; i < waterParticles.Length; i++)
        {
            MeshFilter curMeshFilter = waterParticles[i].gameObject.GetComponent<MeshFilter>();
            if (curMeshFilter != null)
            {
                waterParticleMesheFilters.Add(curMeshFilter);
                waterObjects.Add(waterParticles[i].transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        waterObjects.UpdatePositions();
        notVisitedParticles = waterObjects.ToList();
        for (int i = 0; i < notVisitedParticles.Count; i++)
        {
            GameObject closestParticle = waterObjects.FindClosest(notVisitedParticles[i].position).gameObject;
            notVisitedParticles.RemoveAll(particle => particle.name == notVisitedParticles[i].name);
            Debug.Log(closestParticle.name);
            Debug.DrawLine(waterObjects[i].transform.position, closestParticle.transform.position, Color.red);
        }
    }
}
