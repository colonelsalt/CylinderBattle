using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Jetpack : MonoBehaviour
{
    // --------------------------------------------------------------

    // How long Player can float in the air
    [SerializeField] private float m_FloatTime;

    // --------------------------------------------------------------

    private PlayerController m_Player;

    private ParticleSystem m_JetpackFire;

    // AudioSource that plays looping jetpack engine sound
    private AudioSource m_Audio;

    private bool m_IsFloating = false;

    private float m_RemainingFloatTime;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Player = GetComponentInParent<PlayerController>();
        m_JetpackFire = GetComponentInChildren<ParticleSystem>();
        m_Audio = GetComponent<AudioSource>();

        m_RemainingFloatTime = m_FloatTime;
    }

    private void Update()
    {
        if (m_IsFloating) m_RemainingFloatTime -= Time.deltaTime;

        if (InputHelper.JumpButtonPressed(m_Player.PlayerNum) && m_Player.IsAirBorne && m_RemainingFloatTime > 0f)
        {
            Activate();
        }
        if (InputHelper.JumpButtonReleased(m_Player.PlayerNum) || m_RemainingFloatTime <= 0f)
        {
            Deactivate();
        }
        if (!m_Player.IsAirBorne)
        {
            m_RemainingFloatTime = m_FloatTime;
        }
    }

    private void Activate()
    {
        m_Audio.volume = 1f;
        m_Audio.Play();

        m_IsFloating = true;
        m_Player.IsFloating = true;
        m_JetpackFire.Play();
    }

    private void Deactivate()
    {
        SoundManager.Instance.FadeOut(m_Audio);

        m_IsFloating = false;
        m_Player.IsFloating = false;
        m_JetpackFire.Stop();
    }

    private void OnDeath()
    {
        Deactivate();
        Destroy(gameObject);
    }

}
