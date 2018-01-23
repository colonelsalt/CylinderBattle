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
        PORTAL_GUN = 2,
        JETPACK = 3,
        SPRINT = 4,
        STAMINA = 5,
    }

    // --------------------------------------------------------------

    [SerializeField] private ScoreRetriever m_Score;

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

    private Image m_ArrowImage;

    private Vector3 m_StaticPosition;

    private Animator m_Animator;

    private bool m_TutorialActive = false;

    private bool m_TetherToPlayer = true;

    private float m_TutorialTimeRemaining;

    private float m_TimeUntilNextTutorial;

    // --------------------------------------------------------------

    private void Start()
    {
        WeaponManager.OnWeaponPickup += OnWeaponPickup;
        WeaponManager.OnWeaponActivated += OnWeaponActivated;
        PowerupManager.OnPowerupReceived += OnPowerupReceived;

        m_Text = GetComponent<Text>();
        m_Animator = GetComponent<Animator>();
        m_ArrowImage = GetComponentInChildren<Image>();

        m_StaticPosition = transform.position;
        m_TimeUntilNextTutorial = m_DelayTime;

        m_TutorialQueue = new Queue<TutorialAction>();
        m_TutorialsShown = new Dictionary<TutorialAction, bool>();
        for (int i = 0; i <= 5; i++)
        {
            m_TutorialsShown.Add((TutorialAction)i, false);
        }
    }

    private void Update()
    {
        if (m_TutorialActive && m_TutorialTimeRemaining > 0f)
        {
            SetTextPosition();
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
        // If we've already shown tutorial, don't show it again
        if (m_TutorialsShown[action]) return;

        m_TutorialQueue.Enqueue(action);
        m_TutorialsShown[action] = true;
    }

    private void OnWeaponPickup(Weapon type, int playerNum)
    {
        if (playerNum == m_Score.PlayerNum)
        {
            QueueTutorial(TutorialAction.WEAPON);
        }
    }

    private void OnWeaponActivated(Weapon type, int playerNum)
    {
        if (playerNum == m_Score.PlayerNum && type == Weapon.PORTAL_GUN)
        {
            QueueTutorial(TutorialAction.PORTAL_GUN);
        }
    }

    private void OnPowerupReceived(Powerup type, int playerNum)
    {
        if (playerNum != m_Score.PlayerNum) return;

        switch (type)
        {
            case Powerup.JETPACK:
                QueueTutorial(TutorialAction.JETPACK);
                break;
            case Powerup.LIGHTNING_SPRINT:
                QueueTutorial(TutorialAction.SPRINT);
                QueueTutorial(TutorialAction.STAMINA);
                break;
        }
    }

    private void ShowTutorial(TutorialAction action)
    {
        if (action == TutorialAction.STAMINA)
        {
            m_TetherToPlayer = false;
            m_ArrowImage.enabled = true;
        }
        else
        {
            m_TetherToPlayer = true;
        }
        SetTextPosition();
        
        m_Text.text = GetTutorialTextFor(action);
        m_Animator.SetBool("isVisible", true);
        m_TutorialTimeRemaining = m_MaxTutorialTime;
        m_TimeUntilNextTutorial = m_DelayTime;
        m_TutorialActive = true;
    }

    private void HideTutorial()
    {
        m_Animator.SetBool("isVisible", false);
        m_ArrowImage.enabled = false;
        m_TutorialActive = false;
    }

    private void SetTextPosition()
    {
        if (m_TetherToPlayer)
        {
            // Display text on left or right side of player depending on screen position
            Vector3 offset = (m_PlayerCamera.WorldToViewportPoint(m_Score.PlayerPosition).x < 0.5f) ? m_OffsetFromPlayer : -m_OffsetFromPlayer;
            transform.position = m_PlayerCamera.WorldToScreenPoint(m_Score.PlayerPosition + offset);
        }
        else
        {
            transform.position = m_StaticPosition;
        }
    }

    private string GetTutorialTextFor(TutorialAction action)
    {
        switch (action)
        {
            case TutorialAction.WEAPON:
                return "Press " + InputHelper.GetButtonName(ButtonAction.FIRE, m_Score.PlayerNum) + " to fire weapon!";
            case TutorialAction.JETPACK:
                return "Press " + InputHelper.GetButtonName(ButtonAction.JUMP, m_Score.PlayerNum) + " in mid-air for a boost!";
            case TutorialAction.PORTAL_GUN:
                return "Press " + InputHelper.GetButtonName(ButtonAction.FIRE, m_Score.PlayerNum) + " to fire portals!";
            case TutorialAction.SPRINT:
                return "Hold " + InputHelper.GetButtonName(ButtonAction.SPRINT, m_Score.PlayerNum) + " to sprint!";
            case TutorialAction.STAMINA:
                return "Watch your stamina meter!";
            default:
                return "";
        }
    }

}
