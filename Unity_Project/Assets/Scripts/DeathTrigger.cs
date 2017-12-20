using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if(playerController != null)
            {
                // Kill the player
                playerController.Die();

                other.GetComponent<Health>().Die();
            }
    }
}
