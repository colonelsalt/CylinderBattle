using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    // --------------------------------------------------------------

    public delegate void BreakableEvent();
    public static event BreakableEvent OnAllObjectsBroken;

    // --------------------------------------------------------------

    [SerializeField] private GameObject m_ShatteredVersion;

    // How large change in object's velocity in one frame should be for it to break 
    [SerializeField] private float m_BreakThreshold;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_BreakSounds;

    // --------------------------------------------------------------

    private Rigidbody m_Body;

    private Vector3 m_LastVelocity;

    private Collider m_Collider;

    private bool m_IsBroken = false;

    private static int m_NumBreakables = 0;

    // --------------------------------------------------------------

    public static int NumBreakables
    {
        get
        {
            return m_NumBreakables;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        if (m_NumBreakables == 0)
        {
            GameManager.OnGameOver += OnGameOver;
            GameManager.OnGameExit += OnResetBreakableCount;
        }

        m_Collider = GetComponent<Collider>();
        m_Body = GetComponent<Rigidbody>();
        m_NumBreakables++;
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
        m_NumBreakables--;
        if (m_NumBreakables <= 0)
        {
            OnAllObjectsBroken();
        }

        Destroy(gameObject);
    }

    private static void OnResetBreakableCount()
    {
        m_NumBreakables = 0;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameExit -= OnResetBreakableCount;
    }

    private static void OnGameOver(int winnerNum)
    {
        OnResetBreakableCount();
    }

}
