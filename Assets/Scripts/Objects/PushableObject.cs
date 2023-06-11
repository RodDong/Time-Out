using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class PushableObject : MonoBehaviour
{
    private bool inContact;
    private EventInstance pushWoodBox;
    private GameObject player;

    void Start() {
        inContact = false;
        pushWoodBox = AudioManager.instance.CreateEventInstance(FModEvents.instance.pushWoodBox);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Update()
    {
        if (inContact) {
            /*Vector3 newPos = transform.localPosition + force * Time.deltaTime;
            transform.localPosition = newPos;*/

            PLAYBACK_STATE pushWoodBoxPlayBackState;
            pushWoodBox.getPlaybackState(out pushWoodBoxPlayBackState);
            pushWoodBox.setVolume(Mathf.Clamp(GetComponent<Rigidbody>().velocity.magnitude * 1000, 0, 3)/5);
            if (pushWoodBoxPlayBackState.Equals(PLAYBACK_STATE.STOPPED)){
                pushWoodBox.start();
            }
        }
        else
        {
            pushWoodBox.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    [SerializeField] float force;

    private void OnCollisionEnter(Collision collision) {
        
        if (collision.gameObject.tag == "Player") {
            inContact = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 forceDirect = transform.position - player.transform.position;
            forceDirect.y = 0;
            forceDirect.Normalize();
            GetComponent<Rigidbody>().AddForceAtPosition(forceDirect * force * Time.deltaTime, player.transform.position, ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision collision) {
        
        if (collision.gameObject.tag == "Player") {
            inContact = false;
        }
    }

}
