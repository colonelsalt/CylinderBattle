using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    // --------------------------------------------------------------

    [SerializeField] private Text m_NumPisText;

    [SerializeField] private Text m_NumPlusesText;

    [SerializeField] private Text m_AmmoText;

    [SerializeField] private Text m_HealthText;

    [SerializeField] private Image m_PowerupImage;

    [SerializeField] private Texture[] m_PowerupImages;

    // --------------------------------------------------------------

    private float m_RemainingPowerupTime;

    private int m_NumPis = 0;

    private int m_NumPluses = 0;

    private int m_Health;

    private int m_Ammo;

    private bool m_TimerActive = false;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Health = GameManager.PLAYER_HEALTH;

        m_NumPisText.text = m_NumPis + "/" + GameManager.MAX_NUM_PIS;
        m_NumPlusesText.text = "0";
        m_HealthText.text = "x" + m_Health;
    }

    private void Update()
    {
        if (m_TimerActive)
        {
            m_RemainingPowerupTime -= Time.deltaTime;
            m_AmmoText.text = m_RemainingPowerupTime.ToString();
        }
    }

    public void IncrementAndUpdatePis()
    {
        m_NumPis++;
        m_NumPisText.text = m_NumPis + "/" + GameManager.MAX_NUM_PIS;
    }

    public void IncrementAndUpdatePluses()
    {
        m_NumPluses++;
        m_NumPlusesText.text = m_NumPluses.ToString();
    }

    public void DeathReset()
    {
        m_NumPluses = 0;
        m_Health = GameManager.PLAYER_HEALTH;
        m_NumPlusesText.text = m_NumPluses.ToString();
        m_HealthText.text = "x" + m_Health;
    }

    public void UpdateHealthDisplay(int newHealth)
    {
        m_HealthText.text = "x" + newHealth.ToString();
    }

    public void ShowPowerup(Powerup type)
    {
        switch (type)
        {
            case Powerup.BOMB:
                break;
            case Powerup.GUN:
                m_Ammo = Gun.MAX_AMMO;
                m_AmmoText.enabled = true;
                m_AmmoText.text = m_Ammo.ToString();
                break;
            case Powerup.BOXING_GLOVES:
                m_RemainingPowerupTime = GameManager.POWERUP_DURATION;
                // TEMPORARY SOLUTION! (show timer with powerup text)
                m_AmmoText.enabled = true;
                m_AmmoText.text = m_RemainingPowerupTime.ToString();
                m_TimerActive = true;
                break;
        }

        m_PowerupImage.enabled = true;
    }

    public void HidePowerup()
    {
        m_AmmoText.enabled = false;
        m_PowerupImage.enabled = false;
    }

    public void UpdateAmmoDisplay()
    {
        m_Ammo--;
        m_AmmoText.text = m_Ammo.ToString();
    }
}
