using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class WeaponManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_BombPrefab;

    [SerializeField] private GameObject m_GunPrefab;

    [SerializeField] private GameObject m_BoxingGlovesPrefab;
    
    // --------------------------------------------------------------

    // Events
    public delegate void WeaponEvent (Weapon type, int playerNum);
    public static event WeaponEvent OnWeaponActivated;
    public static event WeaponEvent OnWeaponDisabled;

    // --------------------------------------------------------------

    private PlayerController m_Player;

    private bool m_HasWeapon = false;

    private bool m_WeaponIsActive = false;

    private Weapon m_Weapon;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (InputHelper.FireButtonPressed(m_Player.PlayerNum))
        {
            ActivateWeapon();
        }
    }

    private void ActivateWeapon()
    {
        if (!m_HasWeapon || m_WeaponIsActive) return;

        m_WeaponIsActive = true;
        OnWeaponActivated(m_Weapon, m_Player.PlayerNum);

        switch (m_Weapon)
        {
            case Weapon.BOMB:
                // Spawn bomb 1m in front of Player with fuse pointing up (x=-90 deg. angle)
                Instantiate(m_BombPrefab, transform.position + transform.forward, Quaternion.Euler(-90f, 0f, 0f));
                m_WeaponIsActive = false;
                break;
            case Weapon.GUN:
                // Spawn gun and child it to Player's 'Body' Transform
                Instantiate(m_GunPrefab, transform.GetChild(0));
                break;
            case Weapon.BOXING_GLOVES:
                // Spawn boxing gloves and child them to Player's 'Body' Transform
                Instantiate(m_BoxingGlovesPrefab, transform.GetChild(0));
                break;
        }
        m_HasWeapon = false;
    }

    public void AddWeapon(Weapon weapon)
    {
        if (m_HasWeapon) return;
        
        m_HasWeapon = true;
        m_Weapon = weapon;
        Debug.Log("Received powerup " + m_Weapon);
    }

    public void DisablePowerup()
    {
        m_WeaponIsActive = false;
        OnWeaponDisabled(m_Weapon, m_Player.PlayerNum);
    }
}
