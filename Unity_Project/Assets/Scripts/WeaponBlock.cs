using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlock : MonoBehaviour
{
    // --------------------------------------------------------------

    private float m_Radius;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Radius = GetComponent<MeshRenderer>().bounds.center.y;
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        PowerupManager powerupManager = collision.gameObject.GetComponent<PowerupManager>();
        if (powerupManager != null)
        {
            if (powerupManager.GetComponentInChildren<MeshRenderer>().bounds.min.y >= GetComponent<MeshRenderer>().bounds.max.y)
            {
                powerupManager.AddPowerup(PowerupGenerator.RandomWeapon());
                Break();
            }
            
        }
    }

    private void Break()
    {
        // play break animation
        Destroy(gameObject);
    }
}
