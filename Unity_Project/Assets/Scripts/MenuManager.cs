using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private AudioClip m_ButtonClickSound;

    [SerializeField] private Animator m_FadePanelAnim;

    [SerializeField] private GameObject m_PrimaryButtonsParent;

    [SerializeField] private GameObject m_LevelSelectPanel;

    [SerializeField] private GameObject m_AchievementsPanel;

    // --------------------------------------------------------------

    private Button[] m_PrimaryButtons;

    private GameObject m_LastSelectedMenuItem;

    private Scene m_SelectedScene;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_PrimaryButtons = m_PrimaryButtonsParent.GetComponentsInChildren<Button>();
        m_LastSelectedMenuItem = m_PrimaryButtons[0].gameObject;
    }

    private void ButtonSound()
    {
        SoundManager.Instance.Play(m_ButtonClickSound);
    }

    // Load level select scene
    public void OnPlayButtonClicked()
    {
        ButtonSound();
        m_LastSelectedMenuItem = EventSystem.current.currentSelectedGameObject;
        m_LevelSelectPanel.SetActive(true);
    }

    public void OnTutorialButtonClicked()
    {
        ButtonSound();
        // TODO
    }

    public void OnAchievementsButtonClicked()
    {
        ButtonSound();
        m_LastSelectedMenuItem = EventSystem.current.currentSelectedGameObject;
        SetPrimaryButtonsActive(false);
        m_AchievementsPanel.SetActive(true);
    }

    public void OnLevel1ButtonClicked()
    {
        ButtonSound();
        SceneManager.LoadScene("MainScene");
    }

    public void OnLevel2ButtonClicked()
    {
        ButtonSound();
        SceneManager.LoadScene("SecondScene");
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

    public void OnDismissSubMenu()
    {
        ButtonSound();
        SetPrimaryButtonsActive(true);
        m_AchievementsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(m_LastSelectedMenuItem);
    }

    private void SetPrimaryButtonsActive(bool enabled)
    {
        foreach (Button button in m_PrimaryButtons)
        {
            button.interactable = enabled;
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
