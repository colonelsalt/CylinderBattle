using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // --------------------------------------------------------------

    private enum Direction { HORIZONTAL, VERTICAL }

    // --------------------------------------------------------------

    [SerializeField] private Direction m_Direction;

    [SerializeField] private float m_MoveDistance;

    [SerializeField] private float m_Speed = 2.5f;

    // How long to wait at destination point before changing direction
    [SerializeField] private float m_DelayTime = 2f;

    // --------------------------------------------------------------

    private Vector3 m_StartPosition;

    private Vector3 m_GoalPosition;

    private Vector3 m_MovementDirection;

    private float m_WaitTimeRemaining = 0f;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_StartPosition = transform.position;
        m_WaitTimeRemaining = 2.5f * m_DelayTime;
        m_MovementDirection = (m_Direction == Direction.HORIZONTAL) ? -transform.forward : transform.up;
    }

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

        if (Vector3.Distance(m_StartPosition, transform.position) >= m_MoveDistance)
        {
            m_MovementDirection *= -1;
            m_StartPosition = transform.position;
            m_WaitTimeRemaining = m_DelayTime;
        }
    }

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
