using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private int m_StartHealth = 1;
    
    // After being damaged, how long object will be invincible
    [SerializeField] private float m_InvincibilityTime = 2.5f;

    // --------------------------------------------------------------

    // Events
    public delegate void PlayerHealthEvent(int playerNum, int healthChange);
    public static event PlayerHealthEvent OnPlayerHealthChange;

    // --------------------------------------------------------------

    // If this object is a PlayerController, keep a reference to it
    private PlayerController m_Player;

    private Animator m_Animator;

    private int m_CurrentHealth;

    private bool m_IsInvincible = false;

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
            OnPlayerHealthChange(m_Player.PlayerNum, -damage);
        }

        if (m_CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            m_IsInvincible = true;
            StartCoroutine(InvincibilityFlash());
        }
    }

    private IEnumerator InvincibilityFlash()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        for (float i = 0; i < m_InvincibilityTime; i += 0.10f)
        {
            rend.enabled = !rend.enabled;
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
