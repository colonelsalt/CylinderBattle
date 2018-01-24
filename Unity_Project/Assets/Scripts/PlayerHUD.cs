using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStats))]
public class PlayerHUD : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private PlayerStats m_PlayerStats;

    [SerializeField] private Text m_NumPisText;

    [SerializeField] private Text m_NumPlusesText;

    [SerializeField] private Text m_AmmoText;

    [SerializeField] private Text m_HealthText;

    [SerializeField] private Image m_WeaponImage;

    [SerializeField] private Image m_TimerImage;

    [SerializeField] private Image m_StaminaBar;

    [SerializeField] private Image m_LightningIcon;

    [SerializeField] private Sprite[] m_WeaponSprites;

    // --------------------------------------------------------------

    private Animator m_ImageAnimator;

    private Animator m_PlusAnimator;

    private Animator m_PiAnimator;

    private Animator m_HealthAnimator;

    private Color m_ImageColour;

    private bool m_WeaponTimerActive = false;

    private bool m_LightningSprintActive = false;

    // --------------------------------------------------------------

    private void Start()
    {
        m_ImageAnimator = m_WeaponImage.GetComponent<Animator>();
        m_PlusAnimator = m_NumPlusesText.GetComponent<Animator>();
        m_PiAnimator = m_NumPisText.GetComponent<Animator>();
        m_HealthAnimator = m_HealthText.GetComponent<Animator>();

        m_ImageColour = m_WeaponImage.color;
        UpdateAll();
    }

    private void Update()
    {
        if (m_WeaponTimerActive)
        {
            m_TimerImage.fillAmount = m_PlayerStats.BoxingTimeRemaining / GameManager.POWERUP_DURATION;
        }

        if (m_LightningSprintActive)
        {
            m_StaminaBar.fillAmount = m_PlayerStats.SprintTimeRemaining / m_PlayerStats.MaxSprintTime;
        }
    }

    public void UpdateAll()
    {
        UpdatePis(false);
        UpdatePluses(false);
        UpdateHealthDisplay(false, false);
    }

    public void UpdatePis(bool withAnimation)
    {
        m_NumPisText.text = m_PlayerStats.NumPis.ToString();
        if (withAnimation)
        {
            m_PiAnimator.SetTrigger("flashTrigger");
        }
    }

    public void UpdatePluses(bool withAnimation)
    {
        m_NumPlusesText.text = m_PlayerStats.NumPluses.ToString();
        if (withAnimation)
        {
            m_PlusAnimator.SetTrigger("flashTrigger");
        }
    }

    public void UpdateHealthDisplay(bool withAnimation, bool tookDamage)
    {
        m_HealthText.text = m_PlayerStats.Health.ToString();
        if (withAnimation)
        {
            string animationTrigger = tookDamage ? "damageTrigger" : "extraLifeTrigger";
            m_HealthAnimator.SetTrigger(animationTrigger);
        }
    }

    public void ShowWeapon(Weapon type, bool withAnimation)
    {
        m_WeaponImage.color = m_ImageColour;
        m_WeaponImage.sprite = m_WeaponSprites[(int)type];
        m_WeaponImage.enabled = true;
        if (withAnimation)
        {
            m_ImageAnimator.SetBool("isActive", true);
        }
    }

    public void ActivateWeapon(Weapon type)
    {
        switch (type)
        {
            case Weapon.BOMB:
                HideWeapon();
                break;
            case Weapon.GUN:
                m_AmmoText.enabled = true;
                m_AmmoText.text = m_PlayerStats.RemainingAmmo.ToString();
                break;
            case Weapon.BOXING_GLOVES:
                m_TimerImage.enabled = true;
                m_TimerImage.fillAmount = 1f;
                m_WeaponTimerActive = true;
                break;
        }
        Color fadedColour = m_ImageColour;
        fadedColour.a = 0.5f;
        m_WeaponImage.color = fadedColour;

    }

    public void HideWeapon()
    {
        m_AmmoText.enabled = false;
        m_ImageAnimator.SetBool("isActive", false);
        m_WeaponImage.enabled = false;
        m_TimerImage.enabled = false;
        m_WeaponTimerActive = false;
    }

    public void UpdateAmmoDisplay()
    {
        m_AmmoText.text = m_PlayerStats.RemainingAmmo.ToString();
    }

    public void ActivateLightningSprint()
    {
        m_StaminaBar.enabled = true;
        m_StaminaBar.fillAmount = 1f;
        m_LightningIcon.enabled = true;
        m_LightningSprintActive = true;
    }

    public void HidePowerup()
    {
        m_StaminaBar.enabled = false;
        m_LightningIcon.enabled = false;
        m_LightningSprintActive = false;
    }
}
