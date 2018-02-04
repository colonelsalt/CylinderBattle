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

    // Sounds
    [SerializeField] private AudioClip[] m_FuseSounds;

    [SerializeField] private AudioClip[] m_ExplosionSounds;

    // --------------------------------------------------------------

    private GameObject m_BombOwner;

    // --------------------------------------------------------------

    private void Start()
    {
        SoundManager.Instance.PlayRandom(m_FuseSounds);
        Invoke("Explode", m_ExplosionTime);
    }

    // Called from WeaponManager; Attacker will take credit for deaths caused by this bomb
    public void AssignOwner(GameObject attacker)
    {
        m_BombOwner = attacker;
    }

    private void Explode()
    {
        SoundManager.Instance.PlayRandom(m_ExplosionSounds);

        Collider[] collidersStruck = Physics.OverlapSphere(transform.position, m_ExplosionRadius);
        foreach (Collider hit in collidersStruck)
        {
            // If struck kinematic Rigidbody, make it temporarily be affected by physics
            PhysicsSwitch manualMovedObject = hit.GetComponent<PhysicsSwitch>();
            manualMovedObject?.ActivatePhysicsReactions(true, m_BombOwner);

            // Apply explosion force to each Rigidbody hit
            Rigidbody body = hit.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius, m_ExplosionUpForce);
            }

            // Deal damage to all objects with Health
            IHealth health = hit.GetComponent<IHealth>();
            health?.TakeDamage(m_Damage, m_BombOwner);
        }

        Destroy(gameObject, 1f);
    }
}
