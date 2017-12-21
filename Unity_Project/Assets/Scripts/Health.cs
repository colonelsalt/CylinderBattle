﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    // --------------------------------------------------------------

    [SerializeField]
    private int m_StartHealth = 1;
    
    // After being damaged, how long Player will be invincible
    [SerializeField]
    private float m_InvincibilityTime = 2.5f;

    [SerializeField]
    private bool m_IsInvincible = false;

    // --------------------------------------------------------------

    // Events
    public delegate void PlayerHealthEvent(int playerNum, int newHealth);
    public static event PlayerHealthEvent OnPlayerHealthChange;

    // --------------------------------------------------------------

    private PlayerController m_Player;
    private Animator m_Animator;
    private int m_CurrentHealth;
    
    // --------------------------------------------------------------

    private void Awake()
    {
        m_CurrentHealth = m_StartHealth;
        m_Player = GetComponent<PlayerController>();
        m_Animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (m_IsInvincible) return;

        m_CurrentHealth -= damage;

        if (m_Player != null)
        {
            OnPlayerHealthChange(m_Player.GetPlayerNum(), m_CurrentHealth);
        }

        if (m_CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            m_IsInvincible = true;
            StartCoroutine(InvincibilityFlash());
            //m_Animator.SetTrigger("InvincibilityTrigger");
            //Invoke("DisableInvincibility", m_InvincibilityTime);
        }
    }

    private IEnumerator InvincibilityFlash()
    {
        Renderer rend = GetComponent<Renderer>();
        bool visible = false;
        for (float i = 0; i < m_InvincibilityTime; i += 0.10f)
        {
            rend.enabled = visible;
            visible = !visible;
            yield return new WaitForSeconds(0.10f);
        }
        rend.enabled = true;
        DisableInvincibility();
    }

    private void DisableInvincibility()
    {
        m_IsInvincible = false;
    }

    public void Die()
    {
        if (m_Player != null)
        {
            m_Player.Die();
            m_CurrentHealth = GameManager.PLAYER_HEALTH;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
