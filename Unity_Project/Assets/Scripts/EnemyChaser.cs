using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaser : MonoBehaviour, IEnemyBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_ChaseSpeed = 20f;

    // --------------------------------------------------------------

    private NavMeshAgent m_NavMeshAgent;

    private WaypointPatroller m_Patrol;

    private bool m_TouchingPlayer = false;

    private Collider[] m_Colliders;

    // --------------------------------------------------------------


    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Patrol = GetComponent<WaypointPatroller>();
        m_Colliders = GetComponents<Collider>();
    }

    public void Execute()
    {
        if (!m_TouchingPlayer)
        {
            // Follow Player around once spotted
            m_NavMeshAgent.speed = m_ChaseSpeed;
            m_NavMeshAgent.destination = m_Patrol.PlayerPos;
        }
        else
        {
            m_NavMeshAgent.destination = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null && !m_TouchingPlayer)
        {
            m_TouchingPlayer = true;

            // Unless Player is above us (presumably jumping on our head), deal damage
            if (other.bounds.min.y < m_Colliders[0].bounds.center.y)
            {
                player.TakeDamage(1);
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

    private void BreakToPieces()
    {
        foreach (Collider collider in m_Colliders)
        {
            collider.enabled = false;
        }

        PhysicsSwitch physicsSwitch = GetComponent<PhysicsSwitch>();
        if (physicsSwitch != null)
        {
            physicsSwitch.ActivatePhysicsReactions(false);
        }

        foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
        {
            body.isKinematic = false;
            FadeOut fadeOut = body.GetComponent<FadeOut>();
            if (fadeOut != null)
            {
                fadeOut.Begin();
            }
        }
    }

}
