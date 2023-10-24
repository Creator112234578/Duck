using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float currentSpeed;
    public float WalkSpeed;
    public float CurrentSpeed
    {
        get
        {
            return currentSpeed;
        }
    }
    public float Drag;
    public MovementState state;
    private Vector3 Direction;
    [Header("Running")]
    public bool CanRun;
    public float RunSpeed;
    [Header("Dashing")]
    public bool CanDash;
    public float DashCnt;
    public float DashSpeed;
    public float DashForce;
    public float DashUpwardForce;
    public float DashDuration;
    public float DashCooldownGlobal;
    public float DashCooldownTimerGlobal;
    public float DashCooldown;
    private float DashCooldownTimer;
    [HideInInspector]
    public bool IsDashing;
    [Header("Slopes")]
    public float MaxSlopeAngle;
    private RaycastHit slopeHit;
    private float slopeAngle;
    [Header("Jumping")]
    public bool CanJump;
    public float JumpingForce;
    [Header("Crouching")]
    public bool CanCrouch;
    public float CrouchSpeed;
    public float CrouchYscale;
    public float DefaultYscale;
    [Header("KeyBinds")]
    public KeyCode JumpKey;
    public KeyCode RunKey;
    public KeyCode CrouchKey;
    public KeyCode DashKey;
    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask GroundUNow;
    private bool onGround;

    [Header("Misc")]
    public Transform orientation;
    public Transform PlayerCam;
    public Transform Player;

    private float hInput;
    private float vInput;
    private Rigidbody rb;
    public enum MovementState
    {
        walking,
        running,
        crouching,
        dashing,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentSpeed = WalkSpeed;
        CanJump = true;
        DashCooldownTimerGlobal = DashCooldownGlobal;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.up, Color.green, PlayerHeight * 0.5f + 1.2f);
        onGround = Physics.Raycast(transform.position, Vector3.down, out slopeHit, PlayerHeight * 0.5f + 1.2f, GroundUNow);

        // Slope check
        slopeAngle = 0;
        if (onGround)
        {
            slopeAngle = Vector3.Angle(Direction, slopeHit.normal);
        }
        if (slopeAngle >= MaxSlopeAngle)
        {
            slopeAngle = 0;
        }
        // Speed control
        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        Debug.Log($"{(onGround ? "" : "not")} on the ground, can{(CanRun ? "" : "t")} run, can{(CanJump ? "" : "t")} jump, can{(CanDash ? "" : "t")} dash");
        StateHandler();
        rb.useGravity = slopeAngle == 0;
        if (DashCooldownTimer > 0)
        {
            DashCooldownTimer -= Time.deltaTime;
        }
        if (DashCnt > 0)
        {
            CanDash = true;
            if (Input.GetKeyDown(DashKey))
            {
                Dash();
            }
        }
        if (DashCnt == 0)
        {
            CanDash = false;
            IsDashing = false;
            DashCooldownTimerGlobal -= Time.deltaTime;
        }
        if (DashCooldownTimerGlobal <= 0)
        {
            DashCnt = 3;
            DashCooldownTimerGlobal = DashCooldownGlobal;
        }
    }
    private void StateHandler()
    {
        if (onGround && CanJump && Input.GetKeyDown(JumpKey))
        {
            CanJump = false;
            Jump();
            Invoke(nameof(ResetJump), 0.02f);
        }
        else if (onGround && CanRun && Input.GetKey(RunKey))
        {
            rb.drag = Drag;
            currentSpeed = RunSpeed;
            state = MovementState.running;
        }
        else if (onGround && CanCrouch && Input.GetKey(CrouchKey))
        {
            rb.drag = Drag;
            currentSpeed = CrouchSpeed;
            state = MovementState.crouching;
        }
        else if (onGround)
        {
            rb.drag = Drag;
            currentSpeed = WalkSpeed;
            state = MovementState.walking;
        }
        else if (IsDashing && CanDash)
        {
            rb.drag = Drag;
            currentSpeed = DashSpeed;
            state = MovementState.dashing;
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
        rb.AddForce(transform.up * JumpingForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        CanJump = true;
    }
    private void FixedUpdate()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");
        Direction = orientation.forward * vInput + orientation.right * hInput;
        if (onGround)
        {
            rb.AddForce(15f * currentSpeed * Direction.normalized, ForceMode.Force);
        }
        else if (!onGround)
        {
            rb.AddForce(1 * 15f * currentSpeed * Direction.normalized, ForceMode.Force);
        }
        if (slopeAngle != 0)
        {
            rb.AddForce(15f * currentSpeed * GetSlopeMoveDirection(), ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(Direction, slopeHit.normal).normalized;
    }
    private void Dash()
    {
        if (DashCooldownTimer > 0)
        {
            return;
        }
        else
        {
            DashCooldownTimer = DashCooldown;
        }
        IsDashing = true;
        DashCnt -= 1;
        Vector3 dashDirectionInTheAir = PlayerCam.forward * vInput + PlayerCam.right * hInput;
        Vector3 dashDirectionOnTheGround = orientation.forward * vInput + orientation.right * hInput;
        Vector3 forceToApply = dashDirectionInTheAir.normalized * DashForce + orientation.up * DashUpwardForce;
        Vector3 forceToApplyDirection = PlayerCam.forward * DashForce + orientation.up * DashUpwardForce;
        if (hInput == 0 && vInput == 0)
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
    private void ResetDash()
    {
        IsDashing = false;
    }
}
