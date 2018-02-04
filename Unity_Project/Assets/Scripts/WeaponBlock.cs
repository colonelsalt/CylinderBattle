﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    BOMB = 0,
    GUN = 1,
    BOXING_GLOVES = 2,
    PORTAL_GUN = 3,
    LIGHTNING = 4
}

public class WeaponBlock : MonoBehaviour
{
    // --------------------------------------------------------------

    // Renderer material doesn't cast shadow, so spawn blob shadow prefab to emulate it
    [SerializeField] private GameObject m_ShadowPrefab;

    // --------------------------------------------------------------

    private Animator m_Animator;

    private GameObject m_Shadow;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Shadow = Instantiate(m_ShadowPrefab) as GameObject;
        m_Shadow.GetComponent<Projector>().enabled = true;
        m_Shadow.GetComponent<FollowObject>().SetTarget(transform);
    }

    private static Weapon RandomWeapon()
    {
        return (Weapon)Random.Range(0, 4);
    }

    // Called by PlayerFeet when triggered against from above
    public void Break(WeaponManager brokenBy)
    {
        // Send random Weapon to WeaponManager who broke Block
        brokenBy.PickupWeapon(RandomWeapon());

        m_Animator.SetTrigger("BreakTrigger");
        
        Destroy(gameObject, 0.3f);
        Destroy(m_Shadow);
    }

    private void OnDestroy()
    {
        if (m_Shadow != null)
        {
            Destroy(m_Shadow);
        }
    }
}
