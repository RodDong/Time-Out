using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class SlidingBox : MonoBehaviour
{
    private EventInstance slidingBox;
    private Rigidbody rb;

    void Start()
    {
        slidingBox = AudioManager.instance.CreateEventInstance(FModEvents.instance.pushWoodBox);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
