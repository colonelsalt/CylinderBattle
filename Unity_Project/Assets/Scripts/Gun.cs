using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    // --------------------------------------------------------------

    [SerializeField] private GameObject m_LaserPrefab;
    [SerializeField] private float m_FiringRate = 0.2f;
    [SerializeField] private int m_PlayerNum;

    // --------------------------------------------------------------
    
    // Events
    public delegate void GunEvent(int playerNum);
    public static event GunEvent OnGunFired;
    public static event GunEvent OnGunDisabled;

    // --------------------------------------------------------------

    public const int MAX_AMMO = 10;

    private int m_CurrentAmmo;
    
    // --------------------------------------------------------------

    private void Awake()
    {
        m_CurrentAmmo = MAX_AMMO;
        if (m_PlayerNum <= 0)
        {
            Debug.LogError("Invalid Player number specified.");
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            InvokeRepeating("Fire", 0.00001f, m_FiringRate);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            CancelInvoke("Fire");
        }
    }

    private void Fire()
    {
        OnGunFired(m_PlayerNum);
        m_CurrentAmmo--;
        Instantiate(m_LaserPrefab, transform.position, transform.rotation);
        if (m_CurrentAmmo <= 0)
        {
            //OnGunDisabled(m_PlayerNum);
            Destroy(gameObject);
        }
    }
}
