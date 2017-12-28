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

    [SerializeField] private GameObject m_JetPackPrefab;

    // --------------------------------------------------------------

    private PlayerController m_Player;

    private PlayerHealth m_Health;

    private GameObject m_QuickRunBoots;

    private GameObject m_Jetpack;

    private bool m_HasQuickRunBoots = false;
    private bool m_HasJetPack = false;

    // --------------------------------------------------------------

    private void Awake()
    {
        PlayerHealth.OnPlayerDeath += OnRemovePowerups;
        m_Player = GetComponent<PlayerController>();
        m_Health = GetComponent<PlayerHealth>();
    }

    public void CheckForPlusBonus(int numPluses)
    {
        switch (numPluses)
        {
            case 5:
            case 15:
                Debug.Log("Player got an extra life!");
                m_Health.GetExtraLife();
                break;
            case 10:
                Debug.Log("Player received Quick-Run boots!");
                m_QuickRunBoots = Instantiate(m_QuickRunBootsPrefab, transform.GetChild(0)) as GameObject;
                m_HasQuickRunBoots = true;
                break;
            case 20:
                break;
        }
    }

    public bool IsSprinting()
    {
        return InputHelper.SprintButtonPressed(m_Player.PlayerNum) && m_HasQuickRunBoots;
    }

    private void OnRemovePowerups(int playerNum, int healthChange)
    {
        if (playerNum != m_Player.PlayerNum) return;

        m_HasJetPack = m_HasQuickRunBoots = false;
        Destroy(m_QuickRunBoots);
        Destroy(m_Jetpack);
    }
}
