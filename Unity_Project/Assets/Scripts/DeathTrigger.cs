using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("DeathTrigger killing " + other.gameObject.name);

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            // Kill the player
            player.Die();
        }
        else if (other.GetComponent<PlayerFeet>() == null)
        {
            Destroy(other.gameObject);
        }
    }
}
