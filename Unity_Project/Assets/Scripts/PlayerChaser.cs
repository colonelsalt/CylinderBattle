using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerChaser : EnemyBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_ChaseSpeed = 20f;

    // --------------------------------------------------------------

    private NavMeshAgent m_NavMeshAgent;

    private WaypointPatroller m_Patrol;

    // --------------------------------------------------------------


    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Patrol = GetComponent<WaypointPatroller>();
    }

    public override void Execute()
    {
        m_NavMeshAgent.speed = m_ChaseSpeed;
        m_NavMeshAgent.destination = m_Patrol.PlayerPos;  
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(1);
        }
    }
}
