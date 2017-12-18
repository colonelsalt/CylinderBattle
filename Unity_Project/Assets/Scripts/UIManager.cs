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
        Gun.OnGunFired += OnUpdateAmmo;
        PowerupManager.OnPowerupActivated += OnDisplayPowerup;
        Health.OnPlayerHealthChange += OnUpdateHealth;

        if (m_PlayerHUDs.Length != GameManager.NUM_PLAYERS)
        {
            Debug.LogError("Incorrect number of PlayerHUDs assigned.");
        }
    }

    private void OnUpdateHealth(int playerNum, int healthChange)
    {
        m_PlayerHUDs[playerNum - 1].IncrementHealthDisplayBy(healthChange);
    }

    private void OnDisplayPowerup(Powerup type, int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].ShowPowerup(type);
    }

    private void OnUpdateAmmo(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].DecrementAndUpdateAmmo();
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
        DeathTrigger.OnPlayerDeath -= OnResetPluses;
        Pi.OnPiCaptured -= OnUpdateScore;
        Plus.OnPlusCaptured -= OnUpdatePluses;
        GameManager.OnGameOver -= OnGameOver;
        Gun.OnGunFired -= OnUpdateAmmo;
        PowerupManager.OnPowerupActivated -= OnDisplayPowerup;
        Health.OnPlayerHealthChange -= OnUpdateHealth;

        m_GameOverTitle.enabled = true;
        m_GameOverTitle.text += "\nPlayer " + numOfWinner + " wins!";
    }
}
