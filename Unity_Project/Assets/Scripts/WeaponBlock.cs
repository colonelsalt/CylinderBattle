using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlock : MonoBehaviour
{
    // --------------------------------------------------------------
    
    // --------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        PowerupManager powerupManager = other.GetComponent<PowerupManager>();
        if (powerupManager != null)
        {
            powerupManager.AddPowerup(PowerupGenerator.RandomWeapon());
            Break();
        }
    }

    private void Break()
    {
        // play break animation
        Destroy(gameObject);
    }
}
