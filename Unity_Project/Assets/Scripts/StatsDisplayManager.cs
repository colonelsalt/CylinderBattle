using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// Displays game stats on Game Over scene and handles transition back to Title Screen
public class StatsDisplayManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private Text m_WinnerTitle;

    [SerializeField] private Image m_Player1Icon;

    [SerializeField] private Image m_Player2Icon;

    [SerializeField] private Sprite[] m_PlayerWinSprites;

    [SerializeField] private Sprite[] m_PlayerLoseSprites;

    [SerializeField] private Text[] m_PiTexts;

    [SerializeField] private Text[] m_TotalPlusesTexts;

    [SerializeField] private Text[] m_PlayerKillsTexts;

    [SerializeField] private Text[] m_EnemyKillsTexts;

    [SerializeField] private Text[] m_DeathsTexts;

    [SerializeField] private Text[] m_DistanceTexts;

    [SerializeField] private Animator m_FadePanelAnim;

    [SerializeField] private Button m_MainMenuButton;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip m_ClickSound;

    // --------------------------------------------------------------

    // Ref. to StatsTrackers carried over from level that just ended; all stats retrieved from here
    private StatsTracker[] m_Stats;

    // --------------------------------------------------------------

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(m_MainMenuButton.gameObject);

        // Update title to reflect winner
        m_WinnerTitle.text = "Player " + StatsTracker.PlayerWinner + " wins!";

        // Show appropriate win/lose icon for each Player
        if (StatsTracker.PlayerWinner == 1)
        {
            m_Player1Icon.sprite = m_PlayerWinSprites[0];
            m_Player2Icon.sprite = m_PlayerLoseSprites[1];
        }
        else if (StatsTracker.PlayerWinner == 2)
        {
            m_Player1Icon.sprite = m_PlayerLoseSprites[0];
            m_Player2Icon.sprite = m_PlayerWinSprites[1];
        }

        // Retrieve stats from StatsTrackers and display in appropriate Text elements
        m_Stats = FindObjectsOfType<StatsTracker>();
        if (m_Stats.Length != GameManager.NUM_PLAYERS)
        {
            Debug.LogError("StatsDisplayManager: Invalid number of StatsTrackers found.");
            return;
        }

        for (int i = 0; i < GameManager.NUM_PLAYERS; i++)
        {
            m_PiTexts[i].text = m_Stats[i].NumPis.ToString();
            m_DeathsTexts[i].text = m_Stats[i].NumDeaths.ToString();
            m_TotalPlusesTexts[i].text = m_Stats[i].NumTotalPluses.ToString();
            m_PlayerKillsTexts[i].text = m_Stats[i].NumPlayerKills.ToString();
            m_EnemyKillsTexts[i].text = m_Stats[i].NumEnemyKills.ToString();
            m_DistanceTexts[i].text = m_Stats[i].DistanceCovered + " m";
        }
        
        // Destroy StatsTrackers to ensure they don't carry on old stats into new level
        foreach (StatsTracker tracker in m_Stats)
        {
            Destroy(tracker.gameObject);
        }
    }

    public void OnMainMenuButtonClicked()
    {
        SoundManager.Instance.Play(m_ClickSound);
        m_FadePanelAnim.SetTrigger("fadeOutTrigger");
        Invoke("LoadMainMenu", 1f);
    }

    public void OnQuitButtonClicked()
    {
        SoundManager.Instance.Play(m_ClickSound);
        m_FadePanelAnim.SetTrigger("fadeOutTrigger");
        Invoke("QuitGame", 1f);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

}
