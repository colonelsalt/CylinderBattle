using System;
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
    private int m_CurrentHealth;
    
    // --------------------------------------------------------------

    private void Awake()
    {
        m_CurrentHealth = m_StartHealth;
        m_Player = GetComponent<PlayerController>();
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
            // TODO: Trigger invincibility animation
            Invoke("DisableInvincibility", m_InvincibilityTime);
        }
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
