using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { LOADING, PLAYING, PAUSED, GAME_OVER }

public class GameManager : MonoBehaviour
{
    // --------------------------------------------------------------

    public delegate void GameStateEvent(GameState state);
    public static event GameStateEvent OnGameStateChanged;

    public delegate void GameStartEvent();
    public static event GameStartEvent OnGameStart;

    // --------------------------------------------------------------

    [SerializeField] private float m_LoadTime = 3f;

    // --------------------------------------------------------------

    public const int MAX_NUM_PIS = 5;
    public const int NUM_PLAYERS = 2;
    public const int PLAYER_HEALTH = 3;
    public const float POWERUP_DURATION = 15f;

    // --------------------------------------------------------------

    private GameState m_State = GameState.LOADING;

    private float m_TimeUntilLevelStart;

    private PlayerController[] m_Players;

    // --------------------------------------------------------------

    private void Awake()
    {
        Collector.OnAllPisCollected += OnGameOver;

        m_Players = FindObjectsOfType<PlayerController>();
        SetPlayersActive(false);

        m_TimeUntilLevelStart = m_LoadTime;
    }

    private void Update()
    {
        if (m_State == GameState.LOADING)
        {
            m_TimeUntilLevelStart -= Time.deltaTime;
            if (m_TimeUntilLevelStart <= 0f)
            {
                m_State = GameState.PLAYING;
                OnGameStart();
                SetPlayersActive(true);
            }
        }

        if (InputHelper.PauseButtonPressed())
        {
            if (m_State == GameState.PAUSED)
            {
                Time.timeScale = 1f;
                m_State = GameState.PLAYING;
                
            }
            else if (m_State == GameState.PLAYING)
            {
                Time.timeScale = 0f;
                m_State = GameState.PAUSED;
            }
            OnGameStateChanged(m_State);
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
    }


}
