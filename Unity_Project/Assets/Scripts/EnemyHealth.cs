using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    // --------------------------------------------------------------

    [SerializeField] private int m_StartHealth = 1;

    // Item to spawn when Object knocked out
    [SerializeField] private GameObject m_DropItemPrefab;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_DamageSounds;

    [SerializeField] private AudioClip[] m_DeathSounds;

    // --------------------------------------------------------------

    private int m_CurrentHealth;

    private Animator m_Animator;

    // --------------------------------------------------------------

    private void Awake()
    {
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
        else
        {
            SoundManager.Instance.PlayRandom(m_DamageSounds);
        }
    }

    public void Die()
    {
        SoundManager.Instance.PlayRandom(m_DeathSounds);

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
