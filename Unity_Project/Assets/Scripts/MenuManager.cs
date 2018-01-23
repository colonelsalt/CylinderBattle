using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private AudioClip m_ButtonClickSound;

    [SerializeField] private Animator m_FadePanelAnim;

    // --------------------------------------------------------------

    private void ButtonSound()
    {
        SoundManager.Instance.Play(m_ButtonClickSound);
    }

    // Load level select scene
    public void OnPlayButtonClicked()
    {
        ButtonSound();
        FadeOutBeforeLevelLoad();
    }

    public void OnTutorialButtonClicked()
    {
        ButtonSound();
        // TODO
    }

    public void OnAchievementsButtonClicked()
    {
        ButtonSound();
        // TODO
    }

    public void OnMainMenuButtonClicked()
    {
        ButtonSound();
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            // TODO: Show level select options

        }
        else
        {
            FadeOutBeforeLevelLoad();
        }
    }

    private void FadeOutBeforeLevelLoad()
    {
        m_FadePanelAnim.SetTrigger("fadeOutTrigger");
        Invoke("LoadNextLevel", 1.2f);
    }

    private void LoadNextLevel()
    {
        int levelIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(levelIndex);
    }

}
