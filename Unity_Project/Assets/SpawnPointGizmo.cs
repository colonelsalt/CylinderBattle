using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointGizmo : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private Color m_Color;

    // --------------------------------------------------------------

    private void OnDrawGizmos()
    {
        Gizmos.color = m_Color;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

}
