using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    NONE = 0,
    BOMB = 1,
    GUN = 2,
    BOXING_GLOVES = 3
}

public class WeaponManager : MonoBehaviour
{
    // --------------------------------------------------------------
    [SerializeField]
    private GameObject[] m_WeaponPrefabs;
    // --------------------------------------------------------------
    private Weapon m_CurrentWeapon = Weapon.NONE;
    // --------------------------------------------------------------

    public void AddWeapon(Weapon type)
    {
        if (m_CurrentWeapon == Weapon.NONE)
        {
            m_CurrentWeapon = type;
            Debug.Log("Received weapon " + type);
        }
    }
}
