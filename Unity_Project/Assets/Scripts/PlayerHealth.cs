using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private int m_StartHealth = 1;
    
    // After being damaged, how long object will be invincible
    [SerializeField] private float m_InvincibilityTime = 2.5f;

    [SerializeField] private Vector3 m_SpawningPosition;

    // --------------------------------------------------------------

    // Events
    public delegate void PlayerHealthEvent(int playerNum, int healthChange);
    public static event PlayerHealthEvent OnPlayerHealthChange;
    public static event PlayerHealthEvent OnPlayerDeath;
    public static event PlayerHealthEvent OnPlayerRespawn;

    // --------------------------------------------------------------

    private PlayerController m_Player;

    private Animator m_Animator;

    private int m_CurrentHealth;

    private bool m_IsInvincible = false;

    private bool m_IsAlive = true;

    // The time it takes to respawn
    private const float MAX_RESPAWN_TIME = 1.0f;
    private float m_RespawnTime = MAX_RESPAWN_TIME;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_CurrentHealth = m_StartHealth;
        m_Player = GetComponent<PlayerController>();
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // If the player is dead update the respawn timer and exit update loop
        if (!m_IsAlive)
        {
            UpdateRespawnTime();
        }
    }

    public void GetExtraLife()
    {
        m_CurrentHealth++;
        OnPlayerHealthChange(m_Player.PlayerNum, 1);
    }

    public void TakeDamage(int damage)
    {
        if (m_IsInvincible) return;

        m_CurrentHealth -= damage;

        OnPlayerHealthChange(m_Player.PlayerNum, -damage);

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
        m_IsInvincible = false;
    }

    private void UpdateRespawnTime()
    {
        m_RespawnTime -= Time.deltaTime;
        if (m_RespawnTime < 0.0f)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        m_Player.DeactivatePhysicsReactions();
        GetComponentInChildren<Renderer>().enabled = true; // TEMPORARY!!!

        m_IsAlive = true;
        transform.position = m_SpawningPosition;
        transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        OnPlayerRespawn(m_Player.PlayerNum, 0);
    }

    public void Die()
    {
        m_CurrentHealth = GameManager.PLAYER_HEALTH;

        m_IsAlive = false;
        m_RespawnTime = MAX_RESPAWN_TIME;

        GetComponentInChildren<Renderer>().enabled = false; // TEMPORARY!!!

        // TODO: Trigger death animation

        OnPlayerDeath(m_Player.PlayerNum, -1);
    }
}
