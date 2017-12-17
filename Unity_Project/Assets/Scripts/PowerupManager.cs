using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PowerupManager : MonoBehaviour
{
    // --------------------------------------------------------------
    [SerializeField]
    private GameObject m_BombPrefab;
    [SerializeField]
    private GameObject m_GunPrefab;
    // --------------------------------------------------------------

    // Events
    public delegate void PowerupEvent (Powerup type, int playerNum);
    public static event PowerupEvent OnPowerupGet;
    public static event PowerupEvent OnPowerupDisable;
    // --------------------------------------------------------------

    private PlayerController m_Player;
    private bool m_HasPowerup = false;
    private bool m_PowerupIsRunning = false;
    private Powerup m_Powerup;
    // --------------------------------------------------------------

    private void Start()
    {
        m_Player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ActivatePowerup();
        }
    }

    private void ActivatePowerup()
    {
        Debug.Log("ActivateWeapon called!");
        if (!m_HasPowerup) return;
        switch (m_Powerup)
        {
            case Powerup.BOMB:
                Vector3 spawnPos = transform.position + transform.forward;
                Instantiate(m_BombPrefab, spawnPos, Quaternion.Euler(-90f, 0, 0));
                DisablePowerup();
                break;
            case Powerup.BOXING_GLOVES:
                break;
            case Powerup.GUN:
                break;
        }
    }

    public void AddPowerup(Powerup powerup)
    {
        if (!m_HasPowerup)
        {
            m_HasPowerup = true;
            m_Powerup = powerup;
            Debug.Log("Received weapon " + m_Powerup);
        }
    }

    private void DisablePowerup()
    {
        //OnPowerupDisable(m_Powerup.PowerType, m_Player.GetPlayerNum());
        m_HasPowerup = false;
    }
}
