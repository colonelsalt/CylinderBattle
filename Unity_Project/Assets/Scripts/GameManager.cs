using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // --------------------------------------------------------------

    private enum GameState { LOADING, PRE_GAME, PLAYING, PAUSED, GAME_OVER }

    // --------------------------------------------------------------

    public delegate void GameStateEvent();
    public static event GameStateEvent OnGameStart;
    public static event GameStateEvent OnGamePaused;
    public static event GameStateEvent OnGameResumed;
    public static event GameStateEvent OnMatchPoint;
    public static event GameStateEvent OnMatchPointEnded;

    public delegate void GameOverEvent(int numOfWinner);
    public static event GameOverEvent OnGameOver;

    // --------------------------------------------------------------

    // Time between level finishing load and game starting
    [SerializeField] private float m_StartupTime = 3f;

    // Time between game over and transition to GameOver screen
    [SerializeField] private float m_LevelTransitionTime = 3f;

    // --------------------------------------------------------------

    public const int MAX_NUM_PIS = 5;
    public const int NUM_PLAYERS = 2;
    public const int PLAYER_HEALTH = 3;
    public const float POWERUP_DURATION = 15f;

    // --------------------------------------------------------------

    private GameState m_State = GameState.LOADING;

    private float m_TimeUntilLevelStart;

    private PlayerController[] m_PlayerControllers;

    private IPlayer[] m_Players;

    private float m_GameOverTime;

    private bool m_MatchPointReached = false;

    // --------------------------------------------------------------

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Collector.OnPiPickup += OnCheckForWinState;
        Collector.OnPiDrop += OnCheckIfMatchPointEnded;

        m_PlayerControllers = FindObjectsOfType<PlayerController>();

        m_Players = new IPlayer[m_PlayerControllers.Length];
        for (int i = 0; i < m_PlayerControllers.Length; i++)
        {
            m_Players[i] = m_PlayerControllers[i].GetComponent<IPlayer>();
        }

        SetPlayerControllersActive(false);

        m_TimeUntilLevelStart = m_StartupTime;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_State = GameState.PRE_GAME;
    }

    private void OnCheckForWinState(int playerNum)
    {
        foreach (IPlayer player in m_Players)
        {
            if (player.PlayerNum() == playerNum)
            {
                if (player.NumPis() >= MAX_NUM_PIS)
                {
                    OnGameOver(playerNum);
                    GameOver();
                }
                else if (player.NumPis() == MAX_NUM_PIS - 1 && !m_MatchPointReached)
                {
                    m_MatchPointReached = true;
                    OnMatchPoint();
                }
                break;
            }
        }
    }

    private void OnCheckIfMatchPointEnded(int playerNum)
    {
        if (!m_MatchPointReached) return;

        bool matchPointEnded = false;

        foreach (IPlayer player in m_Players)
        {
            matchPointEnded |= (player.NumPis() == MAX_NUM_PIS - 1);
        }
        if (!matchPointEnded)
        {
            m_MatchPointReached = false;
            OnMatchPointEnded();
        }
    }

    private void Update()
    {
        if (m_State == GameState.PRE_GAME)
        {
            m_TimeUntilLevelStart -= Time.deltaTime;
            if (m_TimeUntilLevelStart <= 0f)
            {
                m_State = GameState.PLAYING;
                OnGameStart();
                SetPlayerControllersActive(true);
            }
        }
        else if (InputHelper.PauseButtonPressed())
        {
            if (m_State == GameState.PAUSED)
            {
                Time.timeScale = 1f;
                m_State = GameState.PLAYING;
                SetPlayerControllersActive(true);
                OnGameResumed();
                
            }
            else if (m_State == GameState.PLAYING)
            {
                Time.timeScale = 0f;
                m_State = GameState.PAUSED;
                SetPlayerControllersActive(false);
                OnGamePaused();
            }
        }
        else if (m_State == GameState.GAME_OVER)
        {
            if (Time.realtimeSinceStartup > m_GameOverTime + m_LevelTransitionTime)
            {
                // Set TimeScale to normal and load Game Over screen
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                SceneManager.LoadScene("GameOver");
            }
        }
    }

    private void SetPlayerControllersActive(bool enabled)
    {
        foreach (PlayerController controller in m_PlayerControllers)
        {
            controller.enabled = enabled;
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        m_State = GameState.GAME_OVER;
        m_GameOverTime = Time.realtimeSinceStartup;
    }




}
