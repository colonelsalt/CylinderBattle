using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // --------------------------------------------------------------
    [SerializeField] private float m_Speed;

    [SerializeField] private int m_Damage;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_VanishSounds;

    // --------------------------------------------------------------

    // Flag to prevent multiple trigger events in one frame
    private bool m_TriggeredThisFrame = false;

    private GameObject m_GunFiredBy;

    private Animator m_Animator;

    private bool m_HasVanished = false;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Called by Gun when Laser instantiated
    public void AssignOwner(GameObject owner)
    {
        m_GunFiredBy = owner;
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
        IHealth otherHealth = other.GetComponent<IHealth>();
        if (otherHealth != null)
        {
            // Damage hit object
            SoundManager.Instance.PlayRandom(m_VanishSounds);
            otherHealth.TakeDamage(m_Damage, m_GunFiredBy);
        }

        // Unless collided with a Portal, laser beam vanishes against anything else
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
