﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Controller for Title Screen sub-menu
public class LevelSelectionMenu : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private Button[] m_LevelButtons;

    [SerializeField] private Button m_ExitButton;

    // --------------------------------------------------------------

    private void OnEnable()
    {
        foreach (Button button in m_LevelButtons)
        {
            button.interactable = true;
        }
        EventSystem.current.SetSelectedGameObject(m_LevelButtons[0].gameObject);
        m_ExitButton.interactable = true;
    }


    private void Update()
    {
        if (InputHelper.CancelButtonPressed())
        {
            m_ExitButton.onClick.Invoke();
        }
    }

}
