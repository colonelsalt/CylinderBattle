using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : EnemyBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_AttackSpeed = 20f;

    [SerializeField] private float m_ImpactForce = 50f;

    // --------------------------------------------------------------

    private bool m_IsAttacking = false;

    private ParticleSystem m_VanishSmoke;

    private WaypointPatroller m_Patrol;

    private Animator m_Animator;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Patrol = GetComponent<WaypointPatroller>();
        m_Animator = GetComponent<Animator>();

        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.gameObject.name == "VanishSmoke")
            {
                m_VanishSmoke = ps;
            }
        }
        if (m_VanishSmoke == null) Debug.LogError("Failed to find VanishSmoke particle system for " + name);
    }

    public override void Execute()
    {
        if (!m_IsAttacking)
        {
            StartCoroutine(Attack(m_Patrol.PlayerPos + (3 * transform.forward)));
        }
    }

    private IEnumerator Attack(Vector3 target)
    {
        yield return new WaitForSeconds(0.2f);
        m_IsAttacking = true;
        m_Animator.SetBool("isAttacking", true);
        transform.LookAt(target);

        yield return new WaitForSeconds(0.3f);

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
    }

    private void OnTriggerEnter(Collider other)
    {
        // If struck kinematic Rigidbody, make it temporarily affected by physics
        PhysicsSwitch manualMovedObject = other.GetComponent<PhysicsSwitch>();
        if (manualMovedObject != null && m_IsAttacking)
        {
            manualMovedObject.ActivatePhysicsReactions(true);
        }

        Rigidbody body = other.GetComponent<Rigidbody>();
        if (body != null && m_IsAttacking)
        {
            body.AddForce(transform.forward * m_ImpactForce);
        }


        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(1);
        }
    }

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
