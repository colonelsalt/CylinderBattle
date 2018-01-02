using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpikeTrap : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_ActivationTime = 1f;

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
            m_TrapActivated = true;
            StartTrap();
        }
        else
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivatePhysicsReactions();
                player.GetComponent<Rigidbody>().AddForce(-player.transform.forward * m_KnockBackForce);
            }

            Health otherHealth = other.GetComponent<Health>();
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(1);
            }
        }
    }

}
