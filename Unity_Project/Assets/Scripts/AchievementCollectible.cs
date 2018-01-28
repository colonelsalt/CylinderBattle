using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCollectible : MonoBehaviour
{
    // --------------------------------------------------------------

    public delegate void AchievementCollectibleEvent();
    public static event AchievementCollectibleEvent OnAllSpecialCollectiblesGrabbed;

    // --------------------------------------------------------------

    private static int NUM_SPECIALS = 0;

    // --------------------------------------------------------------

    private void Awake()
    {
        if (NUM_SPECIALS == 0)
        {
            GameManager.OnGameExit += OnResetCount;
            GameManager.OnGameOver += OnGameOver;
        }
        NUM_SPECIALS++;
    }

    private void OnGameOver(int numOfWinner)
    {
        OnResetCount();
    }

    private void OnVanish()
    {
        NUM_SPECIALS--;
        if (NUM_SPECIALS <= 0)
        {
            OnAllSpecialCollectiblesGrabbed();
        }
    }

    private void OnResetCount()
    {
        NUM_SPECIALS = 0;
        GameManager.OnGameExit -= OnResetCount;
        GameManager.OnGameOver -= OnGameOver;
    }
}
