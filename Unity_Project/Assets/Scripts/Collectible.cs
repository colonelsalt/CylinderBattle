using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public enum Type { PLUS, PI };

    // --------------------------------------------------------------

    [SerializeField] private Type type;

    // How long after Collectible has spawned until it can be collected
    [SerializeField] private float m_CollectionWaitTime = 0f;

    // --------------------------------------------------------------

    // Events
    public delegate void CollectiblePickup(Type t, int playerNum);
    public static event CollectiblePickup OnCollectiblePickup;

    // --------------------------------------------------------------

    private void Update()
    {
        if (m_CollectionWaitTime > 0)
        {
            m_CollectionWaitTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController capturedBy = other.GetComponent<PlayerController>();
        if (capturedBy != null && m_CollectionWaitTime <= 0)
        {
            OnCollectiblePickup(type, capturedBy.PlayerNum);
            Vanish();
        }
    }

    private void Vanish()
    {
        // TODO: play vanish animation
        Destroy(gameObject);
    }
}
