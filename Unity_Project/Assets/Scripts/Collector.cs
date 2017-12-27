//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
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

    // --------------------------------------------------------------


    private void Awake()
    {
        PlayerController.OnPlayerRespawn += OnResetPlusCount;
        Health.OnPlayerHealthChange += OnCheckForPiDrop;

        m_Player = GetComponent<PlayerController>();
    }

    public void PickupCollectible(Collectible.Type type)
    {
        switch (type)
        {
            case Collectible.Type.PLUS:
                OnPlusPickup(m_Player.PlayerNum);
                m_NumPluses++;
                CheckForPlusBonus();
                break;
            case Collectible.Type.PI:
                OnPiPickup(m_Player.PlayerNum);
                m_NumPis++;
                if (m_NumPis >= GameManager.MAX_NUM_PIS)
                {
                    OnAllPisCollected(m_Player.PlayerNum);
                }
                break;
        }
    }

    private void OnResetPlusCount(int playerNum)
    {
        if (m_Player.PlayerNum == playerNum)
        {
            m_NumPluses = 0;
        }
    }

    // If this Player took damage, drop one of their Pis
    private void OnCheckForPiDrop(int playerNum, int healthChange)
    {
        if (playerNum != m_Player.PlayerNum) return;

        if (healthChange < 0 && m_NumPis > 0)
        {
            DropPi();
        }
    }

    // Instantiate Pi in random (upwards) direction
    private void DropPi()
    {
        m_NumPis--;

        Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized; 
        GameObject pi = Instantiate(m_PiPrefab, transform.position + (2f * transform.up), Quaternion.identity) as GameObject;
        pi.GetComponent<Rigidbody>().AddForce(dropDirection * m_DropForce);

        OnPiDrop(m_Player.PlayerNum);
    }

    private void CheckForPlusBonus()
    {
        // TODO
    }
}
