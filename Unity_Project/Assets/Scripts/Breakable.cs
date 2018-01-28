using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object that can be broken when enough force applied to it
public class Breakable : MonoBehaviour
{
    // --------------------------------------------------------------

    public delegate void BreakableEvent();
    public static event BreakableEvent OnAllObjectsBroken;

    // --------------------------------------------------------------

    // Identical version of object consisting of splintered mini-meshes; to be spawned when object broken
    [SerializeField] private GameObject m_ShatteredVersion;

    // How large change in object's velocity in one frame should be for it to break 
    [SerializeField] private float m_BreakThreshold;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_BreakSounds;

    // --------------------------------------------------------------

    // Ref. to RigidBody to keep track of velocity change
    private Rigidbody m_Body;

    private Vector3 m_LastVelocity;

    private Collider m_Collider;

    private bool m_IsBroken = false;

    // Counter for no. of Breakable instances in level (tracked for achievement)
    private static int NUM_BREAKABLES = 0;

    // --------------------------------------------------------------

    public static int NumBreakables
    {
        get
        {
            return NUM_BREAKABLES;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        // Ensure static listeners only set up once per scene
        if (NUM_BREAKABLES == 0)
        {
            GameManager.OnGameOver += OnGameOver;
            GameManager.OnGameExit += OnResetBreakableCount;
        }

        m_Collider = GetComponent<Collider>();
        m_Body = GetComponent<Rigidbody>();
        NUM_BREAKABLES++;
    }

    private void Update()
    {
        if (m_IsBroken) return;
            
        Vector3 velocityChange = m_LastVelocity - m_Body.velocity;
        if (velocityChange.magnitude > m_BreakThreshold)
        {
            Break();
        }
        m_LastVelocity = m_Body.velocity;
    }

    private void Break()
    {
        SoundManager.Instance.PlayRandom(m_BreakSounds);
        m_IsBroken = true;
        m_Collider.enabled = false;
        Instantiate(m_ShatteredVersion, transform.position, transform.rotation);
        NUM_BREAKABLES--;
        if (NUM_BREAKABLES <= 0)
        {
            OnAllObjectsBroken();
        }

        Destroy(gameObject);
    }

    // Ensure count reset on level left
    private static void OnResetBreakableCount()
    {
        NUM_BREAKABLES = 0;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameExit -= OnResetBreakableCount;
    }

    private static void OnGameOver(int winnerNum)
    {
        OnResetBreakableCount();
    }

}
