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

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Player = GetComponentInParent<PlayerController>();
    }

    private void Start()
    {
        Invoke("Deactivate", GameManager.POWERUP_DURATION);
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
        foreach (SphereCollider col in GetComponentsInChildren<SphereCollider>())
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
        PlayerController playerStruck = collision.gameObject.GetComponent<PlayerController>();
        if (playerStruck != null)
        {
            // Do nothing if we just collided with Player wearing gloves
            if (playerStruck.PlayerNum == m_Player.PlayerNum)
            {
                return;
            } 
            else
            {
                // If struck other Player, activate their reaction to physics
                playerStruck.ActivatePhysicsReactions();
            }
        }

        // If object struck has a Rigidbody, apply force to it
        Rigidbody bodyStruck = collision.gameObject.GetComponent<Rigidbody>();
        if (bodyStruck != null)
        {
            bodyStruck.AddForce(m_ImpactForce * transform.forward);
        }

        // If struck object has health, deal damage to it
        PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(m_Damage);
        }
    }

    private void Deactivate()
    {
        m_Player.GetComponent<WeaponManager>().DisablePowerup();
        Destroy(gameObject);
    }

}
