using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // --------------------------------------------------------------

    // How much music speeds up at match point
    [SerializeField] private float m_SpeedUpMultiplier = 1.4f;

    [SerializeField] private ScoreKeeper[] m_ScoreKeepers;

    // --------------------------------------------------------------

    // Whether only one more Pi remains to collect before game over
    private bool m_MatchPointReached = false;

    private AudioSource m_Audio;

    // --------------------------------------------------------------

    private void Awake()
    {
        Collector.OnPiPickup += OnPiPickup;
        Collector.OnPiDrop += OnPiDrop;
        Collector.OnAllPisCollected += OnGameOver;

        m_Audio = GetComponent<AudioSource>();
    }

    private void OnPiPickup(int playerNum)
    {
        foreach (ScoreKeeper score in m_ScoreKeepers)
        {
            if (score.PlayerNum == playerNum)
            {
                if (score.NumPis == GameManager.MAX_NUM_PIS - 1)
                {
                    m_MatchPointReached = true;
                    SpeedUpMusic();
                }
                break;
            }
        }
    }

    private void OnPiDrop(int playerNum)
    {
        if (!m_MatchPointReached) return;

        foreach (ScoreKeeper score in m_ScoreKeepers)
        {
            if (score.PlayerNum == playerNum)
            {
                if (score.NumPis < GameManager.MAX_NUM_PIS - 1)
                {
                    m_MatchPointReached = false;
                    SetMusicToNormal();
                }
                break;
            }
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
