using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] public float velocityRequiredForDestruction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        print(collision.gameObject.transform.GetComponent<Rigidbody>().velocity.magnitude);
        if (collision.gameObject.transform.GetComponent<Rigidbody>().velocity.magnitude >= velocityRequiredForDestruction) {
            print("DESTROY");
            GetComponent<Collider>().isTrigger = true;

            Material m_Material = GetComponent<Renderer>().material;
            m_Material.color = Color.red;
        }
    }
}
