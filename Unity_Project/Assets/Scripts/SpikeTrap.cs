using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpikeTrap : MonoBehaviour
{
    // --------------------------------------------------------------

    // How long after stepping on Trap until it activates
    [SerializeField] private float m_ActivationTime = 1f;

    // How strongly object on top of trap is knocked backwards
    [SerializeField] private float m_KnockBackForce = 15f;

    // --------------------------------------------------------------

    private bool m_TrapActivated = false;

    private Animator m_Animator;

    private Collider m_Collider;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider>();
    }

    private void StartTrap()
    {
        m_Collider.enabled = false;
        m_Animator.SetTrigger("ActivationTrigger");
        Invoke("ExtendSpikes", m_ActivationTime);
    }

    private void ExtendSpikes()
    {
        m_Animator.SetTrigger("AttackTrigger");
        m_Collider.enabled = true;
        Invoke("RetractSpikes", 1f);
    }

    private void RetractSpikes()
    {
        m_TrapActivated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_TrapActivated)
        {
            // Activate trap first time tile stepped on
            m_TrapActivated = true;
            StartTrap();
        }
        else
        {   // If stepped on after spikes extended, damage and knock object backwards

            // If kinematic Rigidbody, temporarily activate its reactions to ohysics
            PhysicsSwitch manualMovedObject = other.GetComponent<PhysicsSwitch>();
            if (manualMovedObject != null)
            {
                manualMovedObject.ActivatePhysicsReactions(true);
            }

            // Apply knockback force
            Rigidbody body = other.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.AddForce(-body.transform.forward * m_KnockBackForce);
            }

            // Damage objects with Health
            IHealth otherHealth = other.GetComponent<IHealth>();
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(1);
            }
        }
    }

}
