using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller for Player's weapons; receives them and spawns them, then hands over control to respective Weapon components
public class WeaponManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_BombPrefab;

    [SerializeField] private GameObject m_GunPrefab;

    [SerializeField] private GameObject m_BoxingGlovesPrefab;

    [SerializeField] private GameObject m_PortalGunPrefab;

    // --------------------------------------------------------------

    // Sounds
    [SerializeField] private AudioClip[] m_WeaponReceivedSounds;

    // --------------------------------------------------------------

    // Events
    public delegate void WeaponEvent (Weapon type, int playerNum);
    public static event WeaponEvent OnWeaponPickup;
    public static event WeaponEvent OnWeaponActivated;
    public static event WeaponEvent OnWeaponDisabled;

    // --------------------------------------------------------------

    private int m_PlayerNum;

    // Whether Player currently holds (but has not yet activated) a weapon
    private bool m_HasWeapon = false;

    private bool m_WeaponIsActive = false;

    // Currently held weapon
    private Weapon m_Weapon;

    // --------------------------------------------------------------

    public Weapon CurrentWeapon
    {
        get
        {
            return m_Weapon;
        }
    }

    // --------------------------------------------------------------


    private void Awake()
    {
        m_PlayerNum = GetComponent<IPlayer>().PlayerNum();
    }

    private void Update()
    {
        if (InputHelper.FireButtonPressed(m_PlayerNum))
        {
            ActivateWeapon();
        }
    }

    private void ActivateWeapon()
    {
        if (!m_HasWeapon || m_WeaponIsActive) return;

        m_WeaponIsActive = true;

        switch (m_Weapon)
        {
            case Weapon.BOMB:
                // Spawn bomb 1m in front of Player with fuse pointing up (x=-90 deg. angle)
                GameObject bomb = Instantiate(m_BombPrefab, transform.position + transform.forward, Quaternion.Euler(-90f, 0f, 0f)) as GameObject;
                bomb.GetComponent<Bomb>().AssignOwner(gameObject);
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
            case Weapon.PORTAL_GUN:
                Instantiate(m_PortalGunPrefab, transform.GetChild(0));
                break;
        }

        OnWeaponActivated(m_Weapon, m_PlayerNum);
        m_HasWeapon = false;
    }

    // If collected weapon block while holding weapon, "refill" it
    private void RefillWeapon()
    {
        BroadcastMessage("OnWeaponReset");
        OnWeaponActivated(m_Weapon, m_PlayerNum);
    }

    public void PickupWeapon(Weapon weapon)
    {
        SoundManager.Instance.PlayRandom(m_WeaponReceivedSounds);
        if (m_WeaponIsActive)
        {
            RefillWeapon();
            return;
        }
        
        m_HasWeapon = true;
        m_Weapon = weapon;

        OnWeaponPickup(weapon, m_PlayerNum);
    }

    // Broadcast from PlayerHealth
    private void OnDeath()
    {
        DisableWeapon();
    }

    public void DisableWeapon()
    {
        m_WeaponIsActive = false;
        OnWeaponDisabled(m_Weapon, m_PlayerNum);
    }
}
