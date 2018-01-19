﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_ExplosionEffect;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_DeathSounds;

    // --------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        IHealth health = other.GetComponent<IHealth>();
        if (health != null)
        {
            SoundManager.Instance.PlayRandom(m_DeathSounds);

            // Spawn explosion effect
            Instantiate(m_ExplosionEffect, other.transform.position + (3f * transform.up), Quaternion.identity);

            // Kill object
            health.Die();
        }
        else if (other.GetComponent<PlayerFeet>() == null)
        {
            Destroy(other.gameObject);
        }
    }
}
