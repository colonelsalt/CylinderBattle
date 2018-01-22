using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // --------------------------------------------------------------

    // How much music speeds up at match point
    [SerializeField] private float m_SpeedUpMultiplier = 1.4f;

    // --------------------------------------------------------------

    // Whether only one more Pi remains to collect before game over
    private bool m_MatchPointReached = false;

    private int m_PlayerNumOfMatchPointHolder = 0;

    private AudioSource m_Audio;

    // --------------------------------------------------------------

    private void Awake()
    {
        GameManager.OnGameStart += OnGameStart;

        Collector.OnMatchPoint += OnMatchPoint;
        Collector.OnPiDrop += OnPiDrop;
        Collector.OnAllPisCollected += OnGameOver;

        m_Audio = GetComponent<AudioSource>();
    }

    private void OnGameStart()
    {
        m_Audio.Play();
    }

    private void OnMatchPoint(int playerNum)
    {
        m_PlayerNumOfMatchPointHolder = playerNum;
        m_MatchPointReached = true;
        SpeedUpMusic();
    }

    private void OnPiDrop(int playerNum)
    {
        if (m_MatchPointReached && playerNum == m_PlayerNumOfMatchPointHolder)
        {
            SetMusicToNormal();
            m_PlayerNumOfMatchPointHolder = 0;
            m_MatchPointReached = false;
        }
    }

    private void OnGameOver(int playerNum)
    {
        m_Audio.Stop();
    }

    private void SpeedUpMusic()
    {
        m_Audio.pitch = m_SpeedUpMultiplier;
    }

    private void SetMusicToNormal()
    {
        m_Audio.pitch = 1f;
    }

}
