using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlock : MonoBehaviour
{
    // --------------------------------------------------------------
    // Events
    public delegate void WeaponPickup(Weapon type);
    public static event WeaponPickup OnWeaponPickup;
    // --------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Weapon block triggered against " + other.gameObject);
        WeaponManager weaponManager = other.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.AddWeapon((Weapon)Random.Range(0, (int)Weapon.BOXING_GLOVES));
            Break();
        }
    }

    private void Break()
    {
        Destroy(gameObject);
    }
}
