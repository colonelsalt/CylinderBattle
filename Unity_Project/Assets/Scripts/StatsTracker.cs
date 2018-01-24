using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Keeps track of Player stats
public class StatsTracker : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private PlayerStats m_Player;

    // How long after Player X knocks over Player Y to consider Y's death as X's kill
    [SerializeField] private float m_KnockBackTime = 3f;

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

    private float m_TimeSinceKnockOver;

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

    public int NumPlayerKills
    {
        get
        {
            return m_PlayerKills;
        }
    }

    public int NumEnemyKills
    {
        get
        {
            return m_EnemyKills;
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
        EnemyHealth.OnEnemyDeath += OnEnemyDeath;
        PhysicsSwitch.OnObjectKnockedBack += OnObjectKnockedBack;

        m_LastPlayerPos = m_Player.Position();
        DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate()
    {
        // If Player alive, update their "distance covered" measure (don't count respawn movement)
        if (m_Player.IsPlayerAlive)
        {
            m_Distance += Vector3.Distance(m_LastPlayerPos, m_Player.Position());
            m_LastPlayerPos = m_Player.Position();
        }

        if (m_TimeSinceKnockOver < m_KnockBackTime)
        {
            m_TimeSinceKnockOver += Time.deltaTime;
        }
    }

    private void OnGameOver(int playerNum)
    {
        m_PlayerWinner = playerNum;

        RemoveListeners();

        m_Pis = m_Player.NumPis;
    }

    private void OnPlusPickup(int playerNum)
    {
        if (playerNum == m_Player.PlayerNum())
        {
            m_TotalPluses++;
        }
    }

    private void OnPlayerDeath(int playerNum, GameObject killer)
    {
        if (playerNum == m_Player.PlayerNum())
        {
            m_Deaths++;

            if (killer.GetComponent<DeathTrigger>() != null)
            {
                // Check if Player out of bounds of LevelMesh
            }

        }
        else
        {
            // If other Player was killed by this Player, increment PlayerKills count
            IPlayer playerAttacker = killer.GetComponent<IPlayer>();

            if (playerAttacker != null)
            {
                if (playerAttacker.PlayerNum() == m_Player.PlayerNum())
                {
                    m_PlayerKills++;
                }
            }

            // If other player fell in lava, check if this counts as our kill
            else if (killer.GetComponent<DeathTrigger>() != null)
            {
                if (m_TimeSinceKnockOver < m_KnockBackTime)
                {
                    m_PlayerKills++;
                }
            }
        }
    }

    private void OnEnemyDeath(GameObject killer)
    {
        IPlayer playerAttacker = killer.GetComponent<IPlayer>();
        if (playerAttacker != null)
        {
            if (playerAttacker.PlayerNum() == m_Player.PlayerNum())
            {
                m_EnemyKills++;
            }
        }
    }

    private void OnObjectKnockedBack(GameObject victim, GameObject attacker)
    {
        IPlayer playerAttacker = attacker.GetComponent<IPlayer>();
        if (playerAttacker != null)
        {
            if (playerAttacker.PlayerNum() == m_Player.PlayerNum())
            {
                if (victim.GetComponent<IPlayer>() != null)
                {
                    m_TimeSinceKnockOver = m_KnockBackTime;
                }
            }
        }
    }

    private void RemoveListeners()
    {
        Collector.OnAllPisCollected -= OnGameOver;
        Collector.OnPlusPickup -= OnPlusPickup;
        GameManager.OnGameStart -= OnGameStart;
        PhysicsSwitch.OnObjectKnockedBack -= OnObjectKnockedBack;
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

}
