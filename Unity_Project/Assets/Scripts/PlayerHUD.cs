using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerHUD : MonoBehaviour {

    // --------------------------------------------------------------

    [SerializeField] private Text m_NumPisText;
    [SerializeField] private Text m_NumPlusesText;
    [SerializeField] private Image m_PowerupImage;

    // --------------------------------------------------------------

    private int m_NumPis = 0;
    private int m_NumPluses = 0;

    // --------------------------------------------------------------

    public void IncrementAndUpdatePis()
    {
        m_NumPis++;
        m_NumPisText.text = m_NumPis + "/" + GameManager.MAX_NUM_PIS;
    }

    public void IncrementAndUpdatePluses()
    {
        m_NumPluses++;
        // Update display
    }


}
