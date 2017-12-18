using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private PlayerHUD[] m_PlayerHUDs;
    [SerializeField] private Text m_GameOverTitle;
    
    // --------------------------------------------------------------

    void OnEnable()
    {
        DeathTrigger.OnPlayerDeath += OnResetPluses;
        Pi.OnPiCaptured += OnUpdateScore;
        Plus.OnPlusCaptured += OnUpdatePluses;
        GameManager.OnGameOver += OnGameOver;
        if (m_PlayerHUDs.Length != GameManager.NUM_PLAYERS)
        {
            Debug.LogError("Incorrect number of PlayerHUDs assigned.");
        }
    }

    private void OnResetPluses(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].ResetPluses();
    }

    private void OnUpdatePluses(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].IncrementAndUpdatePluses();
    }

    void OnUpdateScore(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].IncrementAndUpdatePis();
    }

    private void OnGameOver(int numOfWinner)
    {
        GameManager.OnGameOver -= OnGameOver;
        //DeathTrigger.OnPlayerDeath -= OnUpdateScore;
        Pi.OnPiCaptured -= OnUpdateScore;
        m_GameOverTitle.enabled = true;
        m_GameOverTitle.text += "\nPlayer " + numOfWinner + " wins!";
    }
}
