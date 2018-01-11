using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // --------------------------------------------------------------

    // Time before bomb explodes after being dropped
    [SerializeField] private float m_ExplosionTime;

    // Force surrounding objects will be struck by (horizontally)
    [SerializeField] private float m_ExplosionForce;

    // How hard surrounding objects will be thrust upwards
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
            // If struck kinematic Rigidbody, make it temporarily be affected by physics
            PhysicsSwitch manualMovedObject = hit.GetComponent<PhysicsSwitch>();
            if (manualMovedObject != null)
            {
                manualMovedObject.ActivatePhysicsReactions();
            }

            // Apply explosion force to each Rigidbody hit
            Rigidbody rigidBody = hit.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                rigidBody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius, m_ExplosionUpForce);
            }

            // Deal damage to all objects with Health
            Health health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(m_Damage);
            }

        }

        Destroy(gameObject, 1f);
    }
}
