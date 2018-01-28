using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Platforms that moves back and forth along a single dimension
public class MovingPlatform : MonoBehaviour
{
    // --------------------------------------------------------------

    private enum Direction { HORIZONTAL, VERTICAL }

    // --------------------------------------------------------------

    [SerializeField] private Direction m_Direction;

    // How far to move before reversing direction
    [SerializeField] private float m_MoveDistance;

    [SerializeField] private float m_Speed = 2.5f;

    // How long to wait at destination point before reversing
    [SerializeField] private float m_DelayTime = 2f;

    // --------------------------------------------------------------

    private Vector3 m_StartPosition;

    private Vector3 m_MovementDirection;

    private float m_WaitTimeRemaining = 0f;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_StartPosition = transform.position;
        m_WaitTimeRemaining = 2.5f * m_DelayTime; // Wait a little extra on level start to give Players chance to reach platform
        m_MovementDirection = (m_Direction == Direction.HORIZONTAL) ? -transform.forward : transform.up;
    }

    // Visualise movement in Scene View
    private void OnDrawGizmos()
    {
        Vector3 dir = (m_Direction == Direction.HORIZONTAL) ? -transform.forward : transform.up;
        Debug.DrawRay(transform.position, dir * m_MoveDistance, Color.blue);
    }

    private void Update()
    {
        if (m_WaitTimeRemaining > 0f)
        {
            m_WaitTimeRemaining -= Time.deltaTime;
        }
        else
        {
            transform.Translate(m_MovementDirection * m_Speed * Time.deltaTime);
        }

        // If travelled desired distance, reverse direction and wait before moving again
        if (Vector3.Distance(m_StartPosition, transform.position) >= m_MoveDistance)
        {
            m_MovementDirection *= -1;
            m_StartPosition = transform.position;
            m_WaitTimeRemaining = m_DelayTime;
        }
    }

    // Child Player to platform on touch so they travel along with it
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<IPlayer>() != null)
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IPlayer>() != null)
        {
            other.transform.SetParent(null);
        }
    }

}
