using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField]
    Text m_Player1ScoreText;

    [SerializeField]
    Text m_Player2ScoreText;

    [SerializeField]
    private Text m_GameOverTitle;

    // --------------------------------------------------------------

    int m_Player1Score = 0;
    int m_Player2Score = 0;

    // --------------------------------------------------------------

    void OnEnable()
    {
        DeathTrigger.OnPlayerDeath += OnUpdateScore;
        Pi.OnPiCaptured += OnUpdateScore;
        GameManager.OnGameOver += OnGameOver;
    }

    void OnUpdateScore(int playerNum)
    {
        if(playerNum == 1)
        {
            m_Player1Score += 1;
            m_Player1ScoreText.text = m_Player1Score + "/" + GameManager.MAX_NUM_PIS;
        }
        else if(playerNum == 2)
        {
            m_Player2Score += 1;
            m_Player2ScoreText.text = m_Player2Score + "/" + GameManager.MAX_NUM_PIS;
        }
    }

    private void OnGameOver(int numOfWinner)
    {
        GameManager.OnGameOver -= OnGameOver;
        DeathTrigger.OnPlayerDeath -= OnUpdateScore;
        Pi.OnPiCaptured -= OnUpdateScore;
        m_GameOverTitle.enabled = true;
        m_GameOverTitle.text += "\nPlayer " + numOfWinner + " wins!";
    }
}
