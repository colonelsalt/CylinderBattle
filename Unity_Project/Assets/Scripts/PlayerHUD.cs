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

    [SerializeField] private Image m_TimerImage;

    [SerializeField] private Sprite[] m_WeaponSprites;

    // --------------------------------------------------------------

    private ScoreKeeper m_Score;

    private Animator m_ImageAnimator;

    private Animator m_PlusAnimator;

    private Animator m_PiAnimator;

    private Animator m_HealthAnimator;

    private Color m_ImageColour;

    private bool m_TimerActive = false;

    // --------------------------------------------------------------

    private void Start()
    {
        m_Score = GetComponent<ScoreKeeper>();
        m_ImageAnimator = m_WeaponImage.GetComponent<Animator>();
        m_PlusAnimator = m_NumPlusesText.GetComponent<Animator>();
        m_PiAnimator = m_NumPisText.GetComponent<Animator>();
        m_HealthAnimator = m_HealthText.GetComponent<Animator>();

        m_ImageColour = m_WeaponImage.color;
        UpdateAll();
    }

    private void Update()
    {
        if (m_TimerActive)
        {
            m_TimerImage.fillAmount = m_Score.BoxingTimeRemaining / GameManager.POWERUP_DURATION;
        }
    }

    public void UpdateAll()
    {
        UpdatePis(false);
        UpdatePluses(false);
        UpdateHealthDisplay(false);
    }

    public void UpdatePis(bool withAnimation)
    {
        m_NumPisText.text = m_Score.NumPis.ToString();
        if (withAnimation)
        {
            m_PiAnimator.SetTrigger("flashTrigger");
        }
    }

    public void UpdatePluses(bool withAnimation)
    {
        m_NumPlusesText.text = m_Score.NumPluses.ToString();
        if (withAnimation)
        {
            m_PlusAnimator.SetTrigger("flashTrigger");
        }
    }

    public void UpdateHealthDisplay(bool withAnimation)
    {
        int oldHealth = int.Parse(m_HealthText.text);
        m_HealthText.text = m_Score.Health.ToString();
        if (withAnimation)
        {
            string animationTrigger = (oldHealth > m_Score.Health) ? "extraLifeTrigger" : "damageTrigger";
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
                m_AmmoText.text = m_Score.RemainingAmmo.ToString();
                break;
            case Weapon.BOXING_GLOVES:
                m_TimerImage.enabled = true;
                m_TimerImage.fillAmount = 1f;
                m_TimerActive = true;
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
        m_TimerActive = false;
    }

    public void UpdateAmmoDisplay()
    {
        m_AmmoText.text = m_Score.RemainingAmmo.ToString();
    }
}
