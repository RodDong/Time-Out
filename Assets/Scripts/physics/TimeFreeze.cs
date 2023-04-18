using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeFreeze : MonoBehaviour
{
    private GameObject[] gameObjects;
    private List<GameObject> copiedObjects;
    private List<Vector3> savedVelocities;
    private List<Vector3> savedAngularVelocities;
    private GameObject player, copyfolder;
    [HideInInspector] public bool freezed;
    private CharacterController playerCtrl;
    private Vector3 playerLastPos, playerCurrPos, playerVelocity;


    void Start()
    {
        copiedObjects = new List<GameObject>();
        savedVelocities = new List<Vector3>();
        savedAngularVelocities = new List<Vector3>();
        freezed = false;

        player = GameObject.FindGameObjectWithTag("Player");
        copyfolder = GameObject.FindGameObjectWithTag("CopyPool");

        gameObjects = GameObject.FindGameObjectsWithTag("ControllableObjects");

        
        playerCtrl = player.GetComponent<CharacterController>();

        foreach(GameObject gobj in gameObjects)
        {
            var copy = new GameObject();
            copy.transform.parent = copyfolder.transform;
            copy.name = gobj.name;
            copiedObjects.Add(copy);
            savedVelocities.Add(new Vector3());
            savedAngularVelocities.Add(new Vector3());
        }

        playerLastPos = player.transform.position;
    }

    void Freeze(GameObject gobj, int index)
    {
        // Debug.Log("attempting to freeze");
    
        // move the rigidbody component from object to copy
        var copy = copiedObjects[index];
        Rigidbody rb = gobj.GetComponent<Rigidbody>();
        if (!rb || !copy) {Debug.LogWarning("null test failed");}

        savedVelocities[index] = rb.velocity;
        savedAngularVelocities[index] = rb.angularVelocity;
        rb.isKinematic = true;
        copy.AddComponentCopied(rb);
        // save values only, stop its physics

        Destroy(rb);
        // Debug.Log("freezed");
    }

    void Unfreeze(GameObject gobj, int index)
    {
        // Debug.Log("attempting to unfreeze");

        // move the rigidbody component from copy to object
        var copy = copiedObjects[index];
        Rigidbody savedrb = copy.GetComponent<Rigidbody>();

        if (!savedrb || !copy) {Debug.LogWarning("null test failed");}

        savedrb.isKinematic = false;
        savedrb.velocity = savedVelocities[index];
        savedrb.angularVelocity = savedAngularVelocities[index];

        gobj.AddComponentCopied(savedrb);
        // resume physics on the unfreezed object
        Destroy(savedrb);
        // Debug.Log("unfreezed");
    }

    bool NearZeroVector(Vector3 v)
    {
        return -0.001f < v.x && v.x < 0.001f 
        && -0.001f < v.y && v.y < 0.001f 
        && -0.001f < v.z && v.z < 0.001f;
    }

    bool NearZeroMag(float n) { return -0.001f < n && n < 0.001f; }

    void Update()
    {
        playerCurrPos = player.transform.position;
        playerVelocity = (playerCurrPos - playerLastPos) / Time.deltaTime;
        playerLastPos = playerCurrPos;

        if (NearZeroVector(playerVelocity))
        {
            if (!freezed)
            {
                freezed = true;
                int i = 0;
                foreach (var gobj in gameObjects)
                {
                    Freeze(gobj, i);
                    i++;
                }
            }
            
        }
        else 
        {
            if (freezed)
            {
                freezed = false;
                int i = 0;
                foreach (var gobj in gameObjects)
                {
                    Unfreeze(gobj, i);
                    i++;
                }
            }
        }
    }
}
