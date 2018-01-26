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

    private Renderer m_Rend;

    private Collider[] m_Colliders;

    private Vector3 m_StartingPosition;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_Rend = GetComponentInChildren<Renderer>();
        m_Colliders = GetComponentsInChildren<Collider>();

        m_StartingPosition = transform.position;
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
        StartCoroutine(Fade(false));
        Invoke("Reset", m_TimeBeforeRespawn);
    }

    private void Reset()
    {
        transform.position = m_StartingPosition;

        StartCoroutine(Fade(true));
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

    // Manual fade to prevent Mechanim system taking control of Renderer
    private IEnumerator Fade(bool fadeIn)
    {
        if (fadeIn) m_Rend.enabled = true;

        float remainingFadeTime = 0.4f;
        while (remainingFadeTime > 0f)
        {
            Color colour = m_Rend.material.color;
            if (fadeIn)
            {
                colour.a = 1f - remainingFadeTime / 0.4f;
            }
            else
            {
                colour.a = remainingFadeTime / 0.4f;
            }
            
            m_Rend.material.color = colour;

            remainingFadeTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        foreach (Collider col in m_Colliders)
        {
            col.enabled = fadeIn;
        }
        if (!fadeIn) m_Rend.enabled = false;
    }


}
