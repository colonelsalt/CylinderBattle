using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    // --------------------------------------------------------------

    private PlayerController m_Player;

    // --------------------------------------------------------------


    private void Awake()
    {
        m_Player = GetComponentInParent<PlayerController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player's feet collided with something!");
        PlayerController otherPlayer = other.GetComponent<PlayerController>();
        if (otherPlayer != null)
        {
            if (otherPlayer.PlayerNum != m_Player.PlayerNum)
            {
                otherPlayer.GetComponent<Health>().TakeDamage(1);
            }
        }

        WeaponBlock weaponBlock = other.GetComponent<WeaponBlock>();
        if (weaponBlock != null)
        {
            weaponBlock.Break(m_Player.GetComponent<PowerupManager>());
        }
    }
}
