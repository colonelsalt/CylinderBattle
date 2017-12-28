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

public static class PowerupGenerator
{
    public static Weapon[] ListAll()
    {
        return new Weapon[] { Weapon.BOMB, Weapon.GUN, Weapon.BOXING_GLOVES };
    }

    public static Weapon GetRandom()
    {
        return (Weapon)Random.Range(0, 3);
    }

    public static Weapon RandomWeapon()
    {
        return (Weapon)Random.Range(0, 3);
    }
}
