using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class WaypointPatroller : MonoBehaviour
{
    // --------------------------------------------------------------

    // How long agent waits between checking if Player spotted
    [SerializeField] private float m_TimeBetweenPlayerSearches = 1f;

    // Item to spawn when enemy knocked out
    [SerializeField] private GameObject m_DropItemPrefab;

    // --------------------------------------------------------------

    private NavMeshAgent m_NavMeshAgent;

    private Waypoint m_CurrentWaypoint;

    private Waypoint m_LastVisitedWaypoint;

    private bool m_PlayerInSight = false;

    private PlayerController[] m_Players;

    private PlayerController m_LastSeenPlayer;

    private Camera m_Camera;

    private EnemyBehaviour m_EnemyBehaviour;

    // --------------------------------------------------------------

    public bool PlayerInSight
    {
        get
        {
            return m_PlayerInSight;
        }
    }

    public Vector3 PlayerPos
    {
        get
        {
            return m_LastSeenPlayer.transform.position;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_EnemyBehaviour = GetComponent<EnemyBehaviour>();

        m_Camera = GetComponentInChildren<Camera>();
        m_Players = FindObjectsOfType<PlayerController>();

        FindStartingWaypoint();
        StartCoroutine(LookForPlayer());
    }

    private void FindStartingWaypoint()
    {
        float shortestDistance = float.MaxValue;

        foreach (Waypoint waypoint in FindObjectsOfType<Waypoint>())
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, waypoint.transform.position);
            if (distanceToWaypoint < shortestDistance)
            {
                shortestDistance = distanceToWaypoint;
                m_CurrentWaypoint = waypoint;
            }
        }
        if (m_CurrentWaypoint == null)
        {
            Debug.LogError("Failed to find a starting Waypoint for " + name);
        }
    }

    private void Update()
    {
        if (m_PlayerInSight)
        {
            m_EnemyBehaviour.Execute();
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

    private IEnumerator LookForPlayer()
    {
        Plane[] planesInView = GeometryUtility.CalculateFrustumPlanes(m_Camera);
        bool spottedPlayer = false;
        foreach (PlayerController player in m_Players)
        {
            if (GeometryUtility.TestPlanesAABB(planesInView, player.GetComponent<Collider>().bounds))
            {
                Debug.Log("Player " + player.PlayerNum + " spotted by " + name + "!");
                m_LastSeenPlayer = player;
                spottedPlayer = true;
            }
        }
        m_PlayerInSight = spottedPlayer;

        yield return new WaitForSeconds(m_TimeBetweenPlayerSearches);
        StartCoroutine(LookForPlayer());
    }

    public void Die()
    {
        Instantiate(m_DropItemPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
