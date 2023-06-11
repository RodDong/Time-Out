using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using FMOD.Studio;

public class WallBreak : MonoBehaviour
{
    [SerializeField] GameObject fractured;
    [SerializeField] VisualEffect explosion;
    [SerializeField] GameObject Wall;
    [SerializeField] public float velocityRequiredForDestruction;
    private EventInstance wallBreak;

    private void Start()
    {
        wallBreak = AudioManager.instance.CreateEventInstance(FModEvents.instance.wallBreak);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.transform.GetComponent<Rigidbody>().velocity.magnitude);
        if (collision.gameObject.transform.GetComponent<Rigidbody>().velocity.magnitude >= velocityRequiredForDestruction)
        {
            GetComponent<Collider>().isTrigger = true;
            Destroy(Wall);
            GameObject frac = Instantiate(fractured, transform.position, transform.rotation);
            frac.transform.localScale = transform.localScale;
            foreach (Rigidbody rb in frac.GetComponentsInChildren<Rigidbody>())
            {
                Vector3 force = (rb.transform.right).normalized * 500;
                rb.AddForce(force);
            }

            explosion.Play();
            wallBreak.start();
            Destroy(gameObject);
        }
    }
}
