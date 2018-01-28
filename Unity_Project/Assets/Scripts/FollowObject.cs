using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    // --------------------------------------------------------------

    private Vector3 m_Offset;

    private Transform m_Target;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Offset = transform.position;
    }

    public void SetTarget(Transform target)
    {
        m_Target = target;
        enabled = true;
    }

    private void Update()
    {
        transform.position = m_Target.position + m_Offset;
    }

}
