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

    [SerializeField] private GameObject m_LightningSprintPrefab;

    [SerializeField] private GameObject m_JetpackPrefab;

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
            case 5:
                Debug.Log("Player received Jetpack!");
                Instantiate(m_JetpackPrefab, transform.GetChild(0));
                break;
            case 10:
                Debug.Log("Player received Lightning Sprint!");
                Instantiate(m_LightningSprintPrefab, transform.GetChild(0));
                break;
            default:
                Debug.Log("Player got an extra life!");
                m_Health.GetExtraLife();
                break;
        }
    }
}
