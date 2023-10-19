using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float speed;
    public float walkSpeed;
    public float runSpeed;
    public bool dashInTheAir;
    public float dashes;
    public float dashSpeed;
    public float DashForce;
    public float DashUpwardForce;
    public float DashDuration;
    public float dashCd;
    public float dashCdGlobal;
    public float dashCdTimerGlobal;
    private float dashCdTimer;
    
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public float speedHere;
    

    public float Drag;

    public bool readyToJump;
    public float jumpForce;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYscale;
    public float startYscale;
    [Header("KeyBinds")]
    public KeyCode jumpButton;
    public KeyCode runButton;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask GroundUNow;
    bool grounded;
    public bool crouching;


    public Transform orientation;
    public Transform PlayerCam;
    public Transform Player;
    float HInput;
    float VInput;
    Vector3 Direction;
 
    Rigidbody rb;
    public MovementState state;
    public enum MovementState
    {
	walking,
	running,
	crouching,
        dashing,
	air
    
    }  
    
    public bool dashing;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
	speed = walkSpeed;
	readyToJump = true;
        dashCdTimerGlobal = dashCdGlobal;
    }

    // Update is called once per frame
    private void Update()
    {
	Debug.DrawRay(transform.position, Vector3.up, Color.green, playerHeight * 0.5f + 1.2f);
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 1.2f, GroundUNow);
	crouching = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + 0.2f, GroundUNow);
        MyInput();
        SpeedControl();
	StateHandler();
	speedHere = speed;

        rb.useGravity = !OnSlope();
	OnSlope();
        

        
        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
        
        if (dashes > 0)
        {
            dashInTheAir = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Dash();
            }
        }

        if (dashes == 0)
        {
            dashInTheAir = false;
            dashing = false;
            dashCdTimerGlobal -= Time.deltaTime;
        }
        if (dashCdTimerGlobal <= 0)
        {
            dashes = 3;
            dashCdTimerGlobal = dashCdGlobal;
        }

    }
    private void StateHandler()
    {

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ReadyToJump), 0.02f);
        }

        if (dashing && dashInTheAir)
        {
            rb.drag = Drag;
            state = MovementState.dashing;
            speed = dashSpeed;
            
        }
	else if (grounded && Input.GetKey(KeyCode.LeftShift))
	{
	   state = MovementState.running;
	   speed = runSpeed;
	   rb.drag = 0.5f;
	}
	else if (grounded)
	{
	   state = MovementState.walking;
	   speed = walkSpeed;
	   rb.drag = Drag;
	}
	else
	{
	   state = MovementState.air;
	   rb.drag = 0f;
	}
        
	
	
    }
    private void Jump()
    {
	rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);	   
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ReadyToJump()
    {
	readyToJump = true;
    }
    private void OnTriggerEnter()
    {

    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void MyInput()
    {
        HInput = Input.GetAxisRaw("Horizontal");
        VInput = Input.GetAxisRaw("Vertical");

    }

    private void Movement()
    {
        Direction = orientation.forward * VInput + orientation.right * HInput;
        if (grounded)
        {
            rb.AddForce(Direction.normalized * speed * 15f, ForceMode.Force);
        }
	else if (!grounded)
	{
	    rb.AddForce(Direction.normalized * speed * 15f * 1, ForceMode.Force);
	}
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * speed * 15f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
	

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private bool OnSlope()
    {
	if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 1.2f, GroundUNow))
	{
             float angle = Vector3.Angle(Direction, slopeHit.normal);
             return angle < maxSlopeAngle && angle != 0;
	}

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
         return Vector3.ProjectOnPlane(Direction, slopeHit.normal).normalized;
    }


    private void Dash()
    {
         
         if (dashCdTimer > 0) return;
         else dashCdTimer = dashCd;

         dashing = true;
         dashes -= 1;
         Vector3 dashDirectionInTheAir = PlayerCam.forward * VInput + PlayerCam.right * HInput;
         Vector3 dashDirectionOnTheGround = orientation.forward * VInput + orientation.right * HInput;
         Vector3 forceToApply = dashDirectionInTheAir.normalized * DashForce + orientation.up * DashUpwardForce;
         Vector3 forceToApplyDirection = PlayerCam.forward * DashForce + orientation.up * DashUpwardForce;
         if (HInput == 0 && VInput == 0)
         {
             delayedForceToApply = forceToApplyDirection;
         }
         else
         {
             delayedForceToApply = forceToApply;
         }

         
         delayedForceToApplyDirection = dashDirectionInTheAir;

         Invoke(nameof(DelayedDashInPlaceForce), 0.025f);
         Invoke(nameof(ResetDash), DashDuration);
    }

    private Vector3 delayedForceToApply;
    private Vector3 delayedForceToApplyDirection;	

    private void DelayedDashInPlaceForce()
    {
         rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }
    private void DelayedDashForce()
    {
         rb.AddForce(delayedForceToApplyDirection, ForceMode.Impulse);
    }



    private void ResetDash()
    {
         dashing = false;
    }

}
