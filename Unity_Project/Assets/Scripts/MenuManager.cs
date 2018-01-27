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

    private string m_SelectedScene;

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
        SetPrimaryButtonsActive(false);
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

    public void OnLevelSelected(int level)
    {
        ButtonSound();
        m_SelectedScene = (level == 1) ? "MainScene" : "SecondScene";
        FadeOutBeforeLevelLoad();
    }

    public void OnMainMenuButtonClicked()
    {
        ButtonSound();
        m_SelectedScene = "MainMenu";
        FadeOutBeforeLevelLoad();
    }

    public void OnDismissSubMenu()
    {
        ButtonSound();
        SetPrimaryButtonsActive(true);
        m_AchievementsPanel.SetActive(false);
        m_LevelSelectPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(m_LastSelectedMenuItem);
    }

    private void SetPrimaryButtonsActive(bool enabled)
    {
        foreach (Button button in m_PrimaryButtons)
        {
            button.interactable = enabled;
        }
    }

    public void OnQuitButtonPressed()
    {
        ButtonSound();
        m_FadePanelAnim.SetTrigger("fadeOutTrigger");
        Invoke("QuitGame", 1f);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void FadeOutBeforeLevelLoad()
    {
        m_FadePanelAnim.SetTrigger("fadeOutTrigger");
        Invoke("LoadLevel", 1f);
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(m_SelectedScene);
    }

}
