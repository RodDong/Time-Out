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

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
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
