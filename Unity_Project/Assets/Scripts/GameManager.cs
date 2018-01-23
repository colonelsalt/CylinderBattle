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

    private PlayerController[] m_Players;

    private float m_GameOverTime;

    // --------------------------------------------------------------

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Collector.OnAllPisCollected += OnGameOver;

        m_Players = FindObjectsOfType<PlayerController>();
        SetPlayersActive(false);

        m_TimeUntilLevelStart = m_StartupTime;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_State = GameState.PRE_GAME;
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
                SetPlayersActive(true);
            }
        }
        else if (InputHelper.PauseButtonPressed())
        {
            if (m_State == GameState.PAUSED)
            {
                Time.timeScale = 1f;
                m_State = GameState.PLAYING;
                SetPlayersActive(true);
                OnGameResumed();
                
            }
            else if (m_State == GameState.PLAYING)
            {
                Time.timeScale = 0f;
                m_State = GameState.PAUSED;
                SetPlayersActive(false);
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

    private void SetPlayersActive(bool enabled)
    {
        foreach (PlayerController player in m_Players)
        {
            player.enabled = enabled;
        }
    }

    private void OnGameOver(int playerNum)
    {
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        m_State = GameState.GAME_OVER;
        m_GameOverTime = Time.realtimeSinceStartup;
    }




}
