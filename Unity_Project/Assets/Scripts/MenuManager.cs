using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private AudioClip m_ButtonClickSound;

    // --------------------------------------------------------------

    // Load level select scene
    public void OnPlayButtonClicked()
    {
        SoundManager.Instance.Play(m_ButtonClickSound);
        SceneManager.LoadScene("MainScene");
    }

    public void OnTutorialButtonClicked()
    {
        SoundManager.Instance.Play(m_ButtonClickSound);
    }

    public void OnAchievementsButtonClicked()
    {
        SoundManager.Instance.Play(m_ButtonClickSound);
    }

}
