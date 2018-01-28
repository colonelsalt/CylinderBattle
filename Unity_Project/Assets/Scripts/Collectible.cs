using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum Type { PLUS, PI };

    // --------------------------------------------------------------

    public delegate void DroppedItemEvent(GameObject owner, GameObject receiver);
    public static event DroppedItemEvent OnDroppedItemPickup;

    // --------------------------------------------------------------

    [SerializeField] private Type m_Type;

    // How long after Collectible has spawned until it can be collected
    [SerializeField] private float m_CollectionWaitTime = 0f;

    [SerializeField] private bool m_IsAchievementSpecial = false;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_SpawnSounds;

    [SerializeField] private AudioClip[] m_PickupSounds;

    // --------------------------------------------------------------

    private Animator m_Animator;

    private bool m_HasBeenCollected = false;

    private GameObject m_Owner;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SoundManager.Instance.PlayRandom(m_SpawnSounds);
    }

    private void Update()
    {
        if (m_CollectionWaitTime > 0)
        {
            m_CollectionWaitTime -= Time.deltaTime;
        }
    }

    public void AssignOwner(GameObject owner)
    {
        m_Owner = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_HasBeenCollected) return;

        Collector collector = other.GetComponent<Collector>();
        if (collector != null && m_CollectionWaitTime <= 0)
        {
            collector.PickupCollectible(m_Type);
            m_HasBeenCollected = true;

            if (m_Owner != null)
            {
                OnDroppedItemPickup(m_Owner, other.gameObject);
            }

            Vanish();
        }
    }

    private void Vanish()
    {
        if (m_IsAchievementSpecial)
        {
            BroadcastMessage("OnVanish");
        }
        SoundManager.Instance.PlayRandom(m_PickupSounds);
        m_Animator.SetTrigger("VanishTrigger");
        Destroy(gameObject, 0.5f);
    }
}
