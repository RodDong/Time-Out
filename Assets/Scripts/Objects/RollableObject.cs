using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class RollableObject : MonoBehaviour
{
    [SerializeField] Vector3 force;
    [SerializeField] float maxVelocity;
    Rigidbody rb;
    private EventInstance barrelRoll;
    // Start is called before the first frame update
    void Start()
    {
        barrelRoll = AudioManager.instance.CreateEventInstance(FModEvents.instance.barrelRoll);
    }

    // Update is called once per frame
    void Update()
    {
        rb = transform.GetComponent<Rigidbody>();
        if (maxVelocity >= 0 && rb != null) {
            if (rb.velocity.magnitude > maxVelocity) {
                rb.velocity = Vector3.Normalize(rb.velocity) * maxVelocity;
            }
        }

        //prevent barrel from falling
        if (transform.position.y < -20.0f)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        UpdateBarrelSound();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            transform.GetComponent<Rigidbody>().velocity = force;
            print(">......................");
        }
    }

    private void UpdateBarrelSound()
    {
        if (rb.velocity != Vector3.zero)
        {
            PLAYBACK_STATE barrelRollPlayBackState;
            barrelRoll.getPlaybackState(out barrelRollPlayBackState);
            barrelRoll.setVolume(rb.velocity.magnitude);
            if (barrelRollPlayBackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                barrelRoll.start();
            }
        }
        else
        {
            barrelRoll.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
