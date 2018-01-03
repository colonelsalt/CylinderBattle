//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerHealth))]
public class Collector : MonoBehaviour
{
    // --------------------------------------------------------------

    // Prefab to spawn when Player takes damage and drops a Pi
    [SerializeField] private GameObject m_PiPrefab;

    // When Player drops a Pi, how much force is it pushed away by
    [SerializeField] private float m_DropForce;

    // --------------------------------------------------------------

    // Events
    public delegate void CollectibleEvent(int playerNum);
    public static event CollectibleEvent OnPiPickup;
    public static event CollectibleEvent OnPiDrop;
    public static event CollectibleEvent OnPlusPickup;
    public static event CollectibleEvent OnAllPisCollected;

    // --------------------------------------------------------------
    
    // Number of Pluses collected
    private int m_NumPluses = 0;

    // Number of Pis collected
    private int m_NumPis = 0;

    private PlayerController m_Player;

    private PowerupManager m_PowerupManager;

    private const float TIME_BETWEEN_PI_DROPS = 1f;

    private float m_TimeSinceLastPiDrop = 0f;

    // --------------------------------------------------------------

    public int NumPis
    {
        get
        {
            return m_NumPis;
        }
    }

    public int NumPluses
    {
        get
        {
            return m_NumPluses;
        }
    }

    // --------------------------------------------------------------


    private void Awake()
    {
        PlayerHealth.OnPlayerRespawn += OnResetPlusCount;
        PlayerHealth.OnPlayerDeath += OnCheckForPiDrop;
        PlayerHealth.OnPlayerDamaged += OnCheckForPiDrop;

        m_Player = GetComponent<PlayerController>();
        m_PowerupManager = GetComponent<PowerupManager>();
    }

    private void Update()
    {
        if (m_TimeSinceLastPiDrop < TIME_BETWEEN_PI_DROPS)
        {
            m_TimeSinceLastPiDrop += Time.deltaTime;
        }
    }

    public void PickupCollectible(Collectible.Type type)
    {
        switch (type)
        {
            case Collectible.Type.PLUS:
                m_NumPluses++;
                OnPlusPickup(m_Player.PlayerNum);
                m_PowerupManager.CheckForPlusBonus(m_NumPluses);
                break;
            case Collectible.Type.PI:
                m_NumPis++;
                OnPiPickup(m_Player.PlayerNum);
                if (m_NumPis >= GameManager.MAX_NUM_PIS)
                {
                    OnAllPisCollected(m_Player.PlayerNum);
                }
                break;
        }
    }

    // Reset Plus count when Player dies
    private void OnResetPlusCount(int playerNum)
    {
        if (m_Player.PlayerNum == playerNum)
        {
            m_NumPluses = 0;
        }
    }

    // If this Player died or took damage, drop one of their Pis
    private void OnCheckForPiDrop(int playerNum)
    {
        if (playerNum == m_Player.PlayerNum && m_NumPis > 0)
        {
            DropPi();
        }
    }

    // Instantiate Pi in random (upwards) direction
    private void DropPi()
    {
        if (m_TimeSinceLastPiDrop < TIME_BETWEEN_PI_DROPS) return;
        m_TimeSinceLastPiDrop = 0f;

        m_NumPis--;

        Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized; 
        GameObject pi = Instantiate(m_PiPrefab, transform.position + (2f * transform.up), Quaternion.identity) as GameObject;
        pi.GetComponent<Rigidbody>().AddForce(dropDirection * m_DropForce);

        OnPiDrop(m_Player.PlayerNum);
    }
}
