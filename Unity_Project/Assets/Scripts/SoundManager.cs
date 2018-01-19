using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_LowPitch = 0.95f;

    [SerializeField] private float m_HighPitch = 1.05f;

    [SerializeField] private float m_FadeOutTime = 0.3f;

    // --------------------------------------------------------------

    private static SoundManager m_Instance;

    private AudioSource m_MainAudio;

    private AudioSource m_LoopingAudio;

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
        }
        else
        {
            Destroy(gameObject);
        }
        SetupAudioSources();
        
    }

    private void SetupAudioSources()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 2)
        {
            Debug.LogError(name + " is missing one or more AudioSources");
        }

        m_MainAudio = audioSources[0];
        m_LoopingAudio = audioSources[1];
        m_LoopingAudio.loop = true;
    }

    public void Play(AudioClip clip)
    {
        if (clip == null) return;

        m_MainAudio.pitch = 1f;
        m_MainAudio.PlayOneShot(clip);
    }

    public void PlayRandom(params AudioClip[] clips)
    {
        if (clips.Length <= 0) return;

        AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
        m_MainAudio.pitch = Random.Range(m_LowPitch, m_HighPitch);

        m_MainAudio.PlayOneShot(clipToPlay);
    }

    public void PlayWithLoop(AudioClip clip)
    {
        if (clip == null) return;

        StopAllCoroutines();
        m_LoopingAudio.volume = 1f;
        m_LoopingAudio.clip = clip;
        m_LoopingAudio.Play();
    }

    public void StopLoopingSound()
    {
        StartCoroutine(FadeOutSound());
    }

    private IEnumerator FadeOutSound()
    {
        float startVolume = m_LoopingAudio.volume;
        while (m_LoopingAudio.volume > 0f)
        {
            m_LoopingAudio.volume -= startVolume * Time.deltaTime / m_FadeOutTime;
            yield return new WaitForEndOfFrame();
        }

        m_LoopingAudio.Stop();
        m_LoopingAudio.volume = startVolume;
    }

}
