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
    // --------------------------------------------------------------

    private void Start()
    {
        Invoke("Explode", m_ExplosionTime);
    }

    private void Explode()
    {
        Debug.Log("Bomb exploded!");
        Collider[] collidersStruck = Physics.OverlapSphere(transform.position, m_ExplosionRadius);
        foreach (Collider hit in collidersStruck)
        {
            Rigidbody rigidBody = hit.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                rigidBody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius, m_ExplosionUpForce);
            }
        }

        Destroy(gameObject, 0.5f);
    }
}
