﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Collector : MonoBehaviour
{
    // --------------------------------------------------------------

    // Events
    public delegate void AllPisCollectedEvent(int playerNum);
    public static event AllPisCollectedEvent OnAllPisCollected;

    // --------------------------------------------------------------
    
    // Number of Pluses collected
    private int m_NumPluses = 0;

    // Number of Pis collected
    private int m_NumPis = 0;

    private PlayerController m_Player;

    // --------------------------------------------------------------


    private void Awake()
    {
        Collectible.OnCollectiblePickup += OnCollectiblePickup;
        PlayerController.OnPlayerRespawn += OnResetPlusCount;
        m_Player = GetComponent<PlayerController>();
    }

    private void OnCollectiblePickup(Collectible.Type type, int playerNum)
    {
        // Make sure this Player is the one who collected
        if (playerNum != m_Player.PlayerNum) return;

        switch (type)
        {
            case Collectible.Type.PLUS:
                m_NumPluses++;
                CheckForPlusBonus();
                break;
            case Collectible.Type.PI:
                m_NumPis++;
                if (m_NumPis >= GameManager.MAX_NUM_PIS)
                {
                    OnAllPisCollected(m_Player.PlayerNum);
                }
                break;
        }
    }

    private void OnResetPlusCount(int playerNum)
    {
        if (m_Player.PlayerNum == playerNum)
        {
            m_NumPluses = 0;
        }
    }

    private void CheckForPlusBonus()
    {
        
    }
}