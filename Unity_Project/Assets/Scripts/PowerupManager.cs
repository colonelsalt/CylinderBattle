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

    [SerializeField] private GameObject m_BoxingGlovesPrefab;
    
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

    private void Awake()
    {
        m_Player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (InputHelper.FireButtonPressed(m_Player.PlayerNum))
        {
            ActivatePowerup();
        }
    }

    private void ActivatePowerup()
    {
        if (!m_HasPowerup || m_PowerupIsRunning) return;

        m_PowerupIsRunning = true;
        OnPowerupActivated(m_Powerup, m_Player.PlayerNum);

        switch (m_Powerup)
        {
            case Powerup.BOMB:
                // Spawn bomb 1m in front of Player with fuse pointing up (x=-90 deg. angle)
                Instantiate(m_BombPrefab, transform.position + transform.forward, Quaternion.Euler(-90f, 0f, 0f));
                m_PowerupIsRunning = false;
                break;
            case Powerup.GUN:
                // Spawn gun and child it to Player's 'Body' Transform
                Instantiate(m_GunPrefab, transform.GetChild(0));
                break;
            case Powerup.BOXING_GLOVES:
                // Spawn boxing gloves and child them to Player's 'Body' Transform
                Instantiate(m_BoxingGlovesPrefab, transform.GetChild(0));
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
        OnPowerupDisabled(m_Powerup, m_Player.PlayerNum);
    }
}
