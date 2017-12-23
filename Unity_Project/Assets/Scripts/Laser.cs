using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_Speed;

    [SerializeField] private int m_Damage;

    // Which Player fired the laser
    [SerializeField] private int m_FiredByPlayer;
    
    // --------------------------------------------------------------

    // Flag to prevent multiple trigger events in one frame
    private bool m_TriggeredThisFrame = false;

    // --------------------------------------------------------------

    private void Update()
    {
        transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ensure only one trigger event per frame
        if (m_TriggeredThisFrame) return;
        m_TriggeredThisFrame = true;

        // If we hit an object that has Health
        Health otherHealth = other.GetComponent<Health>();
        if (otherHealth != null)
        {
            PlayerController playerHit = other.GetComponent<PlayerController>();
            if (playerHit != null)
            {
                // Do not damage Player who fired laser
                if (playerHit.PlayerNum == m_FiredByPlayer)
                {
                    Vanish();
                    return;
                }
            }

            // Damage hit object
            otherHealth.TakeDamage(m_Damage);
        }
        Vanish();
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}
