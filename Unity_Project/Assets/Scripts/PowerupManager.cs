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

    private GameObject m_Jetpack;

    private Animator m_JetpackAnimator;

    // --------------------------------------------------------------

    public bool HasJetpack
    {
        get
        {
            return m_Jetpack != null;
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
                Debug.Log("Player received Lightning Sprint!");
                Instantiate(m_LightningSprintPrefab, transform.GetChild(0));
                break;
            case 10:
                Debug.Log("Player received Jetpack!");
                m_Jetpack = Instantiate(m_JetpackPrefab, transform.GetChild(0)) as GameObject;
                m_JetpackAnimator = m_Jetpack.GetComponent<Animator>();
                break;
            default:
                Debug.Log("Player got an extra life!");
                m_Health.GetExtraLife();
                break;
        }
    }

    public void SetJetpackActive(bool active)
    {
        if (m_JetpackAnimator != null)
        {
            m_JetpackAnimator.SetBool("IsFloating", active);
        }
       
    }

    private void OnRemovePowerups(int playerNum)
    {
        if (playerNum != m_Player.PlayerNum) return;

        Destroy(m_Jetpack);
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= OnRemovePowerups;
    }
}
