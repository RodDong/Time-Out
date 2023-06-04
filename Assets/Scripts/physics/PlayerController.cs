using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;

    private Rigidbody rb;

    private float moveHorizontal, moveVertical;

    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public float playerHeight;
    private Vector3 moveDirection;
    private Vector3 m_Forward;

    private bool isMoving;
    private float _currentVelocity;
    [SerializeField] private float smoothTime = 0.05f;

    private Animator m_animator;

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
        if (!rb) Debug.LogError("failed to get player rb");
        playerHeight = GetComponent<BoxCollider>().size.y * transform.localScale.y;
    }

    private void Start()
    {
        CalibrateCameraOrientation();
        m_animator = GetComponent<Animator>();
    }

    public void CalibrateCameraOrientation() {
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

        //transform.Rotate(0, 0, 0);
    }

    private void OnCollisionExit(Collision collision)
    {
        // clear velocity from other sources if not undergoing collision
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void Update()
    {

        moveVertical = 0.0f;
        moveHorizontal = 0.0f;
        if (Input.GetKey(KeyCode.W)) {
            moveVertical += 1.0f;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveHorizontal -= 1.0f;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveVertical -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveHorizontal += 1.0f;
        } 

        if (m_Forward == Vector3.forward)
        {
            // Do Nothing
        }
        else if(m_Forward == Vector3.back)
        {
            moveHorizontal = -moveHorizontal;
            moveVertical = -moveVertical;
        }
        else if(m_Forward == Vector3.right)
        {
            var temp = moveVertical;
            moveVertical = -moveHorizontal;
            moveHorizontal = temp;
        }
        else if(m_Forward == Vector3.left)
        {
            var temp = moveVertical;
            moveVertical = moveHorizontal;
            moveHorizontal = -temp;
        }

        isMoving = !(moveHorizontal == 0.0f && moveVertical == 0.0f);
        
        
        if (Input.GetKey(KeyCode.E)) {
            isMoving = true;
        }

        moveDirection = new Vector3(moveHorizontal, 0, moveVertical);

        if (moveDirection.sqrMagnitude == 0)
        {
            m_animator.Play("Idle");
            return;
        }
        else
        {
            m_animator.Play("Walk");
        }
        var targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

        // Normalize the movement vector to ensure it's unit length
        //moveDirection = moveDirection.normalized;

        if (!rb) Debug.LogError("no player rb");
        // Update the position of the player
        rb.useGravity = true;
        switch (OnSlope())
        {
            case ESlopeLevel.none:
                // fall 
                rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
                break;
            case ESlopeLevel.ground:
                // move along slope
                rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
                // snap to slope, leave a bit of gap
                //rb.position = new Vector3(rb.position.x, slopeHit.point.y + playerHeight * 0.5f + 0.0f, rb.position.z);
                //rb.MovePosition(new Vector3(rb.position.x, slopeHit.point.y + playerHeight * 0.5f + 0.1f, rb.position.z));
                // move horizontally 
                //transform.position += moveDirection * speed * Time.fixedDeltaTime;
                //// snap to ground
                //transform.position = new Vector3(transform.position.x, slopeHit.point.y + playerHeight * 0.5f + 0.1f, transform.position.z);
                //float transformy = transform.position.y;
                //float rby = rb.position.y;
                //Debug.Log("y supposed pos:" + supposedy);
                //rb.position = new Vector3(rb.position.x, slopeHit.point.y, rb.position.z);
                break;
            case ESlopeLevel.slope:
                rb.useGravity = false;
                // move along slope
                rb.MovePosition(rb.position + GetSlopeMoveDirection() * speed * Time.deltaTime);
                // snap to slope, leave a bit of gap
                //rb.MovePosition(new Vector3(rb.position.x, slopeHit.point.y + playerHeight * 0.5f + 0.3f, rb.position.z));
                break;
            case ESlopeLevel.wall:
                rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
                break;
            default: break;
        }
        //rb.velocity = moveDirection * speed;
        //rb.AddForce(moveDirection * speed, ForceMode.VelocityChange);
        //newPos = rb.position + moveDirection * speed * Time.fixedDeltaTime;
        //rb.MovePosition(newPos);
    }


    private ESlopeLevel OnSlope()
    {
        // raycast downwards from center of player 
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle == 0)
            {
                return ESlopeLevel.ground;
            }
            else if (angle <= maxSlopeAngle)
            {
                return ESlopeLevel.slope;
            }
            else
            {
                return ESlopeLevel.wall;
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
