﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    // --------------------------------------------------------------

    [SerializeField] private int m_StartHealth = 1;

    // Item to spawn when Object knocked out
    [SerializeField] private GameObject m_DropItemPrefab;

    // --------------------------------------------------------------

    private int m_CurrentHealth;

    private Animator m_Animator;

    private Collider m_Collider;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Collider = GetComponentInChildren<Collider>();
        m_Animator = GetComponent<Animator>();
        m_CurrentHealth = m_StartHealth;
    }

    public void TakeDamage(int damage)
    {
        m_CurrentHealth -= damage;
        if (m_CurrentHealth <= 0)
        {
            m_CurrentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        // Disable collider on death
        m_Collider.enabled = false;

        // Instantiate drop item
        if (m_DropItemPrefab != null)
        {
            Instantiate(m_DropItemPrefab, transform.position, Quaternion.identity);
        }

        BroadcastMessage("OnDeath");
        m_Animator.SetTrigger("deathTrigger");

        Destroy(gameObject, 2f);
    }

}
