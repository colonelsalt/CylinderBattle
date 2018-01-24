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

    // Sounds
    [SerializeField] private AudioClip[] m_GunSounds;

    // --------------------------------------------------------------

    // Events
    public delegate void GunEvent(int playerNum);
    public static event GunEvent OnGunFired;

    // --------------------------------------------------------------

    public const int MAX_AMMO = 10;

    // The Player this Gun belongs to
    private int m_PlayerNum;

    private int m_RemainingAmmo;

    // Flag to keep track of whether Gun is currently firing
    private bool m_IsFiring = true;
    
    // --------------------------------------------------------------

    public int RemainingAmmo
    {
        get
        {
            return m_RemainingAmmo;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        m_RemainingAmmo = MAX_AMMO;
        m_PlayerNum = GetComponentInParent<IPlayer>().PlayerNum();
    }

    private void Update()
    {
        if (InputHelper.FireButtonPressed(m_PlayerNum) && !m_IsFiring)
        {
            InvokeRepeating("Fire", 0.00001f, m_FiringRate);
            m_IsFiring = true;
        }
        if (InputHelper.FireButtonReleased(m_PlayerNum))
        {
            CancelInvoke("Fire");
            m_IsFiring = false;
        }
    }

    // Send off OnGunFired event and spawn Laser Prefab 2.5m in front of root object
    private void Fire()
    {
        m_RemainingAmmo--;
        OnGunFired(m_PlayerNum);
        Vector3 spawnPos = transform.position + (2.5f * transform.forward);

        GameObject laser = Instantiate(m_LaserPrefab, spawnPos, transform.rotation) as GameObject;
        laser.GetComponent<Laser>().AssignOwner(transform.parent.gameObject);

        SoundManager.Instance.PlayRandom(m_GunSounds);

        // If ran out of ammo, remove Gun
        if (m_RemainingAmmo <= 0)
        {
            Deactivate();
        }
    }

    private void OnWeaponReset()
    {
        m_RemainingAmmo = MAX_AMMO;
    }

    private void OnDeath()
    {
         Deactivate();
    }

    private void Deactivate()
    {
        GetComponentInParent<WeaponManager>().DisableWeapon();
        Destroy(gameObject);
    }
}
