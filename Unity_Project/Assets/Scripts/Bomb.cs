using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // --------------------------------------------------------------
    [SerializeField] private float m_ExplosionTime;
    [SerializeField] private float m_ExplosionForce;
    [SerializeField] private float m_ExplosionUpForce;
    [SerializeField] private float m_ExplosionRadius;
    [SerializeField] private int m_Damage;
    // --------------------------------------------------------------

    private void Start()
    {
        Invoke("Explode", m_ExplosionTime);
    }

    private void Explode()
    {
        Collider[] collidersStruck = Physics.OverlapSphere(transform.position, m_ExplosionRadius);
        foreach (Collider hit in collidersStruck)
        {
            // If struck a Player object, trigger them to be affected by physics
            PlayerController playerHit = hit.GetComponent<PlayerController>();
            if (playerHit != null)
            {
                playerHit.ActivatePhysicsReactions();
            }

            // Apply explosion force to each Rigidbody hit
            Rigidbody rigidBody = hit.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                rigidBody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius, m_ExplosionUpForce);
            }

            // Deal damage to all objects with Health
            Health beingHit = hit.GetComponent<Health>();
            if (beingHit != null)
            {
                beingHit.TakeDamage(m_Damage);
            }

        }

        Destroy(gameObject, 0.5f);
    }
}
