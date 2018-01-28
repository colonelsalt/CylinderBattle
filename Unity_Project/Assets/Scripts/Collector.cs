//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // --------------------------------------------------------------

    private int m_PlayerNum;

    // Number of Pluses collected
    private int m_NumPluses = 0;

    // Number of Pis collected
    private int m_NumPis = 0;

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
        PlayerHealth.OnPlayerDamaged += OnPlayerDamaged;
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;

        m_PlayerNum = GetComponent<IPlayer>().PlayerNum();
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
                OnPlusPickup(m_PlayerNum);
                m_PowerupManager.CheckForPlusBonus(m_NumPluses);
                break;
            case Collectible.Type.PI:
                m_NumPis++;
                OnPiPickup(m_PlayerNum);
                break;
        }
    }

    private void OnPlayerDamaged(int playerNum, GameObject attacker)
    {
        if (playerNum == m_PlayerNum)
        {
            CheckForPiDrop();
        }
    }
    
    private void OnPlayerDeath(int playerNum, GameObject attacker)
    {
        if (playerNum == m_PlayerNum)
        {
            // Reset Plus count when Player dies
            m_NumPluses = 0;
            CheckForPiDrop();
        }
       
    }

    // If this Player died or took damage, drop one of their Pis
    private void CheckForPiDrop()
    {
        if (m_NumPis > 0 && m_TimeSinceLastPiDrop >= TIME_BETWEEN_PI_DROPS)
        {
            DropPi();
        }
    }

    // Instantiate Pi in random (upwards) direction
    private void DropPi()
    {
        m_TimeSinceLastPiDrop = 0f;

        m_NumPis--;

        Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized; 
        GameObject pi = Instantiate(m_PiPrefab, transform.position + (2f * transform.up), Quaternion.identity) as GameObject;
        pi.GetComponent<Rigidbody>().AddForce(dropDirection * m_DropForce);
        pi.GetComponent<Collectible>().AssignOwner(gameObject);

        OnPiDrop(m_PlayerNum);
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDamaged -= OnPlayerDamaged;
        PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
    }
}
