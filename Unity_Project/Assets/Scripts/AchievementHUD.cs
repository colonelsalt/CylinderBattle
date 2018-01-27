using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementHUD : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_NotificationPrefab;

    // --------------------------------------------------------------

    private void Awake()
    {
        AchievementManager.OnAchievementUnlocked += OnAchievementUnlocked;
    }

    private void OnAchievementUnlocked(Achievement a)
    {
        GameObject notification = Instantiate(m_NotificationPrefab, transform) as GameObject;

        notification.transform.GetChild(2).GetComponent<Image>().sprite = a.Icon ?? Achievement.DefaultIcon;
        notification.transform.GetChild(3).GetComponent<Text>().text = a.Title;

        notification.GetComponent<Animator>().SetTrigger("showTrigger");

        Destroy(notification, 5f);
    }

    private void OnDisable()
    {
        AchievementManager.OnAchievementUnlocked -= OnAchievementUnlocked;
    }
}
