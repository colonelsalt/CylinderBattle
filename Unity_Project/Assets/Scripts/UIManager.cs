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
        Collector.OnPiPickup += OnUpdatePis;
        Collector.OnPiDrop += OnUpdatePis;
        Collector.OnPlusPickup += OnUpdatePluses;
        Collector.OnAllPisCollected += OnGameOver;
        Gun.OnGunFired += OnUpdateAmmo;
        WeaponManager.OnWeaponPickup += OnDisplayWeapon;
        WeaponManager.OnWeaponActivated += OnActivateWeapon;
        WeaponManager.OnWeaponDisabled += OnHideWeapon;
        PlayerHealth.OnPlayerDamaged += OnUpdateHealth;
        PlayerHealth.OnPlayerExtraLife += OnUpdateHealth;
        PlayerHealth.OnPlayerRespawn += OnResetDisplay;
        PowerupManager.OnPowerupReceived += OnPowerupReceived;

        if (m_PlayerHUDs.Length != GameManager.NUM_PLAYERS)
        {
            Debug.LogError("Incorrect number of PlayerHUDs assigned.");
        }
    }

    private void OnUpdatePis(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].UpdatePis(true);
    }

    private void OnUpdatePluses(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].UpdatePluses(true);
    }

    // Hide weapon images and text
    private void OnHideWeapon(Weapon type, int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].HideWeapon();
    }

    // Update and display new Player health
    private void OnUpdateHealth(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].UpdateHealthDisplay(true);
    }

    // Display new weapon received
    private void OnDisplayWeapon(Weapon type, int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].ShowWeapon(type, true);
    }

    private void OnActivateWeapon(Weapon type, int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].ActivateWeapon(type);
    }

    // Update and display ammo change
    private void OnUpdateAmmo(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].UpdateAmmoDisplay();
    }

    // Reset HUD after Player death
    private void OnResetDisplay(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].UpdateAll();
    }

    private void OnPowerupReceived(Powerup type, int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].ActivatePowerup(type);
    }

    private void OnGameOver(int numOfWinner)
    {
        Collector.OnPiPickup -= OnUpdatePis;
        Collector.OnPiDrop -= OnUpdatePis;
        Collector.OnPlusPickup -= OnUpdatePluses;
        Collector.OnAllPisCollected -= OnGameOver;
        Gun.OnGunFired -= OnUpdateAmmo;
        WeaponManager.OnWeaponActivated -= OnDisplayWeapon;
        WeaponManager.OnWeaponDisabled -= OnHideWeapon;
        PlayerHealth.OnPlayerDamaged -= OnUpdateHealth;
        PlayerHealth.OnPlayerExtraLife -= OnUpdateHealth;
        PlayerHealth.OnPlayerRespawn -= OnResetDisplay;

        m_GameOverTitle.enabled = true;
        m_GameOverTitle.text += "\nPlayer " + numOfWinner + " wins!";
    }
}
