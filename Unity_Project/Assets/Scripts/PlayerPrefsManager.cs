using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager
{
    // --------------------------------------------------------------

    private const string ACHIEVEMENT_KEY = "unlocked achievement ";

    // --------------------------------------------------------------

    public static void UnlockAchievement(Achievement achivement)
    {
        PlayerPrefs.SetInt(ACHIEVEMENT_KEY + achivement, 1);
        PlayerPrefs.Save();
    }

    public static bool IsAchievementUnlocked(Achievement achievement)
    {
        if (PlayerPrefs.HasKey(ACHIEVEMENT_KEY + achievement))
        {
            return (PlayerPrefs.GetInt(ACHIEVEMENT_KEY + achievement) == 1);
        }
        else
        {
            return false;
        }
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

}
