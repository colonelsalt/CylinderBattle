using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Keeps track of hidden Player stats throughout game, for display on Game Over screen and for notifying Achievement system
public class StatsTracker : MonoBehaviour
{
    // --------------------------------------------------------------

    public delegate void PlayerStatEvent();
    public static event PlayerStatEvent OnFiftyPlusesCollected;
    public static event PlayerStatEvent OnTenThousandMetresMoved;
    public static event PlayerStatEvent OnFivePlayerKills;
    public static event PlayerStatEvent OnTenEnemyKills;
    public static event PlayerStatEvent OnSurvivedLevelWithoutDeath;
    public static event PlayerStatEvent OnFiveZeroGame;
    public static event PlayerStatEvent OnPlayerIndecency;
    public static event PlayerStatEvent OnPlayerSuicide;
    public static event PlayerStatEvent OnPlayerStolePi;

    // --------------------------------------------------------------

    [SerializeField] private PlayerStats m_Player;

    [SerializeField] private PlayerStats m_OtherPlayer;

    // How long after Player X knocks over Player Y to consider Y's death as X's kill
    [SerializeField] private float m_KnockBackTime = 3f;

    // --------------------------------------------------------------

    // Set when level ended
    private static int m_PlayerWinner = 0;

    // --------------------------------------------------------------

    private int m_Pis = 0;

    private int m_TotalPluses = 0;

    private int m_PlayerKills = 0;

    private int m_EnemyKills = 0;

    private int m_Deaths = 0;

    private float m_Distance = 0;

    private bool m_GameStarted = false;

    private Vector3 m_LastPlayerPos;

    private float m_TimeSinceKnockOver;

    private bool m_ReachedTenThousandMetres = false;

    private PlayerController m_PlayerController;

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
        GameManager.OnGameStart -= OnGameStart;
        m_GameStarted = true;

        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameExit += OnGameExit;
        Collector.OnPlusPickup += OnPlusPickup;
        PlayerHealth.OnPlayerDeath += OnPlayerDeath;
        EnemyHealth.OnEnemyDeath += OnEnemyDeath;
        PhysicsSwitch.OnObjectKnockedBack += OnObjectKnockedBack;
        Collectible.OnDroppedItemPickup += OnDroppedPiPickup;

        m_PlayerController = m_Player.GetComponent<PlayerController>();

        m_LastPlayerPos = m_Player.Position();
        DontDestroyOnLoad(gameObject);
    }

    // Check for Pi steal
    private void OnDroppedPiPickup(GameObject owner, GameObject receiver)
    {
        int? ownerNum = owner.GetComponent<IPlayer>()?.PlayerNum();
        if (ownerNum == m_Player.PlayerNum())
        {
            int? receiverNum = receiver.GetComponent<IPlayer>()?.PlayerNum();
            if (receiverNum != m_Player.PlayerNum())
            {
                OnPlayerStolePi();
            }
        }
    }

    // Ensure StatsTracker is destroyed if level quit prematurely, to prevent stats carrying over into new level
    private void OnGameExit()
    {
        Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (!m_GameStarted) return;

        // If Player alive, update their "distance covered" measure (don't count respawn movement)
        if (m_Player.IsPlayerAlive)
        {
            m_Distance += Vector3.Distance(m_LastPlayerPos, m_Player.Position());

            if (m_Distance >= 10000f && !m_ReachedTenThousandMetres)
            {
                OnTenThousandMetresMoved();
                m_ReachedTenThousandMetres = true;
            }
        }
        m_LastPlayerPos = m_Player.Position();

        if (m_TimeSinceKnockOver < m_KnockBackTime)
        {
            m_TimeSinceKnockOver += Time.deltaTime;
        }

        // If Player is crouching, moving up and down and touching their opponent simultaneously, they must be up to something dirty
        if (m_PlayerController.IsCrouching)
        {
            if (Mathf.Abs(InputHelper.GetAxisX(m_Player.PlayerNum())) > 0 || Mathf.Abs(InputHelper.GetAxisY(m_Player.PlayerNum())) > 0)
            {
                if (Vector3.Distance(m_Player.Position(), m_OtherPlayer.Position()) < 1.5f)
                {
                    OnPlayerIndecency();
                }
            }
        }

    }


    private void OnGameOver(int playerNum)
    {
        m_PlayerWinner = playerNum;

        RemoveInGameListeners();

        m_Pis = m_Player.NumPis();

        if (m_Deaths == 0)
        {
            OnSurvivedLevelWithoutDeath();
        }
        if (m_Pis == 5 && m_OtherPlayer.NumPis() == 0)
        {
            OnFiveZeroGame();
        }
    }

    private void OnPlusPickup(int playerNum)
    {
        if (playerNum == m_Player.PlayerNum())
        {
            m_TotalPluses++;
            if (m_Player.NumPluses >= 50)
            {
                OnFiftyPlusesCollected();
            }
        }
    }

    
    // Check if Death increments death count, kill count, or counts as suicide
    private void OnPlayerDeath(int playerNum, GameObject killer)
    {
        if (playerNum == m_Player.PlayerNum())
        {
            m_Deaths++;

            int? playerAttackerNum = killer.GetComponent<IPlayer>()?.PlayerNum();
            if (playerAttackerNum == m_Player.PlayerNum())
            {
                OnPlayerSuicide();
            }
        }
        else
        {
            // If other Player was killed by this Player, increment PlayerKills count
            int? playerAttackerNum = killer.GetComponent<IPlayer>()?.PlayerNum();

            if (playerAttackerNum == m_Player.PlayerNum())
            {
                m_PlayerKills++;
            }

            // If other player fell in lava, check if this counts as our kill
            else if (killer.GetComponent<DeathTrigger>() != null)
            {
                if (m_TimeSinceKnockOver < m_KnockBackTime)
                {
                    m_PlayerKills++;
                }
            }

            if (m_PlayerKills >= 5)
            {
                OnFivePlayerKills();
            }
        }
    }

    // Increment enemy kill count if this tracker's Player killed enemy
    private void OnEnemyDeath(GameObject killer)
    {
        int? playerKillerNum = killer.GetComponent<IPlayer>()?.PlayerNum();
        if (playerKillerNum == m_Player.PlayerNum())
        {
            m_EnemyKills++;
            if (m_EnemyKills >= 10)
            {
                OnTenEnemyKills();
            }
        }
    }

    // If this tracker's Player knocked opponent (or self) over, keep track of it for a few seconds in case it leads to an indirect kill
    private void OnObjectKnockedBack(GameObject victim, GameObject attacker)
    {
        int? playerAttackerNum = attacker.GetComponent<IPlayer>()?.PlayerNum();
        if (playerAttackerNum == m_Player.PlayerNum())
        {
            if (victim.GetComponent<IPlayer>() != null)
            {
                m_TimeSinceKnockOver = 0f;
            }
        }
    }

    private void RemoveInGameListeners()
    {
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameExit -= OnGameExit;
        Collector.OnPlusPickup -= OnPlusPickup;
        PhysicsSwitch.OnObjectKnockedBack -= OnObjectKnockedBack;
    }

    private void OnDisable()
    {
        RemoveInGameListeners();
    }

}
