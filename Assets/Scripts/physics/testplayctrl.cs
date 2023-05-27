using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testplayctrl : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private Vector3 m_Forward;
    private float moveHorizontal, moveVertical;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();


        Camera m_Cam = Camera.main;
        Vector3 m_Cam_forward = m_Cam.transform.forward;
        Vector3 m_Cam_forward_abs = new Vector3(Mathf.Abs(m_Cam_forward.x),
                                                Mathf.Abs(m_Cam_forward.y),
                                                Mathf.Abs(m_Cam_forward.z));
        if (m_Cam_forward_abs.x >= m_Cam_forward_abs.z)
        {
            //forward is roughly x
            if (m_Cam_forward.x > 0)
            {
                m_Forward = Vector3.right;
            }
            else
            {
                m_Forward = Vector3.left;
            }


        }
        else if (m_Cam_forward_abs.z >= m_Cam_forward_abs.x)
        {
            //forward is roughly z
            if (m_Cam_forward.z > 0)
            {
                m_Forward = Vector3.forward;
            }
            else
            {
                m_Forward = Vector3.back;
            }
        }
    }

    void Update()
    {

        if (m_Forward == Vector3.forward)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }
        else if (m_Forward == Vector3.back)
        {
            moveHorizontal = -Input.GetAxis("Horizontal");
            moveVertical = -Input.GetAxis("Vertical");
        }
        else if (m_Forward == Vector3.right)
        {
            moveVertical = -Input.GetAxis("Horizontal");
            moveHorizontal = Input.GetAxis("Vertical");
        }
        else if (m_Forward == Vector3.left)
        {
            moveVertical = Input.GetAxis("Horizontal");
            moveHorizontal = -Input.GetAxis("Vertical");
        }

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.transform.tag == "Pushable Object") {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            if (rb != null) {
                Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, 0);
                float pushPower = 1.0f;
                rb.velocity = pushDir * pushPower;
            }
        }
    }
}
