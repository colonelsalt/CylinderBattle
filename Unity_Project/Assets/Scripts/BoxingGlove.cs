using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BoxingGlove : MonoBehaviour
{
    // --------------------------------------------------------------

    // How much damage being struck by this glove deals
    [SerializeField] private int m_Damage;

    [SerializeField] private float m_TimeBetweenPunches;

    // --------------------------------------------------------------

    // Reference to PlayerController this is attached to
    private PlayerController m_Player;

    private Animator m_Animator;

    private int m_NumPunches = 0;

    private bool m_PunchedThisFrame = false;


    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Player = GetComponentInParent<PlayerController>();
    }
    
    private void Update()
    {
        if (InputHelper.FireButtonPressed(m_Player.PlayerNum) && !m_PunchedThisFrame)
        {
            m_PunchedThisFrame = true;
            InvokeRepeating("Punch", 0.00001f, m_TimeBetweenPunches);
        }
        if (InputHelper.FireButtonReleased(m_Player.PlayerNum))
        {
            m_PunchedThisFrame = false;
            CancelInvoke("Punch");
        }
    }

    // Trigger Animator to thrust gloves forward
    private void Punch()
    {
        m_NumPunches++;
        m_Animator.SetTrigger("PunchTrigger");
        m_Animator.SetInteger("NumPunches", m_NumPunches);
    }

    // Called from the animator when final punch animation in sequence finishes
    private void ResetNumPunches()
    {
        m_NumPunches = 0;
        m_Animator.SetInteger("NumPunches", m_NumPunches);
    }

    // When either glove strikes something, damage it
    private void OnCollisionEnter(Collision collision)
    {
        Health objectStruck = collision.gameObject.GetComponent<Health>();
        if (objectStruck != null)
        {
            objectStruck.TakeDamage(m_Damage);
        }
    }

}
