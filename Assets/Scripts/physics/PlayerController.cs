using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;

    private Rigidbody rb;

    private float moveHorizontal, moveVertical;

    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public float playerHeight;
    private Vector3 moveDirection, newPos;

    public enum SlopeLevel
    {
        ground,     // flat ground, deg = 0
        slope,      // 0 < deg <= maxSlopeAngle
        wall        // deg > maxSlopeAngle
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) Debug.LogError("failed to get player rb");
    }

    void Start()
    {
        //playerHeight = GetComponent<CapsuleCollider>().height;
        //Debug.Log("player height get:" + playerHeight);
    }

    private void OnCollisionExit(Collision collision)
    {
        // clear velocity from other sources if not undergoing collision
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveHorizontal, 0, moveVertical);


        // Normalize the movement vector to ensure it's unit length
        //moveDirection = moveDirection.normalized;

        if (!rb) Debug.LogError("no player rb");
        // Update the position of the player
        switch (OnSlope())
        {
            case SlopeLevel.ground:
                // move horizontally 
                rb.position += moveDirection * speed * Time.fixedDeltaTime;
                // snap to ground
                //rb.position = new Vector3(rb.position.x, slopeHit.point.y + playerHeight * 0.5f + 0.05f, rb.position.z);
                rb.position = new Vector3(rb.position.x, slopeHit.point.y + playerHeight * 0.5f, rb.position.z);
                break;
            case SlopeLevel.slope:
                // move along slope
                rb.position += GetSlopeMoveDirection() * speed * Time.fixedDeltaTime;
                // snap to slope, leave a bit of gap
                rb.position = new Vector3(rb.position.x, slopeHit.point.y + playerHeight * 0.5f + 0.3f, rb.position.z);
                break;
            default: return;
        }
        //rb.velocity = moveDirection * speed;
        //rb.AddForce(moveDirection * speed, ForceMode.VelocityChange);
        //newPos = rb.position + moveDirection * speed * Time.fixedDeltaTime;
        //rb.MovePosition(newPos);
    }


    private SlopeLevel OnSlope()
    {
        // raycast downwards from center of player 
        if (Physics.Raycast(rb.position, Vector3.down, out slopeHit, playerHeight * 5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle != 0)
            {
                if (angle < maxSlopeAngle)
                {
                    return SlopeLevel.slope;
                }
                else
                {
                    return SlopeLevel.wall;
                }
            }
        }

        // angle = 0 or dist > 5 * playerheight
        return SlopeLevel.ground;
    }


    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

}
