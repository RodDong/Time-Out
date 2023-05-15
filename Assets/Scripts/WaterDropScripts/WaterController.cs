using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;
using UnityEngine.Rendering.PostProcessing;

public class WaterController : MonoBehaviour
{
    private ExampleContortAlong m_ContortAlong;
    [SerializeField] private float maxScale = 1.0f;
    [SerializeField] private float minScale = 0.0f;
    [SerializeField] private float startTime = 0.0f;
    [SerializeField] private float speed = 0.5f;
    private float timer = 0.0f;
    private float rate = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_ContortAlong = GetComponent<ExampleContortAlong>();
        m_ContortAlong.scale = new Vector3(minScale, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(m_ContortAlong.scale.x < maxScale && timer > startTime)
        {
            rate += Time.deltaTime;
        }

        if(rate > 1)
        {
            rate = 1;
        }

        m_ContortAlong.ScaleMesh(new Vector3(Mathf.Lerp(minScale, maxScale, rate), 1, 1));
        m_ContortAlong.Contort(rate * speed);
    }
}
