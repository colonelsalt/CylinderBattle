using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    // --------------------------------------------------------------

    // How long agent waits between checking for a new destination
    [SerializeField] private float m_TimeBetweenDestinationUpdates = 1f;

    [SerializeField] private Waypoint m_StartingWaypoint;

    // --------------------------------------------------------------

    private NavMeshAgent m_NavMeshAgent;

    private Waypoint m_CurrentWaypoint;

    private Waypoint m_LastVisitedWaypoint;

    private PlayerController[] m_Players;

    private PlayerController m_LastSeenPlayer;

    private Camera m_Camera;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Camera = GetComponentInChildren<Camera>();
        m_Players = FindObjectsOfType<PlayerController>();

        m_CurrentWaypoint = m_StartingWaypoint;
    }

    private void Update()
    {
        if (IsPlayerInSight())
        {
            m_NavMeshAgent.destination = m_LastSeenPlayer.transform.position;
        }
        else if (m_NavMeshAgent.remainingDistance <= 0.5f)
        {
            Waypoint nextWayPoint = m_CurrentWaypoint.GetNextWaypoint(m_LastVisitedWaypoint);
            m_LastVisitedWaypoint = m_CurrentWaypoint;

            m_NavMeshAgent.destination = nextWayPoint.transform.position;

            m_CurrentWaypoint = nextWayPoint;
        }
        else
        {
            m_NavMeshAgent.destination = m_CurrentWaypoint.transform.position;
        }
    }

    private bool IsPlayerInSight()
    {
        Plane[] planesInView = GeometryUtility.CalculateFrustumPlanes(m_Camera);
        foreach (PlayerController player in m_Players)
        {
            if (GeometryUtility.TestPlanesAABB(planesInView, player.GetComponent<Collider>().bounds))
            {
                Debug.Log("Player" + player.PlayerNum + " spotted by " + name + "!");
                m_LastSeenPlayer = player;
                return true;
            }
        }
        return false;
    }

}
