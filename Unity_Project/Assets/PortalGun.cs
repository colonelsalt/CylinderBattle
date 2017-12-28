using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    // --------------------------------------------------------------

    private PlayerController m_Player;

    private LineRenderer m_AimLine;

    private Vector3 m_AimPos;

    private string m_TagOfAimTarget;

    private bool m_IsAimingAtObject = false;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Player = GetComponentInParent<PlayerController>();
        m_AimLine = GetComponentInChildren<LineRenderer>();
    }

    private void Update()
    {
        UpdateAimTarget();
        UpdateAimLine();
        if (InputHelper.FireButtonPressed(m_Player.PlayerNum) && m_TagOfAimTarget == "Wall")
        {
            FirePortal();
        }
    }

    private void UpdateAimLine()
    {
        if (!m_IsAimingAtObject)
        {
            m_AimLine.enabled = false;
            return;
        }

        m_AimLine.enabled = true;
        m_AimLine.SetPosition(0, transform.position + (1.5f * transform.forward));
        m_AimLine.SetPosition(1, m_AimPos);

        if (m_TagOfAimTarget == "Wall")
        {
            m_AimLine.startColor = m_AimLine.endColor = Color.green;
        }
        else
        {
            m_AimLine.startColor = m_AimLine.endColor = Color.red;
        }
    }

    private void UpdateAimTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
            m_IsAimingAtObject = true;
            m_AimPos = hit.point;
            m_TagOfAimTarget = hit.collider.gameObject.tag;
        }
        else
        {
            m_IsAimingAtObject = false;
        }
    }

    private void FirePortal()
    {

    }

}
