using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    // --------------------------------------------------------------

    private string m_Title;

    private string m_Description;

    private bool m_IsHidden;

    // --------------------------------------------------------------

    public string Title
    {
        get
        {
            return m_Title;
        }
    }

    public string Description
    {
        get
        {
            return m_Description;
        }
    }

    public bool IsHidden
    {
        get
        {
            return m_IsHidden;
        }
    }

    public Sprite Icon
    {
        get
        {
            return Resources.Load<Sprite>("AchievementIcons/" + m_Title);
        }
    }

    public static Sprite DefaultIcon
    {
        get
        {
            return Resources.Load<Sprite>("AchievementIcons/Default");
        }
    }

    // --------------------------------------------------------------

    public Achievement(string title, string description, bool isHidden = false)
    {
        m_Title = title;
        m_Description = description;
        m_IsHidden = isHidden;
    }

    public void Unlock()
    {
        PlayerPrefsManager.UnlockAchievement(this);
    }

    public bool IsUnlocked()
    {
        return PlayerPrefsManager.IsAchievementUnlocked(this);
    }

    public override string ToString()
    {
        return m_Title;
    }

}
