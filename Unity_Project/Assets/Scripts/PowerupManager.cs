using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PowerupManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_BombPrefab;

    [SerializeField] private GameObject m_GunPrefab;
    
    // --------------------------------------------------------------

    // Events
    public delegate void PowerupEvent (Powerup type, int playerNum);
    public static event PowerupEvent OnPowerupActivated;
    public static event PowerupEvent OnPowerupDisabled;

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
        if (Input.GetButtonDown("Fire1" + m_Player.GetPlayerInputString()))
        {
            ActivatePowerup();
        }
    }

    private void ActivatePowerup()
    {
        if (!m_HasPowerup || m_PowerupIsRunning) return;

        m_PowerupIsRunning = true;
        OnPowerupActivated(m_Powerup, m_Player.GetPlayerNum());

        switch (m_Powerup)
        {
            case Powerup.BOMB:
                // Spawn bomb 1m in front of Player with fuse pointing up (x=-90 deg. angle)
                Instantiate(m_BombPrefab, transform.position + transform.forward, Quaternion.Euler(-90f, 0, 0));
                m_PowerupIsRunning = false;
                break;
            case Powerup.GUN:
                // Spawn gun 80cm in front of Player, and child it to the Player's Transform
                GameObject gun = Instantiate(m_GunPrefab, transform) as GameObject;
                gun.transform.position += (transform.forward * 0.8f);
                gun.GetComponent<Gun>().AttachToPlayer(m_Player);
                break;
            case Powerup.BOXING_GLOVES:
                break;
        }
        m_HasPowerup = false;
    }

    public void AddPowerup(Powerup powerup)
    {
        if (m_HasPowerup) return;
        
        m_HasPowerup = true;
        m_Powerup = powerup;
        Debug.Log("Received powerup " + m_Powerup);
    }

    public void DisablePowerup()
    {
        m_PowerupIsRunning = false;
        OnPowerupDisabled(m_Powerup, m_Player.GetPlayerNum());
    }
}
