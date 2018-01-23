using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Middleman class between UI / backend class and Player stats
public class ScoreRetriever : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_Player;

    // --------------------------------------------------------------

    // References to all relevant Player components

    private PlayerController m_PlayerController;

    private PlayerHealth m_PlayerHealth;

    private Collector m_Collector;

    private Gun m_Gun;

    private BoxingGlove m_BoxingGloves;

    private LightningSprint m_LightningSprint;

    // --------------------------------------------------------------

    // Public accessors for retrieving Player stats

    public int PlayerNum
    {
        get
        {
            return m_PlayerController.PlayerNum;
        }
    }

    public int NumPis
    {
        get
        {
            return m_Collector.NumPis;
        }
    }

    public int NumPluses
    {
        get
        {
            return m_Collector.NumPluses;
        }
    }

    public int RemainingAmmo
    {
        get
        {
            if (m_Gun == null) GetGun();

            return m_Gun.RemainingAmmo;
        }
    }

    public int Health
    {
        get
        {
            return m_PlayerHealth.Health;
        }
    }

    public float BoxingTimeRemaining
    {
        get
        {
            if (m_BoxingGloves == null) GetBoxingGloves();

            return m_BoxingGloves.TimeRemaining;
        }
    }

    public float SprintTimeRemaining
    {
        get
        {
            if (m_LightningSprint == null) GetLightningSprint();

            return m_LightningSprint?.RemainingSprintTime ?? 0f;
        }
    }

    public float MaxSprintTime
    {
        get
        {
            if (m_LightningSprint == null) GetLightningSprint();

            return m_LightningSprint.MaxSprintTime;
        }
    }

    public bool PlayerHasStamina
    {
        get
        {
            if (m_LightningSprint == null) GetLightningSprint();

            return m_LightningSprint.HasStamina;
        }
    }

    public bool IsPlayerAlive
    {
        get
        {
            return m_PlayerHealth.IsAlive;
        }
    }

    public Vector3 PlayerPosition
    {
        get
        {
            return m_Player.transform.position;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        m_PlayerController = m_Player.GetComponent<PlayerController>();
        m_PlayerHealth = m_Player.GetComponent<PlayerHealth>();
        m_Collector = m_Player.GetComponent<Collector>();
    }

    private void GetGun()
    {
        m_Gun = m_Player.GetComponentInChildren<Gun>();
    }

    private void GetBoxingGloves()
    {
        m_BoxingGloves = m_Player.GetComponentInChildren<BoxingGlove>();
    }

    private void GetLightningSprint()
    {
        m_LightningSprint = m_Player.GetComponentInChildren<LightningSprint>();
    }
}
