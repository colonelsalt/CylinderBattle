using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
// Launches a powerful charge attack once Player spotted
public class EnemyAttacker : MonoBehaviour, IEnemyBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_AttackSpeed = 20f;

    // Force dealt to any RigidBody in the way of Enemy's attack
    [SerializeField] private float m_ImpactForce = 50f;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip m_AttackSound;

    // --------------------------------------------------------------

    private bool m_IsAttacking = false;

    private ParticleSystem m_VanishSmoke;

    private WaypointPatroller m_Patrol;

    private Animator m_Animator;

    private AudioSource m_Audio;

    private Collider m_Collider;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Patrol = GetComponent<WaypointPatroller>();
        m_Animator = GetComponent<Animator>();
        m_Audio = GetComponent<AudioSource>();
        m_Collider = GetComponent<Collider>();

        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.gameObject.name == "VanishSmoke")
            {
                m_VanishSmoke = ps;
            }
        }
        if (m_VanishSmoke == null) Debug.LogError("Failed to find VanishSmoke particle system for " + name);
    }

    public void Execute()
    {
        if (!m_IsAttacking)
        {
            // Set target 3m in front of Player
            transform.LookAt(m_Patrol.PlayerPos);
            StartCoroutine(Attack(m_Patrol.PlayerPos + (3 * transform.forward)));
        }
    }

    public void Disable() { }

    private IEnumerator Attack(Vector3 target)
    {   
        // Wait a moment, initiate attack animation and sound
        yield return new WaitForSeconds(0.2f);
        m_IsAttacking = true;
        m_Animator.SetBool("isAttacking", true);
        m_Audio.Play();

        // Wait some more
        yield return new WaitForSeconds(0.3f);

        // Lerp move to target 
        Vector3 startPosition = transform.position;
        float distanceToTarget = Vector3.Distance(startPosition, target);
        float startTime = Time.time;
        float distanceCovered = 0f;

        while (distanceToTarget - distanceCovered > 0.1f)
        {
            distanceCovered = (Time.time - startTime) * m_AttackSpeed;
            transform.position = Vector3.Lerp(startPosition, target, distanceCovered / distanceToTarget);
            yield return new WaitForEndOfFrame();
        }

        // Stand still for 0.5 seconds right after attack finished
        float loiterTime = 0f;
        while (loiterTime < 0.5f)
        {
            loiterTime += Time.deltaTime;
            m_Patrol.StandStill();
            yield return new WaitForEndOfFrame();
        }

        m_IsAttacking = false;
        m_Animator.SetBool("isAttacking", false);

        SoundManager.Instance.FadeOut(m_Audio);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If attacking, and struck kinematic Rigidbody, make it temporarily affected by physics
        PhysicsSwitch manualMovedObject = other.GetComponent<PhysicsSwitch>();
        if (manualMovedObject != null && m_IsAttacking)
        {
            manualMovedObject.ActivatePhysicsReactions(true, gameObject);
        }

        // Apply force
        Rigidbody body = other.GetComponent<Rigidbody>();
        if (body != null && m_IsAttacking)
        {
            body.AddForce(transform.forward * m_ImpactForce);
        }

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            // Unless Player is above us (presumably jumping on our head), deal damage
            if (other.bounds.min.y < m_Collider.bounds.center.y)
            {
                player.TakeDamage(1, gameObject);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }

    // Broadcast from EnemyHealth
    private void OnDeath()
    {
        StopAllCoroutines();
        m_VanishSmoke.Play();
    }

    private void OnDestroy()
    {
        OnDeath();
    }
}
