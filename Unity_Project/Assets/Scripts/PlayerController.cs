using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // --------------------------------------------------------------

    // The character's running speed
    [SerializeField] private float m_RunSpeed = 5.0f;

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

    // --------------------------------------------------------------
    
    // Events
    public delegate void PlayerEvent(int playerNum);
    public static event PlayerEvent OnPlayerDeath;
    public static event PlayerEvent OnPlayerRespawn;

    // --------------------------------------------------------------

    public int PlayerNum
    {
        get
        {
            return m_PlayerNum;
        }
    }

    private Rigidbody m_RigidBody;

    private Animator m_Animator;

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

    // Whether the player is alive or not
    private bool m_IsAlive = true;

    private bool m_IsCrouching = false;

    // The time it takes to respawn
    private const float MAX_RESPAWN_TIME = 1.0f;
    private float m_RespawnTime = MAX_RESPAWN_TIME;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        m_SpawningPosition = transform.position;
    }

    private void Jump()
    {
        m_VerticalSpeed = Mathf.Sqrt(m_JumpHeight * m_Gravity);
    }

    private void BackFlip()
    {
        m_VerticalSpeed = Mathf.Sqrt(m_BackFlipHeight * m_Gravity);
        m_Animator.SetBool("IsBackflipping", true);
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

    private void UpdateMovementState()
    {
        // Get Player's movement input and determine direction and set run speed
        float horizontalInput = InputHelper.GetMovementX(m_PlayerNum);
        float verticalInput = InputHelper.GetMovementY(m_PlayerNum);

        m_MovementDirection = new Vector3(horizontalInput, 0, verticalInput);
        m_MovementSpeed = m_RunSpeed;
    }

    private void UpdateJumpState()
    {
        if (m_CharacterController.isGrounded)
        {
            m_Animator.SetBool("IsBackflipping", false);
        }

        // Character can jump when standing on the ground (and when not affected by Bomb explosion)
        if (InputHelper.JumpButtonPressed(m_PlayerNum) && m_CharacterController.isGrounded)
        {
            if (m_IsCrouching)
            {
                BackFlip();
            } else
            {
                Jump();
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
        // If the player is dead update the respawn timer and exit update loop
        if(!m_IsAlive)
        {
            UpdateRespawnTime();
            return;
        }

        // Update movement input
        UpdateMovementState();

        // Update jumping input and apply gravity
        UpdateCrouchState();
        UpdateJumpState();
        ApplyGravity();

        // Calculate actual motion
        m_CurrentMovementOffset = (m_MovementDirection * m_MovementSpeed + new Vector3(0, m_VerticalSpeed, 0)) * Time.deltaTime;

        if (m_RigidBody.isKinematic && !m_IsCrouching)
        {
            // Move character
            m_CharacterController.Move(m_CurrentMovementOffset);

            // If Player is using a GamePad, allow for manual rotation
            if (InputHelper.GamePadConnected())
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

    public void ActivatePhysicsReactions()
    {
        m_RigidBody.isKinematic = false;
        m_CharacterController.enabled = false;
        Invoke("DeactivatePhysicsReactions", m_ExplosionDazeTime);
    }

    private void DeactivatePhysicsReactions()
    {
        m_CharacterController.enabled = true;
        m_RigidBody.isKinematic = true;
    }

    public void Die()
    {
        m_IsAlive = false;
        m_RespawnTime = MAX_RESPAWN_TIME;

        GetComponentInChildren<Renderer>().enabled = false; // TEMPORARY!!!

        // TODO: Trigger death animation

        //OnPlayerDeath(GetPlayerNum());
    }

    private void UpdateRespawnTime()
    {
        m_RespawnTime -= Time.deltaTime;
        if (m_RespawnTime < 0.0f)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        GetComponentInChildren<Renderer>().enabled = true; // TEMPORARY!!!

        m_IsAlive = true;
        transform.position = m_SpawningPosition;
        transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        OnPlayerRespawn(m_PlayerNum);
    }
}
