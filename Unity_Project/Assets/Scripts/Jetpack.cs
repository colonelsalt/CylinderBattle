using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Jetpack : MonoBehaviour
{
    // --------------------------------------------------------------

    // How long Player can float in the air before dropping
    [SerializeField] private float m_FloatTime;

    // --------------------------------------------------------------

    // PlayerController reference needed to synchronise movement
    private PlayerController m_PlayerController;

    // Fire effects to loop when floating
    private ParticleSystem m_JetpackFire;

    // AudioSource that plays looping jetpack engine sound
    private AudioSource m_Audio;

    private bool m_IsFloating = false;

    // Time left before player drops
    private float m_RemainingFloatTime;

    private int m_PlayerNum;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_PlayerNum = GetComponentInParent<IPlayer>().PlayerNum();

        m_PlayerController = GetComponentInParent<PlayerController>();
        m_JetpackFire = GetComponentInChildren<ParticleSystem>();
        m_Audio = GetComponent<AudioSource>();

        m_RemainingFloatTime = m_FloatTime;
    }

    private void Update()
    {
        if (m_IsFloating) m_RemainingFloatTime -= Time.deltaTime;

        if (InputHelper.JumpButtonPressed(m_PlayerNum) && m_PlayerController.IsAirBorne && m_RemainingFloatTime > 0f)
        {
            Activate();
        }
        if (InputHelper.JumpButtonReleased(m_PlayerNum) || m_RemainingFloatTime <= 0f)
        {
            Deactivate();
        }
        if (!m_PlayerController.IsAirBorne)
        {
            // Once touched ground again, return float time to max
            m_RemainingFloatTime = m_FloatTime;
        }
    }

    // Begin floating
    private void Activate()
    {
        m_Audio.volume = 1f;
        m_Audio.Play();

        m_IsFloating = true;
        m_PlayerController.IsFloating = true;
        m_JetpackFire.Play();
    }

    // Stop floating
    private void Deactivate()
    {
        SoundManager.Instance.FadeOut(m_Audio);

        m_IsFloating = false;
        m_PlayerController.IsFloating = false;
        m_JetpackFire.Stop();
    }

    private void OnDeath()
    {
        Deactivate();
        Destroy(gameObject);
    }

}
