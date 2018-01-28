using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Instantiated by PortalGun on Portal fire
public class PortalProjectile : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_Speed;

    [SerializeField] private GameObject m_PortalPrefab;

    // --------------------------------------------------------------

    // Called when PortalGun fired to set destination
    public void SetTarget(Vector3 target, Quaternion portalRotation)
    {
        StartCoroutine(MoveToTarget(target, portalRotation));
    }

    // Lerp move to target aimed at by PortalGun
    private IEnumerator MoveToTarget(Vector3 target, Quaternion portalRotation)
    {
        Vector3 startPosition = transform.position;
        float distanceToTarget = Vector3.Distance(startPosition, target);
        float startTime = Time.time;
        float distanceCovered = 0f;

        while (distanceCovered < distanceToTarget)
        {
            distanceCovered = (Time.time - startTime) * m_Speed;
            transform.position = Vector3.Lerp(startPosition, target, distanceCovered / distanceToTarget);
            yield return new WaitForEndOfFrame();
        }

        Instantiate(m_PortalPrefab, target, portalRotation);
        Destroy(gameObject);
    }
}
