using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplayManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private Text m_WinnerTitle;

    [SerializeField] private Image m_Player1Icon;

    [SerializeField] private Image m_Player2Icon;

    [SerializeField] private Sprite[] m_PlayerWinSprites;

    [SerializeField] private Sprite[] m_PlayerLoseSprites;

    // --------------------------------------------------------------

    private StatsTracker[] m_Stats;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_WinnerTitle.text = "Player " + StatsTracker.PlayerWinner + " wins!";

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

        // TODO: Retrieve stats from StatsTrackers and display
        m_Stats = FindObjectsOfType<StatsTracker>();

        foreach (StatsTracker tracker in m_Stats)
        {
            tracker.Disable();
        }

    }

}
