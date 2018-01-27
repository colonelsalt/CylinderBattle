using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    // --------------------------------------------------------------

    private enum AchievementType
    {
        TEST,
        SURVIVOR,
        TEN_THOUSAND_METRES,
        FIFTY_PLUSES,
        TEN_KILLS,
        OUT_OF_BOUNDS,
        SUICIDE,
        INDECENCY
    }

    // --------------------------------------------------------------

    // Events
    public delegate void AchievementEvent(Achievement achievement);
    public static event AchievementEvent OnAchievementUnlocked;

    // --------------------------------------------------------------

    // DEBUG ONLY: If set, delete all achievement progress on game start
    [SerializeField] private bool m_DeleteAllOnStart = false;

    [SerializeField] private AudioClip m_AchievementSound;

    // --------------------------------------------------------------

    private static AchievementManager m_Instance;

    // List of all achievements
    private Dictionary<AchievementType, Achievement> m_Achievements = new Dictionary<AchievementType, Achievement>();

    private StatsTracker[] m_StatsTrackers;

    private bool m_GameRunning = false;

    // --------------------------------------------------------------

    public static AchievementManager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    public List<Achievement> AchievementList
    {
        get
        {
            return m_Achievements.Values.ToList();
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        if (m_DeleteAllOnStart)
        {
            PlayerPrefsManager.DeleteAll();
        }

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
        StatsTracker.OnFiftyPlusesCollected += OnFiftyPlusesCollected;
        StatsTracker.OnPlayerOutOfBounds += OnPlayerOutOfBounds;
        StatsTracker.OnTenThousandMetresMoved += OnPlayerWalkedTenThousandMeters;
        StatsTracker.OnTenPlayerKills += OnTenPlayerKills;

        m_Achievements[AchievementType.TEST] = new Achievement(
            "TEST",
            "Herp derp schmerp"
            );

        m_Achievements[AchievementType.SURVIVOR] = new Achievement(
            "Survivor",
            "Survive an entire game without dying"
            );

        m_Achievements[AchievementType.TEN_THOUSAND_METRES] = new Achievement(
            "Metric Mile",
            "Move more than 10,000m during the course of a game"
            );

        m_Achievements[AchievementType.FIFTY_PLUSES] = new Achievement(
            "Hoarder",
            "Collect 50 Pluses in one game without dying"
            );

        m_Achievements[AchievementType.TEN_KILLS] = new Achievement(
            "Ruthless",
            "Kill your fellow player ten times"
            );

        m_Achievements[AchievementType.SUICIDE] = new Achievement(
            "Suicide",
            "Kill yourself with your own bomb",
            true
            );

        m_Achievements[AchievementType.OUT_OF_BOUNDS] = new Achievement(
            "What are you doing here?",
            "Step outside the bounds of the level",
            true
            );

        m_Achievements[AchievementType.INDECENCY] = new Achievement(
            "Dirty boi",
            "Perform indecent actions on your fellow player",
            true
            );
    }

    // TODO: Remove this once done testing achievement system
    private void Update()
    {
        if (InputHelper.JumpButtonPressed(1))
        {
            TryUnlock(m_Achievements[AchievementType.TEST]);
        }
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

    private void OnTenPlayerKills()
    {
        TryUnlock(m_Achievements[AchievementType.TEN_KILLS]);
    }

    private void TryUnlock(Achievement a)
    {
        if (!a.IsUnlocked)
        {
            a.Unlock();
            OnAchievementUnlocked(a);
            SoundManager.Instance.Play(m_AchievementSound);
        }
    }
}
