﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_Player;

    // --------------------------------------------------------------

    private PlayerController m_PlayerController;

    private PlayerHealth m_PlayerHealth;

    private PowerupManager m_PowerupManager;

    private Collector m_Collector;

    private Gun m_Gun;

    private BoxingGlove m_BoxingGloves;

    // --------------------------------------------------------------

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
            return m_PlayerHealth.RemainingHealth;
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

    // --------------------------------------------------------------

    private void Awake()
    {
        m_PlayerController = m_Player.GetComponent<PlayerController>();
        m_PlayerHealth = m_Player.GetComponent<PlayerHealth>();
        m_PowerupManager = m_Player.GetComponent<PowerupManager>();
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
}