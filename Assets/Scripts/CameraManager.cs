using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] public GameObject associateCamera;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        Debug.Assert(player != null);
    }

    private void OnTriggerEnter(Collider other) {
        Camera.main.transform.position = associateCamera.transform.position;
        Camera.main.transform.eulerAngles = associateCamera.transform.eulerAngles;
        player.CalibrateCameraOrientation();
    }
}
