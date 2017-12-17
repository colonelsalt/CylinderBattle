using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // --------------------------------------------------------------
    [SerializeField] private float m_ExplosionTime;

    private float m_timePassed = 0f;
    // --------------------------------------------------------------

    private void Update()
    {
        m_timePassed += Time.deltaTime;
        if (m_timePassed >= m_ExplosionTime)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Debug.Log("Bomb exploded!");
        Destroy(gameObject);
    }
}
