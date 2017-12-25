﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // --------------------------------------------------------------

    // Laser prefab to spawn when Player fires Gun
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
        m_Player = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
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

    // Send off OnGunFired event and spawn Laser Prefab 2.5m in front of root object
    private void Fire()
    {
        OnGunFired(m_Player.PlayerNum);
        m_CurrentAmmo--;
        Vector3 spawnPos = transform.position + (2.5f * transform.forward);
        Instantiate(m_LaserPrefab, spawnPos, transform.rotation);

        // If ran out of ammo, remove Gun
        if (m_CurrentAmmo <= 0)
        {
            m_Player.GetComponent<PowerupManager>().DisablePowerup();
            Destroy(gameObject);
        }
    }
}
