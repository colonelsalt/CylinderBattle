using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Safe wrapper class around Unity PlayerPrefs
public static class PlayerPrefsManager
{
    // --------------------------------------------------------------

    private const string ACHIEVEMENT_KEY = "unlocked achievement ";

    // --------------------------------------------------------------

    // Use 1 or 0 to denote true or false
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

    // DEBUG ONLY: Delete everything in PlayerPrefs
    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

}
