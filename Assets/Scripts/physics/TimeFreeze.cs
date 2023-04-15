using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeFreeze : MonoBehaviour
{
    private Rigidbody rb, savedrb;
    private GameObject[] gameObjects;
    private GameObject player, copyfolder, copy;
    [HideInInspector] public bool freezed;
    private CharacterController playerCtrl;
    private Vector3 playerLastPos, playerCurrPos, playerVelocity;

    void Awake()
    {
        if (!player)
        {
            Debug.LogError("The player gameobject was not set");
        }
        if (!copyfolder)
        {
            Debug.LogError("The copyfolder gameobject was not set");
        }
    }
    void Start()
    {
        
        freezed = false;

        player = GameObject.FindGameObjectWithTag("Player");
        copyfolder = GameObject.FindGameObjectWithTag("CopyPool");

        gameObjects = GameObject.FindGameObjectsWithTag("ControllableObjects");

        
        playerCtrl = player.GetComponent<CharacterController>();

        foreach(GameObject gobj in gameObjects)
        {
            copy = new GameObject();
            copy.transform.parent = copyfolder.transform;
            copy.name = gobj.name;
        }

        playerLastPos = player.transform.position;
    }

    void Freeze(GameObject gobj, int index)
    {
        Debug.Log("attempting to freeze");

        if (!freezed) {
            freezed = true;
            // move the rigidbody component from object to copy
            rb = gobj.GetComponent<Rigidbody>();
            if (!rb || !copy) {Debug.LogWarning("null test failed");}
            copy.AddComponentCopied(rb);
            // save values only, stop its physics
            copy.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(rb);
            Debug.Log("freezed");
        }
    }

    void Unfreeze(GameObject gobj)
    {
        Debug.Log("attempting to unfreeze");
        if (freezed) {
            freezed = false;
            // move the rigidbody component from copy to object
            savedrb = copy.GetComponent<Rigidbody>();
            if (!savedrb || !copy) {Debug.LogWarning("null test failed");}
            gobj.AddComponentCopied(savedrb);
            // resume physics on the unfreezed object
            gobj.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(savedrb);
            Debug.Log("unfreezed");
        }
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
            if (freezed)
            {
                foreach (var gobj in gameObjects)
                {
                    Freeze(gobj);
                }
            }
            
        }
        else {
            foreach (var gobj in gameObjects)
            {
                Freeze(gobj);
            }
        }
    }
}
