using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // --------------------------------------------------------------

    // How much music speeds up at match point
    [SerializeField] private float m_SpeedUpMultiplier = 1.4f;

    // --------------------------------------------------------------

    private ScoreKeeper[] m_ScoreKeepers;

    // Whether only one more Pi remains to collect before game over
    private bool m_MatchPointReached = false;

    private AudioSource m_Audio;

    // --------------------------------------------------------------

    private void Awake()
    {
        Collector.OnMatchPoint += OnMatchPoint;
        Collector.OnPiDrop += OnPiDrop;
        Collector.OnAllPisCollected += OnGameOver;

        m_Audio = GetComponent<AudioSource>();
        m_ScoreKeepers = FindObjectsOfType<ScoreKeeper>();
    }

    private void OnMatchPoint(int playerNum)
    {
        m_MatchPointReached = true;
        SpeedUpMusic();
    }

    private void OnPiDrop(int playerNum)
    {
        if (m_MatchPointReached)
        {
            SetMusicToNormal();
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
