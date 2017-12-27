﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public enum Type { PLUS, PI };

    // --------------------------------------------------------------

    [SerializeField] private Type m_Type;

    // How long after Collectible has spawned until it can be collected
    [SerializeField] private float m_CollectionWaitTime = 0f;

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
        Collector collector = other.GetComponent<Collector>();
        if (collector != null && m_CollectionWaitTime <= 0)
        {
            collector.PickupCollectible(m_Type);
            Vanish();
        }
    }

    private void Vanish()
    {
        // TODO: play vanish animation
        Destroy(gameObject);
    }
}
