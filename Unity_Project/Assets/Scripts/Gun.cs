using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    // --------------------------------------------------------------

    [SerializeField] private GameObject m_LaserPrefab;

    // How many seconds delay between consecutive shots
    [SerializeField] private float m_FiringRate = 0.2f;

    // --------------------------------------------------------------
    
    // Events
    public delegate void GunEvent(int playerNum);
    public static event GunEvent OnGunFired;

    // --------------------------------------------------------------

    public const int MAX_AMMO = 10;

    // The Player this Gun belongs to
    private PlayerController m_Player;

    private int m_CurrentAmmo;

    // Flag to keep track of whether Gun is currently firing
    private bool m_IsFiring = false;
    
    // --------------------------------------------------------------

    private void Awake()
    {
        m_CurrentAmmo = MAX_AMMO;
    }

    public void AttachToPlayer(PlayerController player)
    {
        m_Player = player;
    }

    private void Update()
    {
        // Allow Player to change gun direction if using a gamepad
        if (InputHelper.GamePadConnected()) RotateAim();

        if (InputHelper.FireButtonPressed(m_Player.PlayerNum) && !m_IsFiring)
        {
            InvokeRepeating("Fire", 0.00001f, m_FiringRate);
            m_IsFiring = true;
        }
        if (InputHelper.FireButtonReleased(m_Player.PlayerNum))
        {
            CancelInvoke("Fire");
            m_IsFiring = false;
        }
    }

    private void RotateAim()
    {
        float cos = InputHelper.GetRightStickX(m_Player.PlayerNum);
        float sin = InputHelper.GetRightStickY(m_Player.PlayerNum);

        // If right stick hasn't moved, don't change rotation
        if (cos == 0f && sin == 0f) return;

        // Find angle represented by current stick position
        float rotationAngle = Mathf.Atan2(cos, sin) * Mathf.Rad2Deg;

        // Rotate gun to face this direction
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationAngle, transform.eulerAngles.z);
    }

    private void Fire()
    {
        OnGunFired(m_Player.PlayerNum);
        m_CurrentAmmo--;
        Vector3 spawnPos = transform.position + (2.5f * transform.forward);
        Instantiate(m_LaserPrefab, spawnPos, transform.rotation);
        if (m_CurrentAmmo <= 0)
        {
            m_Player.GetComponent<PowerupManager>().DisablePowerup();
            Destroy(gameObject);
        }
    }
}
