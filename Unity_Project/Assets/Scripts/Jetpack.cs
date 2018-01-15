using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{

    // --------------------------------------------------------------

    // How long Player can float in the air
    [SerializeField] private float m_FloatTime;

    // --------------------------------------------------------------

    private CharacterController m_CharacterController;

    private PlayerController m_Player;

    private ParticleSystem m_JetpackFire;

    private bool m_IsFloating = false;

    private float m_RemainingFloatTime;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_CharacterController = GetComponentInParent<CharacterController>();
        m_Player = GetComponentInParent<PlayerController>();
        m_JetpackFire = GetComponentInChildren<ParticleSystem>();

        m_RemainingFloatTime = m_FloatTime;
    }

    private void Update()
    {
        if (m_IsFloating) m_RemainingFloatTime -= Time.deltaTime;

        if (InputHelper.JumpButtonPressed(m_Player.PlayerNum) && !m_CharacterController.isGrounded && m_RemainingFloatTime > 0f)
        {
            Activate();
        }
        if (InputHelper.JumpButtonReleased(m_Player.PlayerNum) || m_RemainingFloatTime <= 0f)
        {
            Deactivate();
        }
        if (m_CharacterController.isGrounded)
        {
            m_RemainingFloatTime = m_FloatTime;
        }
    }

    private void Activate()
    {
        m_IsFloating = true;
        m_Player.IsFloating = true;
        m_JetpackFire.Play();
    }

    private void Deactivate()
    {
        m_IsFloating = false;
        m_Player.IsFloating = false;
        m_JetpackFire.Stop();
    }

    private void OnPlayerDeath(int playerNum)
    {
        if (playerNum == m_Player.PlayerNum)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
    }

}
