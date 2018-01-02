using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    // --------------------------------------------------------------

    [SerializeField] private float m_ConnectivityRadius = 10f;

    // --------------------------------------------------------------

    private List<Waypoint> m_WaypointsInRange;

    private bool m_ChasingPlayer = false;

    // --------------------------------------------------------------

    private void OnDrawGizmos()
    {
        // Visualise reach of this waypoint
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_ConnectivityRadius);
    }

    private void Awake()
    {
        m_WaypointsInRange = new List<Waypoint>();

        foreach (Waypoint waypoint in FindObjectsOfType<Waypoint>())
        {
            // If this waypoint is within range, add it to list of reachable waypoints
            if ((Vector3.Distance(transform.position, waypoint.transform.position) <= 2 * m_ConnectivityRadius) && waypoint != this)
            {
                m_WaypointsInRange.Add(waypoint);
            }
        }

        if (m_WaypointsInRange.Count <= 0)
        {
            Debug.LogError("No Waypoints in range of " + name + " found!");
        }
    }

    // Randomly selects next Waypoint from our list of waypoints reachable from here
    public Waypoint GetNextWaypoint(Waypoint lastVisited)
    {
        // If only reachable waypoint from here is the last one, go back there
        if (m_WaypointsInRange.Count == 1 && m_WaypointsInRange.Contains(lastVisited))
        {
            return lastVisited;
        }
        else
        {
            Waypoint nextWaypoint;
            do
            {
                nextWaypoint = m_WaypointsInRange[Random.Range(0, m_WaypointsInRange.Count)];
            }
            while (nextWaypoint == lastVisited);

            return nextWaypoint;
        }
    }

    public static Waypoint GetRandom()
    {
        Waypoint[] allWayPointsInScene = FindObjectsOfType<Waypoint>();
        if (allWayPointsInScene.Length <= 0)
        {
            Debug.LogError("No Waypoints found in scene!");
        }
        return allWayPointsInScene[Random.Range(0, allWayPointsInScene.Length)];
    }

}
