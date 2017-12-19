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

    private int m_NumPis = 0;
    private int m_NumPluses = 0;
    private int m_Health;
    private int m_Ammo;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_NumPisText.text = m_NumPis + "/" + GameManager.MAX_NUM_PIS;
        m_NumPlusesText.text = m_NumPluses.ToString();
        m_Health = GameManager.PLAYER_HEALTH;
        m_HealthText.text = "x" + m_Health;
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

    public void ResetPluses()
    {
        m_NumPluses = 0;
        m_NumPlusesText.text = m_NumPluses.ToString();
    }

    public void IncrementHealthDisplayBy(int amount)
    {
        m_Health += amount;
        m_HealthText.text = "x" + m_Health.ToString();
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
                break;
        }
    }

    public void DecrementAndUpdateAmmo()
    {
        m_Ammo--;
        m_AmmoText.text = m_Ammo.ToString();
    }
}
