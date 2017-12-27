using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    // --------------------------------------------------------------

    // After striking an Enemy/Player/WeaponBlock, how strongly does Player bounce up
    [SerializeField] private float m_BounceHeight;

    // --------------------------------------------------------------

    private PlayerController m_Player;

    private Rigidbody m_Body;

    // --------------------------------------------------------------


    private void Awake()
    {
        m_Player = GetComponentInParent<PlayerController>();
        m_Body = GetComponentInParent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerController otherPlayer = other.GetComponent<PlayerController>();
        if (otherPlayer != null)
        {
            if (otherPlayer.PlayerNum != m_Player.PlayerNum)
            {
                otherPlayer.GetComponent<Health>().TakeDamage(1);
                Bounce();
            }
        }

        WeaponBlock weaponBlock = other.GetComponent<WeaponBlock>();
        if (weaponBlock != null)
        {
            weaponBlock.Break(m_Player.GetComponent<WeaponManager>());
            Bounce();
        }
    }

    private void Bounce()
    {
        Vector3 bounceDirection = new Vector3(Random.Range(-0.8f, 0.8f), 1f, Random.Range(-0.8f, 0.8f)).normalized;
        m_Player.Jump(m_BounceHeight);
    }
}
