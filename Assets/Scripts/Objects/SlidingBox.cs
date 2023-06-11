using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class SlidingBox : MonoBehaviour
{
    private EventInstance slidingBox;
    private Rigidbody rb;
    [SerializeField] float maxVelocity;

    void Start()
    {
        slidingBox = AudioManager.instance.CreateEventInstance(FModEvents.instance.pushWoodBox);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
         rb = transform.GetComponent<Rigidbody>();
        if (maxVelocity >= 0 && rb != null) {
            if (rb.velocity.magnitude > maxVelocity) {
                float yVelocity = rb.velocity.y;
                rb.velocity = Vector3.Normalize(rb.velocity) * maxVelocity;
                rb.velocity = new Vector3(rb.velocity.x, yVelocity, rb.velocity.z);
            }
        }

        //prevent barrel from falling
        if (transform.position.y < -20.0f)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        PLAYBACK_STATE slidingBoxPlayBackState;
        slidingBox.getPlaybackState(out slidingBoxPlayBackState);
        slidingBox.setVolume(rb.velocity.magnitude / 5);

        if (slidingBoxPlayBackState.Equals(PLAYBACK_STATE.STOPPED) && rb.velocity != Vector3.zero)
        {
            
            slidingBox.start();
        }
        else if(rb.velocity == Vector3.zero)
        {
            Debug.Log(rb.velocity);
            slidingBox.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
