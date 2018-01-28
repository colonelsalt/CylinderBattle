using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
// Chases Player when spotted
public class EnemyChaser : MonoBehaviour, IEnemyBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_ChaseSpeed = 20f;

    // --------------------------------------------------------------

    private NavMeshAgent m_NavMeshAgent;

    // Default speed as set in NavMeshAgent
    private float m_WalkSpeed;

    private WaypointPatroller m_Patrol;

    private AudioSource m_Audio;

    private bool m_TouchingPlayer = false;

    private Collider[] m_Colliders;

    private bool m_PlayingSound = false;

    // --------------------------------------------------------------


    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Patrol = GetComponent<WaypointPatroller>();
        m_Colliders = GetComponents<Collider>();
        m_Audio = GetComponent<AudioSource>();

        // Remember default speed to reset after Player chase finished
        m_WalkSpeed = m_NavMeshAgent.speed;
    }

    public void Execute()
    {
        if (!m_PlayingSound)
        {
            m_Audio.Play();
            m_PlayingSound = true;
        }

        if (!m_TouchingPlayer)
        {
            // Follow Player around once spotted
            m_NavMeshAgent.speed = m_ChaseSpeed;
            m_NavMeshAgent.destination = m_Patrol.PlayerPos;
        }
        else
        {
            // Avoid walking into Player
            m_Patrol.StandStill();
        }
    }

    // Return speed to normal and fade out audio
    public void Disable()
    {
        m_NavMeshAgent.speed = m_WalkSpeed;
        if (m_PlayingSound)
        {
            SoundManager.Instance.FadeOut(m_Audio);
            m_PlayingSound = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            m_TouchingPlayer = true;

            // Unless Player is above us (presumably jumping on our head), deal damage
            if (other.bounds.min.y < m_Colliders[0].bounds.center.y)
            {
                player.TakeDamage(1, gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }

    private void OnTriggerExit(Collider other)
    {
        m_TouchingPlayer = false;
    }

    // Broadcast from EnemyHealth
    private void OnDeath()
    {
        // Disable Colliders to prevent Enemy dealing damage
        foreach (Collider col in m_Colliders)
        {
            col.enabled = false;
        }

        // Fall to pieces
        foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
        {
            body.isKinematic = false;
        }
    }
}
