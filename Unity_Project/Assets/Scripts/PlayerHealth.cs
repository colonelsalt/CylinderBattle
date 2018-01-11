using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : Health
{
    // --------------------------------------------------------------
    
    // After being damaged, how long Player will be invincible
    [SerializeField] private float m_InvincibilityTime = 2.5f;

    [SerializeField] private Vector3 m_SpawningPosition;

    [SerializeField] private GameObject m_DeathExplosionEffect;

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

    private Renderer[] m_Renderers;

    private bool m_IsInvincible = false;

    private bool m_IsAlive = true;

    // Time it takes to respawn
    private const float MAX_RESPAWN_TIME = 3.0f;
    private float m_RespawnTime = MAX_RESPAWN_TIME;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Player = GetComponent<PlayerController>();
        m_Animator = GetComponentInChildren<Animator>();
        m_Renderers = GetComponentsInChildren<Renderer>();
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

    private IEnumerator MoveToSpawnPos()
    {
        yield return new WaitForSeconds(1f);

        Vector3 startPosition = transform.position;
        float distanceToTarget = Vector3.Distance(startPosition, m_SpawningPosition);
        float startTime = Time.time;
        float speed = Vector3.Distance(transform.position, m_SpawningPosition) / (m_RespawnTime - 1f);
        float distanceCovered = 0f;

        while (distanceToTarget - distanceCovered > 0.1f)
        {
            distanceCovered = (Time.time - startTime) * speed;
            transform.position = Vector3.Lerp(startPosition, m_SpawningPosition, distanceCovered / distanceToTarget);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

        m_Animator.SetTrigger("RespawnTrigger");
        yield return new WaitForSeconds(0.2f);
        SetVisibility(true);
        yield return new WaitForSeconds(0.45f);
        Respawn();
    }

    private void SetVisibility(bool visible)
    {
        foreach (Renderer rend in m_Renderers)
        {
            rend.enabled = visible;
        }
    }

    private void Respawn()
    {
        ResetHealth();
        m_IsAlive = true;
        m_Player.enabled = true;

        OnPlayerRespawn(m_Player.PlayerNum);
    }

    public override void Die()
    {
        Instantiate(m_DeathExplosionEffect, transform.position, Quaternion.identity);
        SetVisibility(false);

        m_IsAlive = false;
        m_Player.enabled = false;
        m_RespawnTime = MAX_RESPAWN_TIME;

        OnPlayerDeath(m_Player.PlayerNum);
        StartCoroutine(MoveToSpawnPos());
    }
}
