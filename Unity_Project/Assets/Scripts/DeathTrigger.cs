﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    // --------------------------------------------------------------

    // Events
    public delegate void DeathTriggerEvent();
    public static event DeathTriggerEvent OnPlayerOutOfBounds;

    // --------------------------------------------------------------

    // Explosion effect to spawn when Player drops in lava
    [SerializeField] private GameObject m_ExplosionEffect;

    // Set if this DeathTrigger is outside bounds of LevelMesh
    [SerializeField] private bool m_OutOfBounds = false;

    // --------------------------------------------------------------

    // Sounds
    [SerializeField] private AudioClip[] m_DeathSounds;

    // --------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            SoundManager.Instance.PlayRandom(m_DeathSounds);

            // Spawn explosion effect
            Instantiate(m_ExplosionEffect, other.transform.position + (3f * transform.up), Quaternion.identity);

            // Kill object
            player.Die(gameObject);

            if (m_OutOfBounds)
            {
                OnPlayerOutOfBounds();
            }

        }
        else if (other.GetComponent<PlayerFeet>() == null)
        {
            Destroy(other.gameObject);
        }
    }
}
