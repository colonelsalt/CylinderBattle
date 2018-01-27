using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // --------------------------------------------------------------

    // How much music speeds up at match point
    [SerializeField] private float m_SpeedUpMultiplier = 1.4f;

    // --------------------------------------------------------------

    private AudioSource m_Audio;

    // --------------------------------------------------------------

    private void Awake()
    {
        GameManager.OnGameStart += OnGameStart;
        GameManager.OnGamePaused += OnGamePaused;
        GameManager.OnGameResumed += OnGameResumed;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnMatchPoint += OnSpeedUpMusic;
        GameManager.OnMatchPointEnded += OnSetMusicSpeedToNormal;

        m_Audio = GetComponent<AudioSource>();
    }

    private void OnGameStart()
    {
        m_Audio.Play();
    }

    private void OnGamePaused()
    {
        m_Audio.Pause();
    }

    private void OnGameResumed()
    {
        m_Audio.UnPause();
    }

    private void OnGameOver(int playerNum)
    {
        m_Audio.Stop();
    }

    private void OnSpeedUpMusic()
    {
        m_Audio.pitch = m_SpeedUpMultiplier;
    }

    private void OnSetMusicSpeedToNormal()
    {
        m_Audio.pitch = 1f;
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= OnGameStart;
        GameManager.OnGamePaused -= OnGamePaused;
        GameManager.OnGameResumed -= OnGameResumed;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnMatchPoint -= OnSpeedUpMusic;
        GameManager.OnMatchPointEnded -= OnSetMusicSpeedToNormal;
    }

}
