using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BoxingGlove : MonoBehaviour
{
    // --------------------------------------------------------------

    // How much damage being struck by this glove deals
    [SerializeField] private int m_Damage;

    // Strength of force applied to objects when punched 
    [SerializeField] private float m_ImpactForce;

    // When holding down Fire button, amount of time between consecutive punches
    [SerializeField] private float m_TimeBetweenPunches;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_PunchSounds;

    [SerializeField] private AudioClip[] m_FleshImpactSounds;

    [SerializeField] private AudioClip[] m_HardImpactSounds;

    // --------------------------------------------------------------

    // Player who holds Gloves
    private IPlayer m_Player;

    private Animator m_Animator;

    // For animator to keep track of no. times Player pressed Fire button in a row
    private int m_NumPunches = 0;

    // Flag for whether Fire button held down
    private bool m_IsPunching = false;
    
    // Effect to play at impact position when object hit
    private ParticleSystem[] m_Sparks;

    // Time until gloves disable
    private float m_TimeRemaining;

    // Ref. to each glove's Collider, to be enabled on punch
    private Collider[] m_GloveColliders;

    // --------------------------------------------------------------

    public float TimeRemaining
    {
        get
        {
            return m_TimeRemaining;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Player = GetComponentInParent<IPlayer>();
        m_GloveColliders = GetComponentsInChildren<Collider>();
        m_Sparks = GetComponentsInChildren<ParticleSystem>();
        m_TimeRemaining = GameManager.POWERUP_DURATION;
        SetCollidersActive(false);
    }

    private void Update()
    {
        if (InputHelper.FireButtonPressed(m_Player.PlayerNum()) && !m_IsPunching)
        {
            m_IsPunching = true;
            InvokeRepeating("Punch", 0.00001f, m_TimeBetweenPunches);
        }
        if (InputHelper.FireButtonReleased(m_Player.PlayerNum()))
        {
            m_IsPunching = false;
            CancelInvoke("Punch");
            SetCollidersActive(false);
        }

        m_TimeRemaining -= Time.deltaTime;
        if (m_TimeRemaining <= 0f)
        {
            Deactivate();
        }

    }

    // Trigger Animator to thrust gloves forward
    private void Punch()
    {
        m_NumPunches++;
        SetCollidersActive(true);
        m_Animator.SetTrigger("PunchTrigger");
        m_Animator.SetInteger("NumPunches", m_NumPunches);

        SoundManager.Instance.PlayRandom(m_PunchSounds);

    }

    // Activates or deactivates colliders on each Glove
    private void SetCollidersActive(bool enabled)
    {
        foreach (Collider col in m_GloveColliders)
        {
            col.enabled = enabled;
        }
    }

    // Called from Animator when final punch animation in sequence finishes
    private void ResetNumPunches()
    {
        m_NumPunches = 0;
        m_Animator.SetInteger("NumPunches", m_NumPunches);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If struck a Kinematic Rigidbody, make it temporarily affected by physics
        PhysicsSwitch manualMovedObject = collision.gameObject.GetComponent<PhysicsSwitch>();
        manualMovedObject?.ActivatePhysicsReactions(true, m_Player.GetGameObject());

        // If object struck has Rigidbody, apply force to it
        Rigidbody bodyStruck = collision.gameObject.GetComponent<Rigidbody>();
        if (bodyStruck != null)
        {
            ShowSparks(collision.contacts[0].point);
            bodyStruck.AddForce(m_ImpactForce * transform.forward);
        }

        // If struck object has health, deal damage to it
        IHealth health = collision.gameObject.GetComponent<IHealth>();
        if (health != null)
        {
            health.TakeDamage(m_Damage, m_Player.GetGameObject());
            SoundManager.Instance.PlayRandom(m_FleshImpactSounds);
        }
        else
        {
            SoundManager.Instance.PlayRandom(m_HardImpactSounds);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If struck object has health, deal damage to it
        IHealth health = other.GetComponent<IHealth>();
        if (health != null)
        {
            health.TakeDamage(m_Damage, m_Player.GetGameObject());
            SoundManager.Instance.PlayRandom(m_FleshImpactSounds);
        }
    }

    // Move spark effect to impact position and play
    private void ShowSparks(Vector3 atPosition)
    {
        foreach (ParticleSystem spark in m_Sparks)
        {
            spark.transform.position = atPosition;
            spark.Play();
        }
    }

    // Broadcast from WeaponManager; reset time remaining
    private void OnWeaponReset()
    {
        m_TimeRemaining = GameManager.POWERUP_DURATION;
    }

    // Broadcast from PlayerHealth
    private void OnDeath()
    {
        Deactivate();
    }

    private void Deactivate()
    {
        SendMessageUpwards("DisableWeapon");
        //GetComponentInParent<WeaponManager>().DisableWeapon();
        Destroy(gameObject);
    }
}
