using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class WaterController : MonoBehaviour
{
    private ExampleContortAlong m_ContortAlong;
    [SerializeField] private float maxScale = 1.0f;
    [SerializeField] private float minScale = 0.0f;
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_ContortAlong = GetComponent<ExampleContortAlong>();
        m_ContortAlong.scale = new Vector3(minScale, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_ContortAlong.scale.x < maxScale)
        {
            timer += Time.deltaTime;
        }
        m_ContortAlong.ScaleMesh(new Vector3(Mathf.Lerp(minScale, maxScale, timer), 1, 1));
    }
}
