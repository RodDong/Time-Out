using UnityEngine;
using FMOD.Studio;
using Unity.Burst.CompilerServices;

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

    private EventInstance playerWalkGrass, 
                          playerWalkWater, 
                          playerWalkStair,
                          playerWalkStone,
                          playerWalkSand;

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
        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
    }

    private void Start()
    {
        CalibrateCameraOrientation();
        m_animator = GetComponent<Animator>();
        playerWalkGrass = AudioManager.instance.CreateEventInstance(FModEvents.instance.playerWalkGrass);
        playerWalkWater = AudioManager.instance.CreateEventInstance(FModEvents.instance.playerWalkWater);
        playerWalkStair = AudioManager.instance.CreateEventInstance(FModEvents.instance.playerWalkStair);
        playerWalkStone = AudioManager.instance.CreateEventInstance(FModEvents.instance.playerWalkStone);
        playerWalkSand = AudioManager.instance.CreateEventInstance(FModEvents.instance.playerWalkSand);
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

        UpdateMoveDirection();

        UpdateSound();

        if (moveDirection.sqrMagnitude == 0)
        {
            m_animator.Play("Idle");
            return;
        }
        else
        {
            m_animator.Play("Walk");
        }

        UpdatePlayerTransform();

        UpdateSound();
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

    private void UpdateMoveDirection()
    {
        moveVertical = 0.0f;
        moveHorizontal = 0.0f;
        if (Input.GetKey(KeyCode.W))
        {
            moveVertical += 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal -= 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVertical -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal += 1.0f;
        }

        if (m_Forward == Vector3.forward)
        {
            // Do Nothing
        }
        else if (m_Forward == Vector3.back)
        {
            moveHorizontal = -moveHorizontal;
            moveVertical = -moveVertical;
        }
        else if (m_Forward == Vector3.right)
        {
            var temp = moveVertical;
            moveVertical = -moveHorizontal;
            moveHorizontal = temp;
        }
        else if (m_Forward == Vector3.left)
        {
            var temp = moveVertical;
            moveVertical = moveHorizontal;
            moveHorizontal = -temp;
        }

        isMoving = !(moveHorizontal == 0.0f && moveVertical == 0.0f);


        if (Input.GetKey(KeyCode.E))
        {
            isMoving = true;
        }

        moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
    }

    private void UpdatePlayerTransform()
    {
        var targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

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
                break;
            case ESlopeLevel.slope:
                rb.useGravity = false;
                // move along slope
                rb.MovePosition(rb.position + GetSlopeMoveDirection() * speed * Time.deltaTime);
                break;
            case ESlopeLevel.wall:
                rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
                break;
            default: break;
        }
    }

    private void UpdateSound()
    {
        if(!(moveHorizontal == 0.0f && moveVertical == 0.0f) 
            && OnSlope() != ESlopeLevel.none)
        {
            PLAYBACK_STATE walkGrassPlayBackState,
                           walkWaterPlayBackState,
                           walkStairPlayBackState,
                           walkStonePlayBackState,
                           walkSandPlayBackState;
            playerWalkGrass.getPlaybackState(out walkGrassPlayBackState);
            playerWalkWater.getPlaybackState(out walkWaterPlayBackState);
            playerWalkStair.getPlaybackState(out walkStairPlayBackState);
            playerWalkStone.getPlaybackState(out walkStonePlayBackState);
            playerWalkSand.getPlaybackState(out walkSandPlayBackState);
            switch (GetGroundTag())
            {
                case "Ground":
                    if (walkGrassPlayBackState.Equals(PLAYBACK_STATE.STOPPED))
                    {
                        playerWalkWater.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkStair.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkStone.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkSand.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkGrass.start();
                    }
                    break;
                case "River":
                    if (walkWaterPlayBackState.Equals(PLAYBACK_STATE.STOPPED))
                    {
                        playerWalkGrass.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkStair.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkStone.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkSand.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkWater.start();
                    }
                    break;
                case "Stair":
                    if (walkStairPlayBackState.Equals(PLAYBACK_STATE.STOPPED))
                    {
                        playerWalkGrass.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkWater.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkStone.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkSand.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkStair.start();
                    }
                    break;
                case "Stone":
                    if (walkStonePlayBackState.Equals(PLAYBACK_STATE.STOPPED))
                    {
                        playerWalkGrass.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkWater.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkStair.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkSand.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkStone.start();
                    }
                    break;
                case "Sand":
                    if (walkSandPlayBackState.Equals(PLAYBACK_STATE.STOPPED))
                    {
                        playerWalkGrass.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkWater.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkStair.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkStone.stop(STOP_MODE.ALLOWFADEOUT);
                        playerWalkSand.start();
                    }
                    break;
                default:
                    break;
            }

            
        }
        else
        {
            playerWalkStair.stop(STOP_MODE.ALLOWFADEOUT);
            playerWalkGrass.stop(STOP_MODE.ALLOWFADEOUT);
            playerWalkWater.stop(STOP_MODE.ALLOWFADEOUT);
            playerWalkStone.stop(STOP_MODE.ALLOWFADEOUT);
            playerWalkSand.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    private string GetGroundTag()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 5f);
        return hit.transform.tag;
    }

}
