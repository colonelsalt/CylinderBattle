using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --------------------------------------------------------------

    // Events
    public delegate void GameFinished(int numOfWinner);
    public static event GameFinished OnGameOver;

    // --------------------------------------------------------------

    public const int MAX_NUM_PIS = 10;
    
    // --------------------------------------------------------------

    private int m_Player1Score = 0;
    private int m_Player2Score = 0;
    
    // --------------------------------------------------------------

    private void Awake()
    {
        Pi.OnPiCaptured += OnPiCaptured;
    }

    private void OnPiCaptured(int playerNum)
    {
        switch(playerNum)
        {
            case 1:
                if (++m_Player1Score >= MAX_NUM_PIS) GameOver(1);
                break;
            case 2:
                if (++m_Player2Score >= MAX_NUM_PIS) GameOver(2);
                break;
        }
    }

    private void GameOver(int numOfWinner)
    {
        OnGameOver(numOfWinner);
        Debug.Log("Game over!");
    }

}
