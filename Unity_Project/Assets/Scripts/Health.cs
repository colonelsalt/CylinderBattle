using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private int m_StartHealth = 3;

    // --------------------------------------------------------------

    private int m_CurrentHealth = 3;

    // --------------------------------------------------------------

    public int RemainingHealth
    {
        get
        {
            return m_CurrentHealth;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        m_CurrentHealth = m_StartHealth;
    }

    public virtual void GetExtraLife()
    {
        m_CurrentHealth++;
    }

    public virtual void TakeDamage(int damage)
    {
        m_CurrentHealth -= damage;
        if (m_CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

}
