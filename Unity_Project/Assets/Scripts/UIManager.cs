using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // --------------------------------------------------------------

    // HUD object for each Player in game
    [SerializeField] private PlayerHUD[] m_PlayerHUDs;

    [SerializeField] private Text m_GameOverTitle;
    
    // --------------------------------------------------------------

    void OnEnable()
    {
        Collectible.OnCollectiblePickup += OnUpdateScore;
        Collector.OnAllPisCollected += OnGameOver;
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

    private void OnUpdateScore(Collectible.Type type, int playerNum)
    {
        switch (type)
        {
            case Collectible.Type.PLUS:
                m_PlayerHUDs[playerNum - 1].IncrementAndUpdatePluses();
                break;
            case Collectible.Type.PI:
                m_PlayerHUDs[playerNum - 1].IncrementAndUpdatePis();
                break;
        }
    }

    // Hide powerup images and text
    private void OnHidePowerup(Powerup type, int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].HidePowerup();
    }

    // Update and display new Player health
    private void OnUpdateHealth(int playerNum, int healthChange)
    {
        m_PlayerHUDs[playerNum - 1].UpdateHealthDisplay(healthChange);
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

    private void OnGameOver(int numOfWinner)
    {
        Collectible.OnCollectiblePickup -= OnUpdateScore;
        Collector.OnAllPisCollected -= OnGameOver;
        Gun.OnGunFired -= OnUpdateAmmo;
        PowerupManager.OnPowerupActivated -= OnDisplayPowerup;
        PowerupManager.OnPowerupDisabled -= OnHidePowerup;
        Health.OnPlayerHealthChange -= OnUpdateHealth;
        PlayerController.OnPlayerRespawn -= OnResetDisplay;

        m_GameOverTitle.enabled = true;
        m_GameOverTitle.text += "\nPlayer " + numOfWinner + " wins!";
    }
}
