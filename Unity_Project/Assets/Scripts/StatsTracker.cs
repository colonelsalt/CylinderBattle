using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Keeps track of Player stats
public class StatsTracker : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private ScoreRetriever m_Score;

    // --------------------------------------------------------------

    // TODO: Set this to 0 before release
    private static int m_PlayerWinner = 2;

    // --------------------------------------------------------------

    private int m_Pis = 0;

    private int m_TotalPluses = 0;

    private int m_PlayerKills = 0;

    private int m_EnemyKills = 0;

    private int m_Deaths = 0;

    private float m_Distance = 0;

    private Vector3 m_LastPlayerPos;

    // --------------------------------------------------------------

    public static int PlayerWinner
    {
        get
        {
            return m_PlayerWinner;
        }
    }

    public int NumDeaths
    {
        get
        {
            return m_Deaths;
        }
    }

    public int NumPis
    {
        get
        {
            return m_Pis;
        }
    }

    public int NumTotalPluses
    {
        get
        {
            return m_TotalPluses;
        }
    }

    public int DistanceCovered
    {
        get
        {
            return (int)m_Distance;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        GameManager.OnGameStart += OnGameStart;
    }

    private void OnGameStart()
    {
        Collector.OnAllPisCollected += OnGameOver;
        Collector.OnPlusPickup += OnPlusPickup;
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;

        m_LastPlayerPos = m_Score.PlayerPosition;
        DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate()
    {
        if (m_Score.IsPlayerAlive)
        {
            m_Distance += Vector3.Distance(m_LastPlayerPos, m_Score.PlayerPosition);
            m_LastPlayerPos = m_Score.PlayerPosition;
        }
    }

    private void OnGameOver(int playerNum)
    {
        m_PlayerWinner = playerNum;

        RemoveListeners();

        m_Pis = m_Score.NumPis;
    }

    private void OnPlusPickup(int playerNum)
    {
        if (playerNum == m_Score.PlayerNum)
        {
            m_TotalPluses++;
        }
    }

    private void OnPlayerDeath(int playerNum)
    {
        if (playerNum == m_Score.PlayerNum)
        {
            m_Deaths++;
        }
    }

    private void RemoveListeners()
    {
        Collector.OnAllPisCollected -= OnGameOver;
        GameManager.OnGameStart -= OnGameStart;
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

}
