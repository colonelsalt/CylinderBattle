using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementNotification : MonoBehaviour
{
    // --------------------------------------------------------------

    private Animator m_Anim;

    private Image m_Icon;

    private Text m_Title;

    // --------------------------------------------------------------

    private void Awake()
    {
        AchievementManager.OnAchievementUnlocked += OnShowAchievement;

        m_Anim = GetComponent<Animator>();
        m_Icon = transform.GetChild(1).GetComponent<Image>();
        m_Title = transform.GetChild(2).GetComponent<Text>();
    }

    private void OnShowAchievement(Achievement a)
    {
        m_Title.text = a.Title;
        m_Icon.sprite = a.Icon ?? Achievement.DefaultIcon;
        m_Anim.SetTrigger("showTrigger");
    }

    private void OnDisable()
    {
        AchievementManager.OnAchievementUnlocked -= OnShowAchievement;
    }
}
