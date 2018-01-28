using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : MonoBehaviour, IHealth
{
    // --------------------------------------------------------------

    [SerializeField] private int m_StartHealth = 3;

    // After being damaged, how long Player will be invincible
    [SerializeField] private float m_InvincibilityTime = 2.5f;

    [SerializeField] private GameObject m_DeathExplosionEffect;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_DamageSounds;

    [SerializeField] private AudioClip[] m_DeathSounds;

    [SerializeField] private AudioClip[] m_RespawnSounds;

    // --------------------------------------------------------------

    // Events
    public delegate void PlayerHealthIncreaseEvent(int playerNum);
    public static event PlayerHealthIncreaseEvent OnPlayerExtraLife;
    public static event PlayerHealthIncreaseEvent OnPlayerRespawn;

    public delegate void PlayerDamagedEvent(int playerNum, GameObject attacker);
    public static event PlayerDamagedEvent OnPlayerDamaged;
    public static event PlayerDamagedEvent OnPlayerDeath;

    // --------------------------------------------------------------

    // Ref. to PlayerController to temporarily remove control on death
    private PlayerController m_PlayerController;

    private Animator m_PlayerAnim;

    private Renderer[] m_Renderers;

    private Vector3 m_SpawningPosition;

    private int m_PlayerNum;

    private int m_CurrentHealth;

    private bool m_IsInvincible = false;

    private bool m_IsAlive = true;

    // Time it takes to respawn
    private const float MAX_RESPAWN_TIME = 3.0f;
    private float m_RespawnTime = MAX_RESPAWN_TIME;

    // --------------------------------------------------------------

    public int Health
    {
        get
        {
            return m_CurrentHealth;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        m_PlayerNum = GetComponent<IPlayer>().PlayerNum();
        m_PlayerController = GetComponent<PlayerController>();
        m_PlayerAnim = GetComponentInChildren<Animator>();
        m_Renderers = GetComponentsInChildren<Renderer>();

        m_CurrentHealth = m_StartHealth;
        m_SpawningPosition = transform.position;
    }

    public bool IsAlive()
    {
        return m_IsAlive;
    }

    public void GetExtraLife()
    {
        m_CurrentHealth++;
        OnPlayerExtraLife(m_PlayerNum);
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        if (m_IsInvincible || !m_IsAlive) return;

        SoundManager.Instance.PlayRandom(m_DamageSounds);

        m_CurrentHealth = Mathf.Max(0, m_CurrentHealth - damage); // Don't allow health to fall below zero
        OnPlayerDamaged(m_PlayerNum, attacker);
        if (m_CurrentHealth <= 0)
        {
            Die(attacker);
        }
        else if (m_IsAlive)
        {
            m_IsInvincible = true;
            StartCoroutine(InvincibilityFlash());
        }
    }

    // Flashes Player's Renderer to indicate invincibility immediately following damage
    private IEnumerator InvincibilityFlash()
    {
        Renderer rend = m_Renderers[0];
        for (float i = 0; i < m_InvincibilityTime; i += 0.10f)
        {
            rend.enabled = !rend.enabled;
            yield return new WaitForSeconds(0.10f);
        }
        rend.enabled = true;
        m_IsInvincible = false;
    }

    // Lerp movement back to spawn position, then respawn
    private IEnumerator MoveToSpawnPos()
    {
        SetVisibility(false);
        yield return new WaitForSeconds(1.5f);
        SetVisibility(false);
        transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

        Vector3 startPosition = transform.position;
        float distanceToTarget = Vector3.Distance(startPosition, m_SpawningPosition);
        float startTime = Time.time;

        // Adjust speed to ensure it covers necessary distance in desired respawn time
        float speed = Vector3.Distance(transform.position, m_SpawningPosition) / (m_RespawnTime - 1f);
        float distanceCovered = 0f;

        while (distanceToTarget - distanceCovered > 0.1f)
        {
            distanceCovered = (Time.time - startTime) * speed;
            transform.position = Vector3.Lerp(startPosition, m_SpawningPosition, distanceCovered / distanceToTarget);
            yield return new WaitForEndOfFrame();
        }

        SoundManager.Instance.PlayRandom(m_RespawnSounds);
        yield return new WaitForSeconds(0.2f);
        m_PlayerAnim.SetTrigger("RespawnTrigger");

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
        m_CurrentHealth = m_StartHealth;
        m_IsAlive = true;
        m_PlayerController.enabled = true;

        OnPlayerRespawn(m_PlayerNum);
    }

    public void Die(GameObject killer)
    {
        // Prevent multiple calls before Player has respawned
        if (!m_IsAlive) return;

        SoundManager.Instance.PlayRandom(m_DeathSounds);
        m_CurrentHealth = 0;

        Instantiate(m_DeathExplosionEffect, transform.position, Quaternion.identity);

        m_IsAlive = false;
        m_PlayerController.enabled = false;
        m_RespawnTime = MAX_RESPAWN_TIME;

        OnPlayerDeath(m_PlayerNum, killer);
        BroadcastMessage("OnDeath");

        StopAllCoroutines();
        m_IsInvincible = false;
        StartCoroutine(MoveToSpawnPos());
    }
}
