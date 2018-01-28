using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for child objects with Colliders, where parent Behaviour handles collision

public class ChildCollider : MonoBehaviour
{
    // --------------------------------------------------------------

    private bool m_CollidedThisFrame = false;
    
    // --------------------------------------------------------------

    private void LateUpdate()
    {
        m_CollidedThisFrame = false;
    }

    // Delegate collision handling to parent object
    private void OnCollisionEnter(Collision collision)
    {
        if (m_CollidedThisFrame) return;

        m_CollidedThisFrame = true;
        SendMessageUpwards("OnCollisionEnter", collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_CollidedThisFrame) return;

        m_CollidedThisFrame = true;
        SendMessageUpwards("OnTriggerEnter", other);
    }

}
