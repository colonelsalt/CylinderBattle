using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Sets up achievements 
public class AchievementManager : MonoBehaviour
{
    // --------------------------------------------------------------

    private enum AchievementType
    {
        SURVIVOR,
        STEAL_PI,
        TEN_THOUSAND_METRES,
        FIFTY_PLUSES,
        FIVE_PLAYER_KILLS,
        TEN_ENEMY_KILLS,
        FIVE_ZERO_GAME,
        BROKE_ALL_CRATES,
        COLLECT_ALL_SPIKE_PLUSES,
        OUT_OF_BOUNDS,
        SUICIDE,
        INDECENCY
    }

    // --------------------------------------------------------------

    // Events
    public delegate void AchievementEvent(Achievement a);
    public static event AchievementEvent OnAchievementUnlocked;

    // --------------------------------------------------------------

    // DEBUG ONLY: If set, delete all achievement progress on game start
    [SerializeField] private bool m_DeleteAllOnStart = false;

    [SerializeField] private AudioClip m_AchievementSound;

    // --------------------------------------------------------------

    // Private singleton instance
    private static AchievementManager m_Instance;

    // Dictionary containing all achievements in game
    private Dictionary<AchievementType, Achievement> m_Achievements = new Dictionary<AchievementType, Achievement>();

    // Achievements to unlock on Game Over screen after level finished
    private Queue<Achievement> m_PostGameAchievementQ;

    // --------------------------------------------------------------

