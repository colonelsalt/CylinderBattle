using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    BOMB = 0,
    GUN = 1,
    BOXING_GLOVES = 2,
    PORTAL_GUN = 3,
    LIGHTNING = 4
}

public class WeaponBlock : MonoBehaviour
{
    // --------------------------------------------------------------

    private static Weapon RandomWeapon()
    {
        return (Weapon)Random.Range(0, 4);
    }

    public void Break(WeaponManager brokenBy)
    {
        brokenBy.PickupWeapon(RandomWeapon());
        
        // play break animation
        Destroy(gameObject);
    }
}
