using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private int m_StartHealth = 3;

    // --------------------------------------------------------------

    private int m_CurrentHealth = 3;

    private Collider m_Collider;

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
        m_Collider = GetComponentInChildren<Collider>();
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
        // Disable collider on death
        m_Collider.enabled = false;

        Destroy(gameObject);
    }

}
