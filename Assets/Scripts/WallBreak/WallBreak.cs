using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WallBreak : MonoBehaviour
{
    [SerializeField] GameObject fractured;
    [SerializeField] VisualEffect explosion;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            GameObject frac = Instantiate(fractured, transform.position, transform.rotation);
            frac.transform.localScale = transform.localScale;
            foreach(Rigidbody rb in frac.GetComponentsInChildren<Rigidbody>())
            {
                Vector3 force = (rb.transform.right).normalized * 200;
                rb.AddForce(force);
            }

            explosion.Play();

            Destroy(gameObject);
        }
    }
}
