using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    // --------------------------------------------------------------

    [SerializeField] private int m_StartHealth = 1;

    // Item to spawn when Enemy knocked out
    [SerializeField] private GameObject m_DropItemPrefab;

    // --------------------------------------------------------------

    // Events
    public delegate void EnemyDamageEvent(GameObject attacker);
    public static event EnemyDamageEvent OnEnemyDeath;

    // --------------------------------------------------------------

    // Sounds
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

    public void TakeDamage(int damage, GameObject attacker)
    {
        m_CurrentHealth -= damage;
        if (m_CurrentHealth <= 0)
        {
            m_CurrentHealth = 0;
            Die(attacker);
        }
        else
        {
            SoundManager.Instance.PlayRandom(m_DamageSounds);
        }
    }

    public void Die(GameObject killer)
    {
        SoundManager.Instance.PlayRandom(m_DeathSounds);

        OnEnemyDeath(killer);

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
