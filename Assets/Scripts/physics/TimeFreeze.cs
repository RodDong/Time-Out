using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFreeze : MonoBehaviour
{
    public Rigidbody rb, savedrb;
    public GameObject player, copyfolder, copy;
    public bool freezed;
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
        rb = gameObject.GetComponent<Rigidbody>();
        playerCtrl = player.GetComponent<CharacterController>();
        copy = new GameObject();
        copy.transform.parent = copyfolder.transform;
        copy.name = gameObject.name;
        playerLastPos = player.transform.position;
    }

    // Let the rigidbody take control.
    void EnableRagdoll()
    {
        rb.isKinematic = false;
        // rb.detectCollisions = true;
    }

    // Let animation control the rigidbody.
    void DisableRagdoll()
    {
        rb.isKinematic = true;
        // rb.detectCollisions = false;
    }

    void Freeze()
    {
        Debug.Log("attempting to freeze");
        if (!freezed) {
            freezed = true;
            // move the rigidbody component from object to copy
            rb = gameObject.GetComponent<Rigidbody>();
            if (!rb || !copy) {Debug.LogWarning("null test failed");}
            copy.AddComponentCopied(rb);
            // save values only, stop its physics
            copy.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(rb);
            Debug.Log("freezed");
        }
    }

    void Unfreeze()
    {
        Debug.Log("attempting to unfreeze");
        if (freezed) {
            freezed = false;
            // move the rigidbody component from copy to object
            savedrb = copy.GetComponent<Rigidbody>();
            if (!savedrb || !copy) {Debug.LogWarning("null test failed");}
            gameObject.AddComponentCopied(savedrb);
            // resume physics on the unfreezed object
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
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
        // Debug.Log("player speed:" + playerCtrl.velocity.magnitude);

        if (NearZeroVector(playerVelocity))
        // if (player.transform.position.x < 1 || player.transform.position.x > 1.5)
        // if (NearZeroMag(playerCtrl.velocity.magnitude))
        {
            
            if (!freezed) { Freeze(); }
        }
        else {
            if (freezed) { Unfreeze(); }
        }
    }
}
