using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    // --------------------------------------------------------------

    [SerializeField] int m_MaxHealth = 1;

    // --------------------------------------------------------------

    // Events
    public delegate void PlayerHealthEvent(int playerNum, int healthChange);
    public static event PlayerHealthEvent OnPlayerHealthChange;

    // --------------------------------------------------------------

    private PlayerController m_Player;
    private int m_CurrentHealth;
    
    // --------------------------------------------------------------

    private void Awake()
    {
        m_CurrentHealth = m_MaxHealth;
        m_Player = GetComponent<PlayerController>();
    }

    public void TakeDamage(int damage)
    {
        m_CurrentHealth -= damage;
        if (m_Player != null)
        {
            OnPlayerHealthChange(m_Player.GetPlayerNum(), -damage);
        }
        if (m_CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
