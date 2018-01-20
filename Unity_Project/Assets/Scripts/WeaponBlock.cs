using System.Collections;
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

    [SerializeField] private AudioClip[] m_SpawnSounds;

    // --------------------------------------------------------------

    private Animator m_Animator;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SoundManager.Instance.PlayRandom(m_SpawnSounds);
    }

    private static Weapon RandomWeapon()
    {
        return (Weapon)Random.Range(0, 4);
    }

    public void Break(WeaponManager brokenBy)
    {
        brokenBy.PickupWeapon(RandomWeapon());

        m_Animator.SetTrigger("BreakTrigger");
        
        Destroy(gameObject, 0.3f);
    }
}
