using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointGizmo : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private Color m_Color;

    [SerializeField] private bool m_DrawReachRay;

    // --------------------------------------------------------------

    private void OnDrawGizmos()
    {
        Gizmos.color = m_Color;
        Gizmos.DrawWireSphere(transform.position, 1f);

        if (m_DrawReachRay)
        {
            Gizmos.DrawRay(transform.position, transform.up * -6f);
        }
    }

}
