using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LightningSprint : MonoBehaviour
{
    // How long Player can sprint for before recharge needed
    [SerializeField] private float m_MaxSprintTime;

    // --------------------------------------------------------------

    // Sounds
    [SerializeField] private AudioClip[] m_OutOfStaminaSounds;

    // --------------------------------------------------------------

    private PlayerController m_Player;

    private Renderer m_Rend;

    private ParticleSystem m_Lightning;

    private Animator m_Animator;

    private AudioSource m_Audio;

    private float m_RemainingSprintTime;

    private bool m_HasStamina = true;

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
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;

        m_Player = GetComponentInParent<PlayerController>();
        m_Rend = GetComponent<Renderer>();
        m_Lightning = GetComponentInChildren<ParticleSystem>();
        m_Animator = transform.parent.GetComponent<Animator>();
        m_Audio = GetComponent<AudioSource>();

        m_RemainingSprintTime = m_MaxSprintTime;
    }

    private void Update()
    {
        if (InputHelper.SprintButtonPressed(m_Player.PlayerNum))
        {
            if (m_HasStamina && m_RemainingSprintTime > 0)
            {
                Sprint();
                m_RemainingSprintTime -= Time.deltaTime;
            }
            else
            {
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
        
        m_Player.IsRunning = true;
        m_Animator.SetBool("IsSprinting", true);

        m_Rend.enabled = true;
        m_Lightning.Play();
    }

    private void Stop()
    {
        SoundManager.Instance.FadeOut(m_Audio);
        m_PlayingSprintSound = false;

        m_Player.IsRunning = false;
        m_Animator.SetBool("IsSprinting", false);

        m_Rend.enabled = false;
        m_Lightning.Stop();
    }

    private void OnPlayerDeath(int playerNum)
    {
        if (playerNum == m_Player.PlayerNum)
        {
            Stop();
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
    }

}
