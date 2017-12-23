using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    // --------------------------------------------------------------

    [SerializeField] private GameObject m_LaserPrefab;

    [SerializeField] private float m_FiringRate = 0.2f;

    // --------------------------------------------------------------
    
    // Events
    public delegate void GunEvent(int playerNum);
    public static event GunEvent OnGunFired;

    // --------------------------------------------------------------

    public const int MAX_AMMO = 10;

    private int m_CurrentAmmo;

    private PlayerController m_Player;
    
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
        RotateAim();

        if (Input.GetButtonDown("Fire1" + m_Player.GetPlayerInputString()))
        {
            InvokeRepeating("Fire", 0.00001f, m_FiringRate);
        }
        if (Input.GetButtonUp("Fire1" + m_Player.GetPlayerInputString()))
        {
            CancelInvoke("Fire");
        }
    }

    private void RotateAim()
    {
        Debug.Log("RightAxisX: " + Input.GetAxis("RightAxisX" + m_Player.GetPlayerInputString()) + ", RightAxisY: " + Input.GetAxis("RightAxisY" + m_Player.GetPlayerInputString()));

        float xRotation = Input.GetAxis("RightAxisX" + m_Player.GetPlayerInputString());
        float yRotation = Input.GetAxis("RightAxisY" + m_Player.GetPlayerInputString());
        float rotationAmount = Mathf.Atan2(xRotation, yRotation) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationAmount, transform.eulerAngles.z);
    }

    private void Fire()
    {
        OnGunFired(m_Player.GetPlayerNum());
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
