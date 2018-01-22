using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private AudioClip m_ButtonClickSound;

    // --------------------------------------------------------------

    private void ButtonSound()
    {
        SoundManager.Instance.Play(m_ButtonClickSound);
    }

    // Load level select scene
    public void OnPlayButtonClicked()
    {
        ButtonSound();
        SceneManager.LoadScene("MainScene");
    }

    public void OnTutorialButtonClicked()
    {
        ButtonSound();
    }

    public void OnAchievementsButtonClicked()
    {
        ButtonSound();
    }

    public void OnMainMenuButtonClicked()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            // TODO
        }
        else
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }

}
