using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --------------------------------------------------------------

    public delegate void GameStateEvent();
    public static event GameStateEvent OnGamePause;
    public static event GameStateEvent OnGameResume;

    // --------------------------------------------------------------

    public const int MAX_NUM_PIS = 10;
    public const int NUM_PLAYERS = 2;
    public const int PLAYER_HEALTH = 3;
    public const float POWERUP_DURATION = 15f;

    // --------------------------------------------------------------

    private bool m_IsPaused = false;

    // --------------------------------------------------------------

    private void Update()
    {
        if (InputHelper.PauseButtonPressed())
        {
            if (m_IsPaused)
            {
                Time.timeScale = 1f;
                OnGameResume();
            }
            else
            {
                Time.timeScale = 0f;
                OnGamePause();
            }
        }
    }


}
