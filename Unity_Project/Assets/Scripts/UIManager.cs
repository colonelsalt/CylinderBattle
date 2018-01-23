using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // --------------------------------------------------------------

    // HUD object for each Player in game
    [SerializeField] private PlayerHUD[] m_PlayerHUDs;

    [SerializeField] private Text m_ObjectiveTitle;

    [SerializeField] private Text m_StartTitle;

    [SerializeField] private Text m_GameOverTitle;

    [SerializeField] private Text m_MatchPointTitle;

    [SerializeField] private Text m_PauseTitle;

    [SerializeField] private Image m_FadePanel;

    // --------------------------------------------------------------

    private Animator m_MatchPointAnim;

    private Animator m_FadePanelAnim;

    // --------------------------------------------------------------

    private void OnEnable()
    {
        GameManager.OnGameStart += OnGameStart;
        GameManager.OnGamePaused += OnGamePaused;
        GameManager.OnGameResumed += OnGameResumed;

        SceneManager.sceneLoaded += OnSceneLoaded;

        Collector.OnPiPickup += OnPiPickup;
        Collector.OnPiDrop += OnPiDrop;
        Collector.OnPlusPickup += OnUpdatePluses;
        Collector.OnAllPisCollected += OnGameOver;
        Collector.OnMatchPoint += OnMatchPoint;

        Gun.OnGunFired += OnUpdateAmmo;

        WeaponManager.OnWeaponPickup += OnWeaponPickup;
        WeaponManager.OnWeaponActivated += OnActivateWeapon;
        WeaponManager.OnWeaponDisabled += OnHideWeapon;

        PlayerHealth.OnPlayerDamaged += OnUpdateHealth;
        PlayerHealth.OnPlayerExtraLife += OnUpdateHealth;
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;
        PlayerHealth.OnPlayerRespawn += OnPlayerRespawn;

        PowerupManager.OnPowerupReceived += OnPowerupReceived;

        m_MatchPointAnim = m_MatchPointTitle.GetComponent<Animator>();
        m_FadePanelAnim = m_FadePanel.GetComponent<Animator>();

        if (m_PlayerHUDs.Length != GameManager.NUM_PLAYERS)
        {
            Debug.LogError("UIManager: Incorrect number of PlayerHUDs assigned.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_ObjectiveTitle.GetComponent<Animator>().SetTrigger("showTrigger");
        Destroy(m_ObjectiveTitle, 4f);
    }

    private void OnGameStart()
    {
        m_StartTitle.enabled = true;
        Destroy(m_StartTitle.gameObject, 2f);
    }

    private void OnGamePaused()
    {
        m_PauseTitle.enabled = true;
        m_FadePanelAnim.SetBool("isPaused", true);
    }

    private void OnGameResumed()
    {
        m_PauseTitle.enabled = false;
        m_FadePanelAnim.SetBool("isPaused", false);
    }

    private void OnPiPickup(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].UpdatePis(true);
    }

    private void OnPiDrop(int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].UpdatePis(false);
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
    private void OnWeaponPickup(Weapon type, int playerNum)
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

    private void OnPlayerDeath(int playerNum)
    {
        // Reset health count
        m_PlayerHUDs[playerNum - 1].UpdateHealthDisplay(true);

        // Reset plus count
        m_PlayerHUDs[playerNum - 1].UpdatePluses(false);

        // Remove powerup display if present
        m_PlayerHUDs[playerNum - 1].HidePowerup();
    }

    private void OnPlayerRespawn(int playerNum)
    {
        // Reset number of lives
        m_PlayerHUDs[playerNum - 1].UpdateHealthDisplay(false);
    }

    private void OnPowerupReceived(Powerup type, int playerNum)
    {
        m_PlayerHUDs[playerNum - 1].ActivatePowerup(type);
    }

    private void OnMatchPoint(int playerNum)
    {
        m_MatchPointAnim.SetTrigger("showTrigger");
    }

    private void OnGameOver(int numOfWinner)
    {
        Collector.OnPiPickup -= OnPiPickup;
        Collector.OnPiDrop -= OnPiPickup;
        Collector.OnPlusPickup -= OnUpdatePluses;
        Collector.OnAllPisCollected -= OnGameOver;
        Gun.OnGunFired -= OnUpdateAmmo;
        WeaponManager.OnWeaponActivated -= OnWeaponPickup;
        WeaponManager.OnWeaponDisabled -= OnHideWeapon;
        PlayerHealth.OnPlayerDamaged -= OnUpdateHealth;
        PlayerHealth.OnPlayerExtraLife -= OnUpdateHealth;
        PlayerHealth.OnPlayerRespawn -= OnPlayerRespawn;
        SceneManager.sceneLoaded -= OnSceneLoaded;

        m_GameOverTitle.enabled = true;
        m_GameOverTitle.text += "\nPlayer " + numOfWinner + " wins!";
    }
}
