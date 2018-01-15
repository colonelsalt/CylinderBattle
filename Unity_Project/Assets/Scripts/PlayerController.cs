using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_WalkSpeed = 5f;

    // The character's running speed
    [SerializeField] private float m_RunSpeed = 10f;

    // The gravity strength
    [SerializeField] private float m_Gravity = 60.0f;

    // The maximum speed the character can fall
    [SerializeField] private float m_MaxFallSpeed = 20.0f;

    // The character's jump height
    [SerializeField] private float m_JumpHeight = 4.0f;

    [SerializeField] private float m_BackFlipHeight = 8f;

    [SerializeField] private int m_PlayerNum;

    // After struck by an explosion, how long Player will remain immobile
    [SerializeField] private float m_ExplosionDazeTime;

    // How long Player can float in the air when they have the jetpack
    [SerializeField] private float m_JetpackFloatTime;

    // --------------------------------------------------------------

    private Rigidbody m_RigidBody;

    private Animator m_Animator;

    private PowerupManager m_PowerupManager;

    // The charactercontroller of the player
    private CharacterController m_CharacterController;

    // The current movement direction in x & z.
    private Vector3 m_MovementDirection = Vector3.zero;

    // The current movement speed
    private float m_MovementSpeed = 0.0f;

    // The current vertical / falling speed
    private float m_VerticalSpeed = 0.0f;

    // The current movement offset
    private Vector3 m_CurrentMovementOffset = Vector3.zero;

    // The starting position of the player
    private Vector3 m_SpawningPosition = Vector3.zero;

    private bool m_IsCrouching = false;

    private bool m_IsFloating = false;

    private bool m_IsBackFlipping = false;

    private float m_RemainingFloatTime;

    // --------------------------------------------------------------

    public int PlayerNum
    {
        get
        {
            return m_PlayerNum;
        }
    }

    public bool IsRunning
    {
        get
        {
            return m_MovementSpeed == m_RunSpeed;
        }
        set
        {
            if (value)
            {
                m_MovementSpeed = m_RunSpeed;
            }
            else
            {
                m_MovementSpeed = m_WalkSpeed;
            }
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();
        m_PowerupManager = GetComponent<PowerupManager>();

        m_MovementSpeed = m_WalkSpeed;
    }

    private void Start()
    {
        m_SpawningPosition = transform.position;
    }

    public void Jump(float heightToJump)
    {
        m_VerticalSpeed = Mathf.Sqrt(heightToJump * m_Gravity);
    }

    private void BackFlip()
    {
        m_VerticalSpeed = Mathf.Sqrt(m_BackFlipHeight * m_Gravity);
        m_IsBackFlipping = true;
        GetUp();
    }

    private void ApplyGravity()
    {
        // Apply gravity
        m_VerticalSpeed -= m_Gravity * Time.deltaTime;

        // Make sure we don't fall any faster than m_MaxFallSpeed.
        m_VerticalSpeed = Mathf.Max(m_VerticalSpeed, -m_MaxFallSpeed);
        m_VerticalSpeed = Mathf.Min(m_VerticalSpeed, m_MaxFallSpeed);
    }

    private void UpdateMovementDirection()
    {
        // Get Player's movement input and determine direction and set run speed
        float horizontalInput = InputHelper.GetMovementX(m_PlayerNum);
        float verticalInput = InputHelper.GetMovementY(m_PlayerNum);

        m_MovementDirection = new Vector3(horizontalInput, 0, verticalInput);
    }

    private void UpdateJumpState()
    {
        if (m_CharacterController.isGrounded || m_IsFloating)
        {
            m_IsBackFlipping = false;
        }

        // Character can jump when standing on the ground
        if (InputHelper.JumpButtonPressed(m_PlayerNum) && m_CharacterController.isGrounded)
        {
            if (m_IsCrouching)
            {
                BackFlip();
            }
            else
            {
                Jump(m_JumpHeight);
            }
        }
    }

    private void UpdateCrouchState()
    {
        if (InputHelper.CrouchButtonPressed(m_PlayerNum) && m_CharacterController.isGrounded && !m_IsCrouching)
        {
            Crouch();
        }
        if (InputHelper.CrouchButtonRealeased(m_PlayerNum) && m_IsCrouching)
        {
            GetUp();
        }
    }

    private void UpdateFloatState()
    {
        if (InputHelper.JumpButtonPressed(m_PlayerNum) && m_PowerupManager.HasJetpack
            && !m_CharacterController.isGrounded && m_RemainingFloatTime > 0f)
        {
            m_IsFloating = true;
            m_PowerupManager.SetJetpackActive(true);

            // Make sure there is no extra movement along y-axis
            m_VerticalSpeed = 0f;
        }
        else if (InputHelper.JumpButtonReleased(m_PlayerNum) || m_RemainingFloatTime <= 0f)
        {
           m_IsFloating = false;
            m_PowerupManager.SetJetpackActive(false);
        } 
    }

    private void Crouch()
    {
        transform.Translate(0f, -0.5f, 0f);
        transform.Rotate(90f, 0f, 0f);
        m_IsCrouching = true;
    }

    private void GetUp()
    {
        transform.Rotate(-90f, 0f, 0f);
        transform.Translate(0f, 0.5f, 0f);
        m_IsCrouching = false;
    }

    private void Update()
    {
        // Update movement input
        UpdateMovementDirection();

        // Update jumping input and apply gravity
        UpdateCrouchState();
        UpdateJumpState();
        UpdateFloatState();

        UpdateAnimationState();

        if (m_IsFloating)
        {
            m_RemainingFloatTime -= Time.deltaTime;
        }
        else
        {
            ApplyGravity();
            if (m_CharacterController.isGrounded)
            {
                m_RemainingFloatTime = m_JetpackFloatTime;
            }
        }

        // Calculate actual motion
        m_CurrentMovementOffset = (m_MovementDirection * m_MovementSpeed + new Vector3(0, m_VerticalSpeed, 0)) * Time.deltaTime;

        if (m_RigidBody.isKinematic && !m_IsCrouching)
        {
            // Move character
            m_CharacterController.Move(m_CurrentMovementOffset);

            // If Player is using a GamePad, allow for manual rotation
            if (InputHelper.GamePadConnected(m_PlayerNum))
            {
                RotateFromGamePad();
            }
            // Otherwise, rotate the character in movement direction
            else if (m_MovementDirection != Vector3.zero)
            {
                RotateCharacter(m_MovementDirection);
            }
        }
    }

    private void RotateCharacter(Vector3 movementDirection)
    {
        Quaternion lookRotation = Quaternion.LookRotation(movementDirection);
        if (transform.rotation != lookRotation)
        {
            transform.rotation = lookRotation;
        }
    }

    private void RotateFromGamePad()
    {
        float cos = InputHelper.GetRightStickX(m_PlayerNum);
        float sin = InputHelper.GetRightStickY(m_PlayerNum);

        // If right stick is in neutral position, simply rotate to movementDirection as normal
        if (cos == 0f && sin == 0f)
        {
            if (m_MovementDirection != Vector3.zero) RotateCharacter(m_MovementDirection);
            return;
        }

        // Find angle represented by current stick position
        float rotationAngle = Mathf.Atan2(cos, sin) * Mathf.Rad2Deg;

        // Rotate player by this angle around the y-axis
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationAngle, transform.eulerAngles.z);
    }

    private void UpdateAnimationState()
    {
        m_Animator.SetBool("IsBackflipping", m_IsBackFlipping);

        bool isWalking = (m_MovementDirection != Vector3.zero && !IsRunning);
        m_Animator.SetBool("IsWalking", isWalking);
    }
}
