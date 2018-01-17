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

    // Reference to PlayerController this is attached to
    private PlayerController m_Player;

    private Animator m_Animator;

    // For animator to keep track of how many times Player has pressed Fire button in a row
    private int m_NumPunches = 0;

    // Flag to keep track of whether Fire button held down
    private bool m_IsPunching = false;

    private ParticleSystem[] m_Sparks;

    private float m_TimeRemaining;

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
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;
        m_Animator = GetComponent<Animator>();
        m_Player = GetComponentInParent<PlayerController>();
        m_GloveColliders = GetComponentsInChildren<Collider>();
        m_Sparks = GetComponentsInChildren<ParticleSystem>();
        m_TimeRemaining = GameManager.POWERUP_DURATION;
        SetCollidersActive(false);
    }

    private void Update()
    {
        if (InputHelper.FireButtonPressed(m_Player.PlayerNum) && !m_IsPunching)
        {
            m_IsPunching = true;
            InvokeRepeating("Punch", 0.00001f, m_TimeBetweenPunches);
        }
        if (InputHelper.FireButtonReleased(m_Player.PlayerNum))
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
        // Do nothing if we just collided with ourselves
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            if (player.PlayerNum == m_Player.PlayerNum) return;
        }
        
        // If struck a Kinematic Rigidbody, make it temporarily affected by physics
        PhysicsSwitch manualMovedObject = collision.gameObject.GetComponent<PhysicsSwitch>();
        if (manualMovedObject != null)
        {
            manualMovedObject.ActivatePhysicsReactions(true);
        }

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
            health.TakeDamage(m_Damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If struck object has health, deal damage to it
        IHealth health = other.GetComponent<IHealth>();
        if (health != null)
        {
            health.TakeDamage(m_Damage);
        }
    }

    private void ShowSparks(Vector3 atPosition)
    {
        foreach (ParticleSystem spark in m_Sparks)
        {
            spark.transform.position = atPosition;
            spark.Play();
        }
    }

    private void OnWeaponReset()
    {
        m_TimeRemaining = GameManager.POWERUP_DURATION;
    }

    private void OnPlayerDeath(int playerNum)
    {
        if (playerNum == m_Player.PlayerNum)
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
        m_Player.GetComponent<WeaponManager>().DisableWeapon();
        Destroy(gameObject);
    }

}
