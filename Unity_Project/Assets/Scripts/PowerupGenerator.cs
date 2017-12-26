using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Powerup
{
    BOMB = 0,
    GUN = 1,
    BOXING_GLOVES = 2,
    QUICK_BOOTS = 3,
    EXTRA_LIFE = 4
}

public static class PowerupGenerator
{
    public static Powerup[] ListAll()
    {
        return new Powerup[] { Powerup.BOMB, Powerup.GUN, Powerup.BOXING_GLOVES };
    }

    public static Powerup GetRandom()
    {
        return (Powerup)Random.Range(0, 3);
    }

    public static Powerup RandomWeapon()
    {
        return (Powerup)Random.Range(0, 3);
    }
}
