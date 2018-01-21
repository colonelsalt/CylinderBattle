using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour
{
    private enum TutorialAction
    {
        MAIN_OBJECTIVE = 0,
        WEAPON = 1,
        GUN = 2,
        BOXING_GLOVES = 3,
        PORTAL_GUN = 4,
        JETPACK = 5,
        SPRINT = 6,
        STAMINA = 7,
        BOMB = 8
    }

    // --------------------------------------------------------------

    [SerializeField] private ScoreKeeper m_Score;

    [SerializeField] private Camera m_PlayerCamera;

    [SerializeField] private float m_MaxTutorialTime = 2f;

    // Delay between an action happening and its tutorial appearing
    [SerializeField] private float m_DelayTime = 0.5f;

    [SerializeField] private Vector3 m_OffsetFromPlayer;

    // --------------------------------------------------------------

    // Tracks whether a given tutorial has been shown
    private Dictionary<TutorialAction, bool> m_TutorialsShown;

    private Queue<TutorialAction> m_TutorialQueue;

    private Text m_Text;

    private Animator m_Animator;

    private bool m_TutorialActive = false;

    private float m_TutorialTimeRemaining;

    private float m_TimeUntilNextTutorial;

    // --------------------------------------------------------------

    private void Awake()
    {
        WeaponManager.OnWeaponPickup += OnWeaponPickup;
        PowerupManager.OnPowerupReceived += OnPowerupReceived;

        m_Text = GetComponent<Text>();
        m_Animator = GetComponent<Animator>();

        m_TutorialQueue = new Queue<TutorialAction>();
        m_TutorialsShown = new Dictionary<TutorialAction, bool>();
        for (int i = 0; i <= 8; i++)
        {
            m_TutorialsShown.Add((TutorialAction)i, false);
        }
    }

    private void Update()
    {
        if (m_TutorialActive && m_TutorialTimeRemaining > 0f)
        {
            AnchorTextToPlayer();
            m_TutorialTimeRemaining -= Time.deltaTime;
            if (m_TutorialTimeRemaining <= 0f)
            {
                HideTutorial();
            }
        }
        else if (m_TutorialQueue.Count > 0 && m_TimeUntilNextTutorial > 0f)
        {
            m_TimeUntilNextTutorial -= Time.deltaTime;
            if (m_TimeUntilNextTutorial <= 0f)
            {
                ShowTutorial(m_TutorialQueue.Dequeue());
            }
        }
    }

    private void QueueTutorial(TutorialAction action)
    {
        m_TutorialQueue.Enqueue(action);
        m_TimeUntilNextTutorial = m_DelayTime;
    }

    private void OnWeaponPickup(Weapon type, int playerNum)
    {
        if (playerNum != m_Score.PlayerNum) return;

        switch (type)
        {
            case Weapon.GUN:
                if (!m_TutorialsShown[TutorialAction.GUN])
                {
                    QueueTutorial(TutorialAction.GUN);
                }
                break;
            case Weapon.BOMB:
                if (!m_TutorialsShown[TutorialAction.BOMB])
                {
                    QueueTutorial(TutorialAction.BOMB);
                }
                break;
            case Weapon.PORTAL_GUN:
                if (!m_TutorialsShown[TutorialAction.PORTAL_GUN])
                {
                    QueueTutorial(TutorialAction.PORTAL_GUN);
                }
                break;
        }
    }

    private void OnPowerupReceived(Powerup type, int playerNum)
    {
        if (playerNum != m_Score.PlayerNum) return;

        switch (type)
        {
            case Powerup.JETPACK:
                if (!m_TutorialsShown[TutorialAction.JETPACK])
                {
                    QueueTutorial(TutorialAction.JETPACK);
                }
                break;
        }

    }

    private void ShowTutorial(TutorialAction action)
    {
        AnchorTextToPlayer();
        
        m_Text.text = GetTutorialTextFor(action);
        m_Animator.SetBool("isVisible", true);
        m_TutorialTimeRemaining = m_MaxTutorialTime;

        m_TutorialsShown[action] = true;
        m_TutorialActive = true;
    }

    private void HideTutorial()
    {
        m_Animator.SetBool("isVisible", false);
        m_TutorialActive = false;
    }

    private void AnchorTextToPlayer()
    {
        // Display text on left or right side of player depending on screen position
        Vector3 offset = (m_PlayerCamera.WorldToViewportPoint(m_Score.PlayerPosition).x < 0.5f) ? m_OffsetFromPlayer : -m_OffsetFromPlayer;
        m_Text.transform.position = m_PlayerCamera.WorldToScreenPoint(m_Score.PlayerPosition + offset);
    }

    private string GetTutorialTextFor(TutorialAction action)
    {
        switch (action)
        {
            case TutorialAction.GUN:
                return "Press " + InputHelper.GetButtonName(ButtonAction.FIRE, m_Score.PlayerNum) + " to fire laser!";
            case TutorialAction.JETPACK:
                return "Press " + InputHelper.GetButtonName(ButtonAction.JUMP, m_Score.PlayerNum) + " in mid-air to use jetpack!";
            case TutorialAction.BOMB:
                return "Press " + InputHelper.GetButtonName(ButtonAction.FIRE, m_Score.PlayerNum) + " to place bomb!";
            case TutorialAction.PORTAL_GUN:
                return "Press " + InputHelper.GetButtonName(ButtonAction.FIRE, m_Score.PlayerNum) + " to fire portals!";
            default:
                return "";
        }
    }

}
