using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple object counting component; if all AchievementCollectibles in a level are collected, earn achievement
public class AchievementCollectible : Collectible
{
    // --------------------------------------------------------------

    public delegate void AchievementCollectibleEvent();
    public static event AchievementCollectibleEvent OnAllSpecialCollectiblesGrabbed;

    // --------------------------------------------------------------

    // Static counter for number of objects of this type in level
    private static int NUM_SPECIALS = 0;

    // --------------------------------------------------------------

    protected override void Awake()
    {
        // Ensure static listeners are only set up once per level
        if (NUM_SPECIALS == 0)
        {
            GameManager.OnGameExit += OnResetCount;
            GameManager.OnGameOver += OnGameOver;
        }
        NUM_SPECIALS++;
        base.Awake();
    }

    private void OnGameOver(int numOfWinner)
    {
        OnResetCount();
    }

    protected override void Vanish()
    {
        NUM_SPECIALS--;
        if (NUM_SPECIALS <= 0)
        {
            OnAllSpecialCollectiblesGrabbed();
        }
        base.Vanish();
    }

    // Ensure count is reset when level left
    private void OnResetCount()
    {
        NUM_SPECIALS = 0;
        GameManager.OnGameExit -= OnResetCount;
        GameManager.OnGameOver -= OnGameOver;
    }
}
