using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaser : EnemyBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_ChaseSpeed = 20f;

    // --------------------------------------------------------------

    private NavMeshAgent m_NavMeshAgent;

    private WaypointPatroller m_Patrol;

    private bool m_TouchingPlayer = false;

    // --------------------------------------------------------------


    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Patrol = GetComponent<WaypointPatroller>();
    }

    public override void Execute()
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

            player.TakeDamage(1);
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
        PhysicsSwitch physicsSwitch = GetComponent<PhysicsSwitch>();
        if (physicsSwitch != null)
        {
            physicsSwitch.ActivatePhysicsReactions(false);
        }

        foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
        {
            body.isKinematic = false;
        }
    }

}
