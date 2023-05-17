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
    private Vector3 m_Forward;

    [SerializeField] private bool defaultIsRight = false;
    [SerializeField] private bool faceRight;
    private bool isMoving;
    
    private SpriteRenderer spriteRenderer;

    public enum ESlopeLevel
    {
        none,
        ground,     // flat ground, deg = 0
        slope,      // 0 < deg <= maxSlopeAngle
        wall        // deg > maxSlopeAngle
    }
    public bool GetIsMoving() { return isMoving; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!rb) Debug.LogError("failed to get player rb");
        if (!spriteRenderer) Debug.LogError("failed to get player spriteRenderer");
        faceRight = defaultIsRight;
        playerHeight = GetComponent<Transform>().localScale.y;
    }

    private void Start()
    {
        Camera m_Cam = Camera.main;
        Vector3 m_Cam_forward = m_Cam.transform.forward;
        Vector3 m_Cam_forward_abs = new Vector3(Mathf.Abs(m_Cam_forward.x), 
                                                Mathf.Abs(m_Cam_forward.y), 
                                                Mathf.Abs(m_Cam_forward.z));
        if(m_Cam_forward_abs.x >= m_Cam_forward_abs.z)
        {
            //forward is roughly x
            if(m_Cam_forward.x > 0)
            {
                m_Forward = Vector3.right;
            }
            else
            {
                m_Forward = Vector3.left;
            }
            

        }
        else if(m_Cam_forward_abs.z >= m_Cam_forward_abs.x)
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

        Debug.Log(m_Forward);
    }

    private void OnCollisionExit(Collision collision)
    {
        // clear velocity from other sources if not undergoing collision
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        if(m_Forward == Vector3.forward)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }else if(m_Forward == Vector3.back)
        {
            moveHorizontal = -Input.GetAxis("Horizontal");
            moveVertical = -Input.GetAxis("Vertical");
        }
        else if(m_Forward == Vector3.right)
        {
            moveVertical = -Input.GetAxis("Horizontal");
            moveHorizontal = Input.GetAxis("Vertical");
        }
        else if(m_Forward == Vector3.left)
        {
            moveVertical = Input.GetAxis("Horizontal");
            moveHorizontal = -Input.GetAxis("Vertical");
        }
        

        isMoving = !(moveHorizontal == 0.0f && moveVertical == 0.0f);

        faceRight = Input.GetAxis("Horizontal") >= 0;

        spriteRenderer.flipX = (faceRight ^ defaultIsRight);

        moveDirection = new Vector3(moveHorizontal, 0, moveVertical);


        // Normalize the movement vector to ensure it's unit length
        //moveDirection = moveDirection.normalized;

        if (!rb) Debug.LogError("no player rb");
        // Update the position of the player
        switch (OnSlope())
        {
            case ESlopeLevel.none:
                // fall 
                rb.position += moveDirection * speed * Time.fixedDeltaTime;
                break;
            case ESlopeLevel.ground:
                // move horizontally 
                rb.position += moveDirection * speed * Time.fixedDeltaTime;
                // snap to ground
                rb.position = new Vector3(rb.position.x, slopeHit.point.y + playerHeight * 0.5f + 0.3f, rb.position.z);
                //rb.position = new Vector3(rb.position.x, slopeHit.point.y, rb.position.z);
                break;
            case ESlopeLevel.slope:
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


    private ESlopeLevel OnSlope()
    {
        // raycast downwards from center of player 
        if (Physics.Raycast(rb.position, Vector3.down, out slopeHit, playerHeight * 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle != 0)
            {
                if (angle < maxSlopeAngle)
                {
                    return ESlopeLevel.slope;
                }
                else if (angle == 0)
                {
                    return ESlopeLevel.ground;
                }
                else
                {
                    return ESlopeLevel.wall;
                }
            }
        }

        // dist > 0.5 * playerheight, fall
        return ESlopeLevel.none;
    }


    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

}
