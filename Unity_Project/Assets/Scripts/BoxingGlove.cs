using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BoxingGlove : MonoBehaviour
{
    // --------------------------------------------------------------

    // How much damage being struck by this glove deals
    [SerializeField] private int m_Damage;

    // --------------------------------------------------------------

    // Reference to PlayerController this is attached to
    private PlayerController m_Player;

    private Animator m_Animator;

    private int m_NumPunches = 0;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Player = GetComponentInParent<PlayerController>();
    }
    
    private void Update()
    {
        if (InputHelper.FireButtonPressed(m_Player.PlayerNum))
        {
            Punch();
        }
    }

    // Trigger Animator to thrust gloves forward
    private void Punch()
    {
        m_Animator.SetTrigger("PunchTrigger");
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
