using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlock : MonoBehaviour
{
    // --------------------------------------------------------------
    // --------------------------------------------------------------

    //private void OnCollisionEnter(Collision collision)
    //{
    //    PowerupManager powerupManager = collision.gameObject.GetComponent<PowerupManager>();
    //    if (powerupManager != null)
    //    {
    //        // If Player's lowest y-coordinate is higher than Block's highest y-coordinate, break Block
    //        if (powerupManager.GetComponentInChildren<MeshRenderer>().bounds.min.y >= GetComponent<MeshRenderer>().bounds.max.y)
    //        {
    //            powerupManager.AddPowerup(PowerupGenerator.RandomWeapon());
    //            Break();
    //        }
            
    //    }
    //}

    public void Break(WeaponManager brokenBy)
    {
        brokenBy.AddWeapon(PowerupGenerator.RandomWeapon());
        
        // play break animation
        Destroy(gameObject);
    }
}
