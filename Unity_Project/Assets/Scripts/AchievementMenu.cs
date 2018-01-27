using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AchievementMenu : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private Transform m_AchievementsGrid;

    [SerializeField] private GameObject m_AchievementBlockPrefab;

    // Background colour of unlocked achievements
    [SerializeField] private Color m_UnlockedColour;

    [SerializeField] private Button m_ExitButton;

    // --------------------------------------------------------------


    private void OnEnable()
    {
        m_ExitButton.interactable = true;
        EventSystem.current.SetSelectedGameObject(m_ExitButton.gameObject);
    }

    private void Awake()
    {
        List<Achievement> achivements = AchievementManager.Instance.AchievementList;

        foreach (Achievement a in achivements)
        {
            // Create achievement block and insert it into menu grid
            GameObject displayBlock = Instantiate(m_AchievementBlockPrefab, m_AchievementsGrid) as GameObject;
            displayBlock.transform.SetParent(m_AchievementsGrid);

            // Give block different colour if achievement unlocked
            if (a.IsUnlocked)
            {
                displayBlock.GetComponent<Image>().color = m_UnlockedColour;
            }

            // Assign achievement icon
            Sprite icon;
            if (a.IsUnlocked) icon = a.Icon ?? Achievement.DefaultIcon;
            else if (a.IsHidden) icon = Achievement.HiddenIcon;
            else icon = Achievement.LockedIcon;
            displayBlock.transform.GetChild(0).GetComponent<Image>().sprite = icon;

            // Assign achievement title
            string title;
            if (a.IsUnlocked) title = a.Title;
            else if (a.IsHidden) title = "Hidden";
            else title = "Locked";
            displayBlock.transform.GetChild(1).GetComponent<Text>().text = title;

            // Assign achievement description
            displayBlock.transform.GetChild(2).GetComponent<Text>().text = (a.IsHidden) ? string.Empty : a.Description;
        }
    }

    private void Update()
    {
        if (InputHelper.GetButtonDown(ButtonAction.CANCEL))
        {
            m_ExitButton.onClick.Invoke();
        }
    }

}
