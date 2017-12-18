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

    private float m_timePassed = 0f;
    // --------------------------------------------------------------

    private void Update()
    {
        m_timePassed += Time.deltaTime;
        if (m_timePassed >= m_ExplosionTime)
        {
            Explode();
        }
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

        Destroy(gameObject, 1f);
    }
}
