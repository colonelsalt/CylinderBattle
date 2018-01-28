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

    // How long after Collectible has spawned until it can be collected (useful if spawned right next to Player)
    [SerializeField] private float m_CollectionWaitTime = 0f;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_SpawnSounds;

    [SerializeField] private AudioClip[] m_PickupSounds;

    // --------------------------------------------------------------

    private Animator m_Animator;

    // Flag to prevent multiple pickups
    private bool m_HasBeenCollected = false;

    // If Collectible dropped, object it was dropped by
    private GameObject m_Owner;

    // --------------------------------------------------------------

    // Marked virtual to allow AchievementCollectible to override
    protected virtual void Awake()
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

    // Store ref. to object who dropped Collectible (currently only used when Player drops Pi)
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
            // Indicate to Collector that Collectible picked up
            collector.PickupCollectible(m_Type);

            m_HasBeenCollected = true;

            if (m_Owner != null)
            {
                OnDroppedItemPickup(m_Owner, other.gameObject);
            }

            Vanish();
        }
    }

    // Marked virtual to allow AchievementCollectible to override
    protected virtual void Vanish()
    {
        SoundManager.Instance.PlayRandom(m_PickupSounds);
        m_Animator.SetTrigger("VanishTrigger");
        Destroy(gameObject, 0.5f);
    }
}
