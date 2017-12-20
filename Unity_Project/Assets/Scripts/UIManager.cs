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
        Pi.OnPiCaptured += OnUpdateScore;
        Plus.OnPlusCaptured += OnUpdatePluses;
        GameManager.OnGameOver += OnGameOver;
        Gun.OnGunFired += OnUpdateAmmo;
        PowerupManager.OnPowerupActivated += OnDisplayPowerup;
        PowerupManager.OnPowerupDisabled += OnHidePowerup;
        Health.OnPlayerHealthChange += OnUpdateHealth;
        PlayerController.OnPlayerRespawn += OnResetDisplay;

        if (m_PlayerHUDs.Length != GameManager.NUM_PLAYERS)
        {
            Debug.LogError("Incorrect number of PlayerHUDs assigned.");
        }
    }

    private void OnHidePowerup(Powerup type, int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].HidePowerup();
    }

    // Update and display new Player health
    private void OnUpdateHealth(int playerNum, int newHealth)
    {
        m_PlayerHUDs[playerNum - 1].UpdateHealthDisplay(newHealth);
    }

    // Display new powerup received
    private void OnDisplayPowerup(Powerup type, int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].ShowPowerup(type);
    }

    // Update and display ammo change
    private void OnUpdateAmmo(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].UpdateAmmoDisplay();
    }

    // Reset HUD after Player death
    private void OnResetDisplay(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].DeathReset();
    }

    // Update and display number of Pluses collected
    private void OnUpdatePluses(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].IncrementAndUpdatePluses();
    }

    private void OnUpdateScore(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].IncrementAndUpdatePis();
    }

    private void OnGameOver(int numOfWinner)
    {
        Pi.OnPiCaptured -= OnUpdateScore;
        Plus.OnPlusCaptured -= OnUpdatePluses;
        GameManager.OnGameOver -= OnGameOver;
        Gun.OnGunFired -= OnUpdateAmmo;
        PowerupManager.OnPowerupActivated -= OnDisplayPowerup;
        PowerupManager.OnPowerupDisabled -= OnHidePowerup;
        Health.OnPlayerHealthChange -= OnUpdateHealth;
        PlayerController.OnPlayerRespawn -= OnResetDisplay;

        m_GameOverTitle.enabled = true;
        m_GameOverTitle.text += "\nPlayer " + numOfWinner + " wins!";
    }
}
