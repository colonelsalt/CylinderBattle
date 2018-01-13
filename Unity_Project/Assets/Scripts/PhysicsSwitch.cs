using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Makes a Kinematic Rigidbody temporarily non-kinematic (to react to outside forces in natural way)
[RequireComponent(typeof(Rigidbody))]
public class PhysicsSwitch : MonoBehaviour
{
    // --------------------------------------------------------------

    // How long object remains under influence of automatic physics
    [SerializeField] private float m_AutoPhysicsTime = 1f;

    // --------------------------------------------------------------

    private Rigidbody m_Body;

    private NavMeshAgent m_NavMeshAgent;

    private CharacterController m_CharacterController;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_CharacterController = GetComponent<CharacterController>();
    }

    public void ActivatePhysicsReactions(bool reactivateAfter)
    {
        // Temporarily remove manual position control from NavMeshAgent/CharacterController
        if (m_NavMeshAgent != null)
        {
            m_NavMeshAgent.enabled = false;
        }
        else if (m_CharacterController != null)
        {
            m_CharacterController.enabled = false;
        }

        m_Body.isKinematic = false;

        if (reactivateAfter)
        {
            Invoke("DeactivatePhysicsReactions", m_AutoPhysicsTime);
        }
    }

    private void DeactivatePhysicsReactions()
    {
        if (m_NavMeshAgent != null)
        {
            m_NavMeshAgent.enabled = true;
        }
        else if (m_CharacterController != null)
        {
            m_CharacterController.enabled = true;
        }

        m_Body.isKinematic = true;
    }


}
