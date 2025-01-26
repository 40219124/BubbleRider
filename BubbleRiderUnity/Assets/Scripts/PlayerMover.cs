using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{

    Rigidbody2D Body;

    [SerializeField]
    float MoveSpeed = 5f;
    float Acceleration
    {
        get { return MoveSpeed * 3f * (TimeSinceLanded < 0.1f ? 3f : 1f); }
    }
    [SerializeField]
    float JumpStrength = 6f;
    float MaxJumpTime = 0.3f;
    [SerializeField]
    float GravityStrength = -9.8f;
    float TimeSinceJumped = 1000f;

    [SerializeField]
    float _groundCount = 0;
    float GroundCount
    {
        get { return _groundCount; }
        set
        {
            if (_groundCount == 0 && value > 0)
            {
                if (TimeSinceAirborne > MaxJumpTime)
                {
                    TimeSinceLanded = 0f;
                }
            }
            _groundCount = value;
            if (_groundCount == 0)
            {
                TimeSinceAirborne = 0f;
            }
        }

    }
    float TimeSinceAirborne = 100f;
    float TimeSinceLanded = 100f;
    bool IsGrounded
    {
        get { return GroundCount > 0; }
    }
    [SerializeField]
    Collider2D GroundCheckCollider;

    Vector2 LastInputDir;
    bool InputPushed;
    float JumpBuffer = 0.1f;
    float JumpRequested = 0f;
    bool InteractPushed;

    InputAction MoveAction;
    InputAction JumpAction;
    InputAction InteractAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Body = GetComponent<Rigidbody2D>();

        MoveAction = InputSystem.actions.FindAction("Move");
        JumpAction = InputSystem.actions.FindAction("Jump");
        InteractAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        JumpRequested -= Time.deltaTime;
        InputPushed = MoveAction.IsPressed();
        if (InputPushed)
        {
            LastInputDir = MoveAction.ReadValue<Vector2>();
        }

        if (JumpAction.WasPressedThisFrame())
        {
            Debug.Log($"Grounded:{IsGrounded}, Jump:{TimeSinceJumped}, Airborne:{TimeSinceAirborne}");
        }
        if (IsGrounded || (TimeSinceJumped > MaxJumpTime && TimeSinceAirborne < 0.1f))
        {
            if (JumpAction.WasPerformedThisFrame())
            {
                Debug.Log("Passed jump check");
            }
            JumpRequested = JumpAction.WasPressedThisFrame() ? JumpBuffer : JumpRequested;
        }
        InteractPushed = InteractAction.WasPressedThisFrame();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimeSinceJumped += Time.fixedDeltaTime;
        TimeSinceAirborne += Time.fixedDeltaTime;
        TimeSinceLanded += Time.fixedDeltaTime;
        if (JumpRequested > 0f)
        {
            JumpRequested = 0f;
            TimeSinceJumped = 0f;
            Body.linearVelocityY = JumpStrength;
        }
        if (TimeSinceJumped >= MaxJumpTime && !IsGrounded)
        {
            Body.linearVelocityY = AccelerateTowards(Vector2.up * GravityStrength, Body.linearVelocityY * Vector2.up, Mathf.Abs(GravityStrength * 3f), Time.fixedDeltaTime).y;
        }

        Vector2 move = InputPushed ? LastInputDir : Body.linearVelocityX != 0 ? (Mathf.Sign(-Body.linearVelocityX) * Vector2.right) : Vector2.zero;
        move.y = 0;
        Vector2 horVal = Vector2.right * Mathf.Clamp(move.x / 0.8f, -1f, 1f);
        Vector2 desiredHor = horVal * MoveSpeed;

        Vector2 x = AccelerateTowards(desiredHor, Body.linearVelocityX * Vector2.right, Acceleration, Time.fixedDeltaTime);

        //Vector2 diff = horVal - (Body.linearVelocityX * Vector2.right);

        //Vector2 possible = horVal * Acceleration * Time.fixedDeltaTime;
        //if (possible.sqrMagnitude > diff.sqrMagnitude)
        //{
        Body.linearVelocityX = x.x;
        //}
        //else
        //{
        //    Body.linearVelocityX += possible.x;
        //}
        if (Body.linearVelocityX * Body.linearVelocityX < 0.01f)
        {
            Body.linearVelocityX = 0f;
        }
    }

    private Vector2 AccelerateTowards(Vector2 goalSpeed, Vector2 currentSpeed, float acceleration, float deltaTime)
    {
        Vector2 attainedSpeed;

        Vector2 diff = goalSpeed - currentSpeed;
        Vector2 pot = diff.normalized * acceleration * deltaTime;
        if (pot.sqrMagnitude > diff.sqrMagnitude)
        {
            pot = diff;
        }
        attainedSpeed = currentSpeed + pot;

        return attainedSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground") && collision.otherCollider == GroundCheckCollider)
        {
            GroundCount++;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground") && collision.otherCollider == GroundCheckCollider)
        {
            GroundCount--;
        }
    }
}
