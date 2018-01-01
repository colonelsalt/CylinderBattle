using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScoreKeeper))]
public class PlayerHUD : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private Text m_NumPisText;

    [SerializeField] private Text m_NumPlusesText;

    [SerializeField] private Text m_AmmoText;

    [SerializeField] private Text m_HealthText;

    [SerializeField] private Image m_WeaponImage;

    [SerializeField] private Texture[] m_WeaponImages;

    // --------------------------------------------------------------

    private ScoreKeeper m_Score;

    private bool m_TimerActive = false;

    // --------------------------------------------------------------

    private void Start()
    {
        m_Score = GetComponent<ScoreKeeper>();
        UpdateAll();
    }

    private void Update()
    {
        if (m_TimerActive)
        {
            m_AmmoText.text = m_Score.BoxingTimeRemaining.ToString();
        }
    }

    public void UpdateAll()
    {
        UpdatePis();
        UpdatePluses();
        UpdateHealthDisplay();
    }

    public void UpdatePis()
    {
        m_NumPisText.text = m_Score.NumPis + "/" + GameManager.MAX_NUM_PIS;
    }

    public void UpdatePluses()
    {
        m_NumPlusesText.text = m_Score.NumPluses.ToString();
    }

    public void UpdateHealthDisplay()
    {
        m_HealthText.text = "x" + m_Score.Health.ToString();
    }

    public void ShowWeapon(Weapon type)
    {
        switch (type)
        {
            case Weapon.BOMB:
                break;
            case Weapon.GUN:
                m_AmmoText.enabled = true;
                m_AmmoText.text = m_Score.RemainingAmmo.ToString();
                break;
            case Weapon.BOXING_GLOVES:
                // TEMPORARY SOLUTION! (show timer with powerup text)
                m_AmmoText.enabled = true;
                m_AmmoText.text = m_Score.BoxingTimeRemaining.ToString();
                m_TimerActive = true;
                break;
        }

        m_WeaponImage.enabled = true;
    }

    public void HideWeapon()
    {
        m_AmmoText.enabled = false;
        m_WeaponImage.enabled = false;
        m_TimerActive = false;
    }

    public void UpdateAmmoDisplay()
    {
        m_AmmoText.text = m_Score.RemainingAmmo.ToString();
    }
}
