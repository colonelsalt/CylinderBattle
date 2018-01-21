using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Powerup
{
    EXTRA_LIFE = 0,
    LIGHTNING_SPRINT = 1,
    JETPACK = 2
}

[RequireComponent(typeof(PlayerController))]
public class PowerupManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_LightningSprintPrefab;

    [SerializeField] private GameObject m_JetpackPrefab;

    [SerializeField] private GameObject[] m_PowerupEffects;

    // --------------------------------------------------------------

    // Sounds
    [SerializeField] private AudioClip[] m_PowerupSounds;

    // --------------------------------------------------------------

    // Events
    public delegate void PowerupEvent(Powerup type, int playerNum);
    public static event PowerupEvent OnPowerupReceived;

    // --------------------------------------------------------------

    private PlayerController m_Player;

    private PlayerHealth m_Health;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Player = GetComponent<PlayerController>();
        m_Health = GetComponent<PlayerHealth>();
    }

    // Called at every Plus collection
    public void CheckForPlusBonus(int numPluses)
    {
        // Get bonus only for each 10 Pluses collected
        if (numPluses % 10 != 0) return;

        SoundManager.Instance.PlayRandom(m_PowerupSounds);

        switch (numPluses)
        {
            case 10:
                // Receive Jetpack
                Instantiate(m_JetpackPrefab, transform.GetChild(0));
                Instantiate(m_PowerupEffects[(int)Powerup.JETPACK], transform);
                OnPowerupReceived(Powerup.JETPACK, m_Player.PlayerNum);
                break;
            case 30:
                // Receive Lightning Sprint
                Instantiate(m_LightningSprintPrefab, transform.GetChild(0));
                Instantiate(m_PowerupEffects[(int)Powerup.LIGHTNING_SPRINT], transform);
                OnPowerupReceived(Powerup.LIGHTNING_SPRINT, m_Player.PlayerNum);
                break;
            default:
                // Get extra life
                Instantiate(m_PowerupEffects[(int)Powerup.EXTRA_LIFE], transform);
                OnPowerupReceived(Powerup.EXTRA_LIFE, m_Player.PlayerNum);
                m_Health.GetExtraLife();
                break;
        }

    }
}
