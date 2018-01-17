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

    private AudioSource m_Audio;

    // TODO: Get this
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

        m_Audio = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        m_Audio.pitch = 1f;
        m_Audio.PlayOneShot(clip);
    }

    public void PlayRandom(params AudioClip[] clips)
    {
        if (clips.Length <= 0) return;

        AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
        m_Audio.pitch = Random.Range(m_LowPitch, m_HighPitch);

        m_Audio.PlayOneShot(clipToPlay);
    }

    public void PlayWithLoop(AudioClip clip)
    {
        m_Audio.pitch = 1f;
        m_Audio.clip = clip;
        m_Audio.loop = true;
        m_Audio.Play();
    }

    public void StopLoopingSound()
    {
        // TODO: Fade out looping sound here
        m_Audio.Stop();
    }

    private IEnumerator FadeOutSound()
    {
        float startVol = m_LoopingAudio.volume;
        while (m_LoopingAudio.volume > 0f)
        {
            m_LoopingAudio.volume -= startVol * Time.deltaTime / m_FadeOutTime;
            yield return new WaitForEndOfFrame();
        }

        m_LoopingAudio.Stop();
        m_LoopingAudio.volume = startVol;
    }

}
