using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : EnemyBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_AttackSpeed = 20f;

    [SerializeField] private float m_ImpactForce = 50f;

    // --------------------------------------------------------------

    private bool m_IsAttacking = false;

    private WaypointPatroller m_Patrol;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Patrol = GetComponent<WaypointPatroller>();
    }

    public override void Execute()
    {
        if (!m_IsAttacking)
        {
            StartCoroutine(Attack(m_Patrol.PlayerPos + (2 * transform.forward)));
        }
    }

    private IEnumerator Attack(Vector3 target)
    {
        m_IsAttacking = true;

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

        m_IsAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (m_IsAttacking)
            {
                player.ActivatePhysicsReactions();
                player.GetComponent<Rigidbody>().AddForce(transform.forward * m_ImpactForce);
            }
            player.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
