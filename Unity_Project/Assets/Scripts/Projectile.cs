using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // --------------------------------------------------------------
    [SerializeField] private float m_Speed;
    [SerializeField] private int m_Damage;
    [SerializeField] private int m_FiredByPlayer;
    // --------------------------------------------------------------


    private void Update()
    {
        transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Health otherHealth = other.GetComponent<Health>();
        if (otherHealth != null)
        {
            PlayerController playerHit = other.GetComponent<PlayerController>();
            if (playerHit != null)
            {
                // Do not damage Player who fired laser
                if (playerHit.GetPlayerNum() == m_FiredByPlayer)
                {
                    Vanish();
                    return;
                }
            }
            otherHealth.TakeDamage(m_Damage);
        }
        Vanish();
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}
