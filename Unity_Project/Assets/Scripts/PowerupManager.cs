using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Powerup
{
    EXTRA_LIFE = 0,
    QUICK_RUN_BOOTS = 1,
    JETPACK = 2
}

[RequireComponent(typeof(PlayerController))]
public class PowerupManager : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_QuickRunBootsPrefab;

    [SerializeField] private GameObject m_JetpackPrefab;

    // --------------------------------------------------------------

    private PlayerController m_Player;

    private PlayerHealth m_Health;

    private GameObject m_QuickRunBoots;

    private GameObject m_Jetpack;

    // --------------------------------------------------------------

    public bool HasJetpack
    {
        get
        {
            return m_Jetpack != null;
        }
    }

    public bool HasQuickRunBoots
    {
        get
        {
            return m_QuickRunBoots != null;
        }
    }

    // --------------------------------------------------------------

    private void Awake()
    {
        PlayerHealth.OnPlayerDeath += OnRemovePowerups;
        m_Player = GetComponent<PlayerController>();
        m_Health = GetComponent<PlayerHealth>();
    }

    public void CheckForPlusBonus(int numPluses)
    {
        if (numPluses % 5 != 0) return;

        switch (numPluses)
        {
            case 5:
                Debug.Log("Player received Jetpack!");
                m_Jetpack = Instantiate(m_JetpackPrefab, transform.GetChild(0)) as GameObject;
                break;
            case 10:
                break;
            case 20:
                Debug.Log("Player received Quick-Run boots!");
                m_QuickRunBoots = Instantiate(m_QuickRunBootsPrefab, transform.GetChild(0)) as GameObject;
                break;
            default:
                Debug.Log("Player got an extra life!");
                m_Health.GetExtraLife();
                break;
        }
    }

    private void OnRemovePowerups(int playerNum)
    {
        if (playerNum != m_Player.PlayerNum) return;

        Destroy(m_QuickRunBoots);
        Destroy(m_Jetpack);
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= OnRemovePowerups;
    }
}
