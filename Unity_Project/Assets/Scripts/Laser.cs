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

    private Animator m_Animator;

    private bool m_HasVanished = false;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!m_HasVanished)
        {
            transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
        }
        m_TriggeredThisFrame = false;
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

        // Unless collided with a Portal, laser beam vanishes
        if (other.GetComponent<Portal>() == null)
        {
            Vanish();
        }
    }

    private void Vanish()
    {
        m_HasVanished = true;
        m_Animator.SetTrigger("VanishTrigger");
        Destroy(gameObject, 0.5f);
    }
}
