using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sound effects singleton
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    // --------------------------------------------------------------

    // Random variation in sound effect pitch
    [SerializeField] private float m_LowPitch = 0.95f;
    [SerializeField] private float m_HighPitch = 1.05f;

    // Time taken to fade out a passed-in audio source
    [SerializeField] private float m_FadeOutTime = 0.3f;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip m_GameStartSound;

    [SerializeField] private AudioClip m_GameOverSound;

    // --------------------------------------------------------------

    private static SoundManager m_Instance;

    private AudioSource m_Audio;

    // --------------------------------------------------------------

    public static SoundManager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
            GameManager.OnGameStart += OnGameStart;
            m_Audio = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnGameStart()
    {
        Play(m_GameStartSound);
    }

    public void Play(AudioClip clip)
    {
        if (clip == null) return;

        m_Audio.pitch = 1f;
        m_Audio.PlayOneShot(clip);
    }

    // Plays one of clips passed in with slight random variation in pitch
    public void PlayRandom(params AudioClip[] clips)
    {
        if (clips.Length <= 0) return;

        AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
        m_Audio.pitch = Random.Range(m_LowPitch, m_HighPitch);

        m_Audio.PlayOneShot(clipToPlay);
    }

    // Called by objects with their own looping audio source to prevent jarring breaks when playback stops
    public void FadeOut(AudioSource audio)
    {
        if (!audio.isPlaying) return;

        StopAllCoroutines();

        StartCoroutine(FadeOutSound(audio));
    }

    private IEnumerator FadeOutSound(AudioSource audio)
    {
        float startVolume = audio?.volume ?? 1f;
        while (audio?.volume > 0.1f)
        {
            audio.volume -= startVolume * Time.deltaTime / m_FadeOutTime;
            yield return new WaitForEndOfFrame();
            if (audio == null) break;
        }
        if (audio != null)
        {
            audio.Stop();
            audio.volume = startVolume;
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= OnGameStart;
    }

}
