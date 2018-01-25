using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_TimeBeforeDrop = 2f;

    [SerializeField] private float m_TimeBeforeRespawn = 6f;

    // --------------------------------------------------------------

    private Animator m_Anim;

    private bool m_Activated = false;

    private bool m_IsDropping = false;

    private float m_RemainingFloatTime;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (m_Activated && !m_IsDropping)
        {
            m_RemainingFloatTime -= Time.deltaTime;
            if (m_RemainingFloatTime <= 0f)
            {
                Drop();
            }
        }
    }

    private void Drop()
    {
        m_IsDropping = true;
        m_Anim.SetTrigger("dropTrigger");
        Invoke("Reset", m_TimeBeforeRespawn);
    }

    private void Reset()
    {
        m_Anim.SetTrigger("respawnTrigger");
        m_Activated = m_IsDropping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPlayer>() != null && !m_Activated)
        { 
            m_RemainingFloatTime = m_TimeBeforeDrop;
            m_Anim.SetTrigger("activationTrigger");
            m_Activated = true;
        }
    }


}