    // For retrieving full list of Achievements on Title Screen sub-menu
    public static List<Achievement> AchievementList
    {
        get
        {
            return m_Instance.m_Achievements.Values.ToList();
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialise();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialise()
    {
        // Set up listeners
        SceneManager.sceneLoaded += OnSceneLoaded;

        DeathTrigger.OnPlayerOutOfBounds += OnPlayerOutOfBounds;
        Breakable.OnAllObjectsBroken += OnAllCratesBroken;
        AchievementCollectible.OnAllSpecialCollectiblesGrabbed += OnAllSpikePlusesCollected;
        StatsTracker.OnFiftyPlusesCollected += OnFiftyPlusesCollected;
        StatsTracker.OnTenThousandMetresMoved += OnPlayerWalkedTenThousandMeters;
        StatsTracker.OnFivePlayerKills += OnFivePlayerKills;
        StatsTracker.OnTenEnemyKills += OnTenEnemyKills;
        StatsTracker.OnSurvivedLevelWithoutDeath += OnSurvivedLevelWithoutDeath;
        StatsTracker.OnFiveZeroGame += OnFiveZeroGame;
        StatsTracker.OnPlayerIndecency += OnPlayerIndeceny;
        StatsTracker.OnPlayerSuicide += OnPlayerSuicide;
        StatsTracker.OnPlayerStolePi += OnPiStolen;

        if (m_DeleteAllOnStart)
        {
            PlayerPrefsManager.DeleteAll();
        }

        // Create Achievement instances
        m_Achievements[AchievementType.SURVIVOR] = new Achievement(
            "Survivor",
            "Survive an entire game without dying"
            );

        m_Achievements[AchievementType.STEAL_PI] = new Achievement(
            "MINE!",
            "Steal a Pi from your fellow player"
            );

        m_Achievements[AchievementType.TEN_THOUSAND_METRES] = new Achievement(
            "Metric Mile",
            "Move more than 10,000m during the course of a game"
            );

        m_Achievements[AchievementType.FIFTY_PLUSES] = new Achievement(
            "Hoarder",
            "Collect 50 Pluses in one game without dying"
            );

        m_Achievements[AchievementType.TEN_ENEMY_KILLS] = new Achievement(
            "Vigilante",
            "Kill ten enemies in one game"
            );

        m_Achievements[AchievementType.FIVE_PLAYER_KILLS] = new Achievement(
            "Ruthless",
            "Kill your fellow player five times in one game"
            );

        m_Achievements[AchievementType.FIVE_ZERO_GAME] = new Achievement(
            "Unfair Match",
            "Beat your opponent 5-0"
            );

        m_Achievements[AchievementType.BROKE_ALL_CRATES] = new Achievement(
            "Demolition Man",
            "Break all the crates in a level"
            );

        m_Achievements[AchievementType.COLLECT_ALL_SPIKE_PLUSES] = new Achievement(
            "Livin' on the Edge",
            "Collect all Pluses in <i>Limits from Above</i>'s grid of Spike Traps",
            true
            );

        m_Achievements[AchievementType.SUICIDE] = new Achievement(
            "Suicide",
            "Kill yourself with your own bomb",
            true
            );

        m_Achievements[AchievementType.OUT_OF_BOUNDS] = new Achievement(
            "Not here, buddy",
            "Step outside the bounds of the level",
            true
            );

        m_Achievements[AchievementType.INDECENCY] = new Achievement(
            "Dirty boi",
            "Perform indecent actions on your fellow player",
            true
            );

        m_PostGameAchievementQ = new Queue<Achievement>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If Game Over screen, unlock all queued-up Achievements
        if (scene.name == "GameOver" && m_PostGameAchievementQ.Count > 0)
        {
            while (m_PostGameAchievementQ.Count > 0)
            {
                TryUnlock(m_PostGameAchievementQ.Dequeue());
            }
        }
    }

    private void OnPiStolen()
    {
        TryUnlock(m_Achievements[AchievementType.STEAL_PI]);
    }

    private void OnAllSpikePlusesCollected()
    {
        TryUnlock(m_Achievements[AchievementType.COLLECT_ALL_SPIKE_PLUSES]);
    }

    private void OnAllCratesBroken()
    {
        TryUnlock(m_Achievements[AchievementType.BROKE_ALL_CRATES]);
    }

    private void OnPlayerSuicide()
    {
        TryUnlock(m_Achievements[AchievementType.SUICIDE]);
    }

    private void OnPlayerIndeceny()
    {
        TryUnlock(m_Achievements[AchievementType.INDECENCY]);
    }

    private void OnSurvivedLevelWithoutDeath()
    {
        m_PostGameAchievementQ.Enqueue(m_Achievements[AchievementType.SURVIVOR]);
    }


    private void OnPlayerOutOfBounds()
    {
        TryUnlock(m_Achievements[AchievementType.OUT_OF_BOUNDS]);
    }

    private void OnPlayerWalkedTenThousandMeters()
    {
        TryUnlock(m_Achievements[AchievementType.TEN_THOUSAND_METRES]);
    }

    private void OnFiftyPlusesCollected()
    {
        TryUnlock(m_Achievements[AchievementType.FIFTY_PLUSES]);
    }

    private void OnFivePlayerKills()
    {
        TryUnlock(m_Achievements[AchievementType.FIVE_PLAYER_KILLS]);
    }

    private void OnTenEnemyKills()
    {
        TryUnlock(m_Achievements[AchievementType.TEN_ENEMY_KILLS]);
    }

    private void OnFiveZeroGame()
    {
        m_PostGameAchievementQ.Enqueue(m_Achievements[AchievementType.FIVE_ZERO_GAME]);
    }

    // Unlock achievement if it hasn't been already
    private void TryUnlock(Achievement a)
    {
        if (!a.IsUnlocked)
        {
            a.Unlock();
            OnAchievementUnlocked(a);
            SoundManager.Instance.Play(m_AchievementSound);
        }
    }

    private void OnDisable()
    {
        // Remove listeners
        SceneManager.sceneLoaded -= OnSceneLoaded;

        DeathTrigger.OnPlayerOutOfBounds -= OnPlayerOutOfBounds;
        Breakable.OnAllObjectsBroken -= OnAllCratesBroken;
        AchievementCollectible.OnAllSpecialCollectiblesGrabbed -= OnAllSpikePlusesCollected;
        StatsTracker.OnFiftyPlusesCollected -= OnFiftyPlusesCollected;
        StatsTracker.OnTenThousandMetresMoved -= OnPlayerWalkedTenThousandMeters;
        StatsTracker.OnFivePlayerKills -= OnFivePlayerKills;
        StatsTracker.OnTenEnemyKills -= OnTenEnemyKills;
        StatsTracker.OnSurvivedLevelWithoutDeath -= OnSurvivedLevelWithoutDeath;
        StatsTracker.OnFiveZeroGame -= OnFiveZeroGame;
        StatsTracker.OnPlayerIndecency -= OnPlayerIndeceny;
        StatsTracker.OnPlayerSuicide -= OnPlayerSuicide;
        StatsTracker.OnPlayerStolePi -= OnPiStolen;
    }
}
