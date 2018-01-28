using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Component for LightningSprint powerup (30 Pluses)
[RequireComponent(typeof(AudioSource))]
public class LightningSprint : MonoBehaviour
{
    // How long Player can sprint for before recharge needed
    [SerializeField] private float m_MaxSprintTime;

    // --------------------------------------------------------------

    // Sounds
    [SerializeField] private AudioClip[] m_OutOfStaminaSounds;

    // --------------------------------------------------------------

    private int m_PlayerNum;

    // Reference to PlayerController to synchronise movement
    private PlayerController m_PlayerController;

    // Renderer to show on top of Player when sprinting
    private Renderer m_Rend;

    // Lightning effect to loop while sprinting
    private ParticleSystem m_Lightning;

    private Animator m_Animator;

    private AudioSource m_Audio;

    private float m_RemainingSprintTime;

    // Whether Player has left sprint time fall to zero and needs rest before they can sprint again
    private bool m_HasStamina = true;

    // Flag to prevent constantly replaying audio
    private bool m_PlayingSprintSound = false;

    // --------------------------------------------------------------

    public float RemainingSprintTime
    {
        get
        {
            return m_RemainingSprintTime;
        }
    }

    public float MaxSprintTime
    {
        get
        {
            return m_MaxSprintTime;
        }
    }

    public bool HasStamina
    {
        get
        {
            return m_HasStamina;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        m_PlayerNum = GetComponentInParent<IPlayer>().PlayerNum();
        m_PlayerController = GetComponentInParent<PlayerController>();
        m_Rend = GetComponent<Renderer>();
        m_Lightning = GetComponentInChildren<ParticleSystem>();
        m_Animator = transform.parent.GetComponent<Animator>();
        m_Audio = GetComponent<AudioSource>();

        m_RemainingSprintTime = m_MaxSprintTime;
    }

    private void Update()
    {
        if (InputHelper.SprintButtonPressed(m_PlayerNum))
        {
            if (m_HasStamina && m_RemainingSprintTime > 0)
            {
                Sprint();
                m_RemainingSprintTime -= Time.deltaTime;
            }
            else
            {
                // If Player let sprint time fall to zero, full recharge is needed before another sprint
                if (m_HasStamina)
                {
                    SoundManager.Instance.PlayRandom(m_OutOfStaminaSounds);
                }

                m_HasStamina = false;
                Stop();
            }
        }
        else
        {
            Stop();
            if (m_RemainingSprintTime < m_MaxSprintTime)
            {
                m_RemainingSprintTime += Time.deltaTime;
            }
            else
            {
                m_HasStamina = true;
            }
        }
    }

    private void Sprint()
    {
        if (!m_PlayingSprintSound)
        {
            m_Audio.volume = 1f;
            m_Audio.Play();
            m_PlayingSprintSound = true;
        }

        m_PlayerController.IsRunning = true;
        m_Animator.SetBool("IsSprinting", true);

        m_Rend.enabled = true;
        m_Lightning.Play();
    }

    private void Stop()
    {
        SoundManager.Instance.FadeOut(m_Audio);
        m_PlayingSprintSound = false;

        m_PlayerController.IsRunning = false;
        m_Animator.SetBool("IsSprinting", false);

        m_Rend.enabled = false;
        m_Lightning.Stop();
    }

    private void OnDeath()
    {
        Stop();
        Destroy(gameObject);
    }
}