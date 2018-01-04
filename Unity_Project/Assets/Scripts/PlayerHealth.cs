using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : Health
{
    // --------------------------------------------------------------
    
    // After being damaged, how long object will be invincible
    [SerializeField] private float m_InvincibilityTime = 2.5f;

    [SerializeField] private Vector3 m_SpawningPosition;

    // --------------------------------------------------------------

    // Events
    public delegate void PlayerHealthEvent(int playerNum);
    public static event PlayerHealthEvent OnPlayerDamaged;
    public static event PlayerHealthEvent OnPlayerExtraLife;
    public static event PlayerHealthEvent OnPlayerDeath;
    public static event PlayerHealthEvent OnPlayerRespawn;

    // --------------------------------------------------------------

    private PlayerController m_Player;

    private Animator m_Animator;

    private bool m_IsInvincible = false;

    private bool m_IsAlive = true;

    // time it takes to respawn
    private const float MAX_RESPAWN_TIME = 1.0f;
    private float m_RespawnTime = MAX_RESPAWN_TIME;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Player = GetComponent<PlayerController>();
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // If Player is dead, update respawn timer
        if (!m_IsAlive)
        {
            UpdateRespawnTime();
        }
    }

    public override void GetExtraLife()
    {
        base.GetExtraLife();
        OnPlayerExtraLife(m_Player.PlayerNum);
    }

    public override void TakeDamage(int damage)
    {
        if (m_IsInvincible) return;

        base.TakeDamage(damage);

        OnPlayerDamaged(m_Player.PlayerNum);

        if (m_IsAlive)
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
        ResetHealth();
        m_IsAlive = true;

        GetComponentInChildren<Renderer>().enabled = true; // TEMPORARY!!!

        transform.position = m_SpawningPosition;
        transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        OnPlayerRespawn(m_Player.PlayerNum);
    }

    public override void Die()
    {
        m_IsAlive = false;
        m_RespawnTime = MAX_RESPAWN_TIME;

        GetComponentInChildren<Renderer>().enabled = false; // TEMPORARY!!!

        // TODO: Trigger death animation

        OnPlayerDeath(m_Player.PlayerNum);
    }
}
