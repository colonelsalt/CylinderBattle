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

    public void CheckForPlusBonus(int numPluses)
    {
        if (numPluses % 5 != 0) return;

        switch (numPluses)
        {
            case 10:
                // Receive Jetpack
                Instantiate(m_JetpackPrefab, transform.GetChild(0));
                Instantiate(m_PowerupEffects[(int)Powerup.JETPACK], transform);
                break;
            case 20:
                // Receive Lightning Sprint
                Instantiate(m_LightningSprintPrefab, transform.GetChild(0));
                Instantiate(m_PowerupEffects[(int)Powerup.LIGHTNING_SPRINT], transform);
                OnPowerupReceived(Powerup.LIGHTNING_SPRINT, m_Player.PlayerNum);
                break;
            default:
                Debug.Log("Player got an extra life!");
                Instantiate(m_PowerupEffects[(int)Powerup.EXTRA_LIFE], transform);
                m_Health.GetExtraLife();
                break;
        }

    }
}
