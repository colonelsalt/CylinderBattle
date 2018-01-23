using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Keeps track of hidden Player stats (
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

    private int m_Distance = 0;

    // --------------------------------------------------------------

    public static int PlayerWinner
    {
        get
        {
            return m_PlayerWinner;
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
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;

        DontDestroyOnLoad(gameObject);
    }

    private void OnGameOver(int playerNum)
    {
        m_PlayerWinner = playerNum;
        Collector.OnAllPisCollected -= OnGameOver;
        GameManager.OnGameStart -= OnGameStart;
    }

    private void OnPlayerDeath(int playerNum)
    {
        if (playerNum == m_Score.PlayerNum)
        {
            m_Deaths++;
        }
    }

    public void Disable()
    {
        Destroy(gameObject);
    }

}
