using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Separate component attached to child of Player to detect if Player jumped on something's head
public class PlayerFeet : MonoBehaviour
{
    // --------------------------------------------------------------

    // After striking an Enemy/Player/WeaponBlock, how strongly does Player bounce up
    [SerializeField] private float m_BounceHeight;

    // Wiggle room for determining if Player is on top of object
    [SerializeField] private float m_Epsilon = 0.1f;

    // --------------------------------------------------------------

    private PlayerController m_PlayerController;

    private Collider m_Collider;

    // --------------------------------------------------------------


    private void Awake()
    {
        m_PlayerController = GetComponentInParent<PlayerController>();
        m_Collider = transform.parent.GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        // If Player is on top of object
        if (Mathf.Abs(m_Collider.bounds.min.y - other.bounds.max.y) <= m_Epsilon)
        {
            // Damage other Health object
            IHealth otherHealth = other.GetComponent<IHealth>();
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(1, m_PlayerController.gameObject);
                Bounce();
            }
            
            // Break Weapon Block
            WeaponBlock weaponBlock = other.GetComponent<WeaponBlock>();
            if (weaponBlock != null)
            {
                weaponBlock.Break(m_PlayerController.GetComponent<WeaponManager>());
                Bounce();
            }
        }
    }

    private void Bounce()
    {
        m_PlayerController.Jump(m_BounceHeight);
    }
}
