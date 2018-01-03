using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_GreenPortalPrefab;

    [SerializeField] private GameObject m_PurplePortalPrefab;

    // --------------------------------------------------------------

    private PlayerController m_Player;

    private LineRenderer m_AimLine;

    private Vector3 m_TargetPos;

    private Quaternion m_TargetRotation;

    private string m_TargetTag;

    private bool m_IsAimingAtObject = false;

    private GameObject m_GreenPortal;

    private GameObject m_PurplePortal;

    private bool m_IsFiring = false;

    // --------------------------------------------------------------

    private void Awake()
    {
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;

        m_Player = GetComponentInParent<PlayerController>();
        m_AimLine = GetComponentInChildren<LineRenderer>();
    }

    private void Update()
    {
        UpdateAimTarget();
        UpdateAimLine();

        if (InputHelper.FireButtonPressed(m_Player.PlayerNum) && m_TargetTag == "Wall" && !m_IsFiring)
        {
            m_IsFiring = true;
            FirePortal();
        }
        if (InputHelper.FireButtonReleased(m_Player.PlayerNum))
        {
            m_IsFiring = false;
        }
    }

    private void UpdateAimLine()
    {
        // If Aiming at nothing, don't show AimLine
        if (!m_IsAimingAtObject)
        {
            m_AimLine.enabled = false;
            return;
        }

        // Otherwise, draw line between tip of Portal Gun and object Player is aiming at
        m_AimLine.enabled = true;
        m_AimLine.SetPosition(0, transform.position + (1.5f * transform.forward));
        m_AimLine.SetPosition(1, m_TargetPos);

        if (m_TargetTag == "Wall")
        {
            m_AimLine.startColor = m_AimLine.endColor = Color.green;
        }
        else
        {
            m_AimLine.startColor = m_AimLine.endColor = Color.red;
        }
    }

    // Determine what Player is aiming at
    private void UpdateAimTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
            m_IsAimingAtObject = true;

            m_TargetPos = hit.point;
            m_TargetRotation = Quaternion.LookRotation(hit.normal);

            m_TargetTag = hit.collider.gameObject.tag;
        }
        else
        {
            m_IsAimingAtObject = false;
        }
    }

    private void FirePortal()
    {
        if (m_GreenPortal == null)
        {
            m_GreenPortal = Instantiate(m_GreenPortalPrefab, m_TargetPos, m_TargetRotation) as GameObject;
        }
        else if (m_PurplePortal == null)
        {
            m_PurplePortal = Instantiate(m_PurplePortalPrefab, m_TargetPos, m_TargetRotation) as GameObject;
            m_GreenPortal.GetComponent<Portal>().AttachToPortal(m_PurplePortal.transform);
            m_PurplePortal.GetComponent<Portal>().AttachToPortal(m_GreenPortal.transform);

            Deactivate();
        }
    }

    private void OnPlayerDeath(int playerNum)
    {
        if (playerNum != m_Player.PlayerNum) return;

        // If only one portal has been fired, destroy it
        if (m_PurplePortal == null)
        {
            if (m_GreenPortal != null) Destroy(m_GreenPortal);
        }

        Deactivate();
    }

    private void Deactivate()
    {
        PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
        m_Player.GetComponent<WeaponManager>().DisableWeapon();
        Destroy(gameObject);
    }

}
