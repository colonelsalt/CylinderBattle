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

    // --------------------------------------------------------------

    private NavMeshAgent m_NavMeshAgent;

    // Waypoint Enemy is currently walking towards
    private Waypoint m_CurrentWaypoint;

    // Waypoint Enemy last visited
    private Waypoint m_LastVisitedWaypoint;

    // Default speed as set in NavMeshAgent (might be modified by EnemyBehaviour later)
    private float m_WalkSpeed;

    // Whether Enemy can currently see Player
    private bool m_PlayerInSight = false;

    // Reference to each Player in scene
    private PlayerController[] m_Players;

    private PlayerController m_LastSeenPlayer;

    private Camera m_Camera;

    // Behaviour to execute once Enemy spots Player
    private IEnemyBehaviour m_EnemyBehaviour;

    private ParticleSystem m_Exclamation;

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
        m_EnemyBehaviour = GetComponent<IEnemyBehaviour>();

        m_Camera = GetComponentInChildren<Camera>();
        m_Exclamation = GetComponentInChildren<ParticleSystem>();
        m_Players = FindObjectsOfType<PlayerController>();

        m_WalkSpeed = m_NavMeshAgent.speed;

        FindStartingWaypoint();
        StartCoroutine(LookForPlayer());
    }

    // Determine which Waypoint in scene is closest, and go to that one first
    private void FindStartingWaypoint()
    {
        float shortestDistance = float.MaxValue;

        // Find all waypoints belonging to this Enemy
        foreach (GameObject waypoint in GameObject.FindGameObjectsWithTag(tag + "Waypoint"))
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, waypoint.transform.position);
            if (distanceToWaypoint < shortestDistance)
            {
                shortestDistance = distanceToWaypoint;
                m_CurrentWaypoint = waypoint.GetComponent<Waypoint>();
            }
        }
        if (m_CurrentWaypoint == null)
        {
            Debug.LogError("Failed to find a starting Waypoint for " + name);
        }
    }

    private void Update()
    {
        if (!m_NavMeshAgent.enabled) return;

        if (m_PlayerInSight)
        {
            m_EnemyBehaviour.Execute();
        }
        else if (m_NavMeshAgent.remainingDistance <= 0.5f)
        {
            // If we've reached our destination Waypoint, find the next one
            m_NavMeshAgent.speed = m_WalkSpeed;

            Waypoint nextWayPoint = m_CurrentWaypoint.GetNextWaypoint(m_LastVisitedWaypoint);
            m_LastVisitedWaypoint = m_CurrentWaypoint;

            m_NavMeshAgent.destination = nextWayPoint.transform.position;

            m_CurrentWaypoint = nextWayPoint;
        }
        else
        {
            m_NavMeshAgent.speed = m_WalkSpeed;
            m_NavMeshAgent.destination = m_CurrentWaypoint.transform.position;
        }
    }

    // Check if Player is within view frustum of Enemy's camera
    private IEnumerator LookForPlayer()
    {
        Plane[] planesInView = GeometryUtility.CalculateFrustumPlanes(m_Camera);
        bool spottedPlayer = false;
        foreach (PlayerController player in m_Players)
        {
            if (GeometryUtility.TestPlanesAABB(planesInView, player.GetComponent<Collider>().bounds))
            {
                m_LastSeenPlayer = player;
                spottedPlayer = true;
                break;
            }
        }
        m_PlayerInSight = spottedPlayer;
        if (spottedPlayer)
        {
            ShowExclamation();
        }

        // Only update occasionally to prevent performance hit (and to make Enemy a little dumber)
        yield return new WaitForSeconds(m_TimeBetweenPlayerSearches);
        StartCoroutine(LookForPlayer());
    }

    public void StandStill()
    {
        m_NavMeshAgent.destination = transform.position;
    }

    private void ShowExclamation()
    {
        m_Exclamation.Play();
    }

    // Handler for Health component's 'OnDeath' message
    private void OnDeath()
    {
        m_NavMeshAgent.enabled = false;
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        OnDeath();
    }

}
