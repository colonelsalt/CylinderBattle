using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialMenu : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private MenuManager m_MenuManager;

    [SerializeField] private Sprite[] m_TutorialSlides;

    [SerializeField] private Button m_ArrowButton;

    [SerializeField] private Button m_ExitButton;

    [SerializeField] private Text m_NextSlideDescription;

    // --------------------------------------------------------------

    private Image m_Image;

    private int m_SlideIndex = 0;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_ExitButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_ArrowButton.gameObject.SetActive(true);
        m_ArrowButton.interactable = true;
        EventSystem.current.SetSelectedGameObject(m_ArrowButton.gameObject);
        ShowImage();

    }

    private void ShowImage()
    {
        switch (m_SlideIndex)
        {
            case 0:
                m_ExitButton.gameObject.SetActive(false);
                m_NextSlideDescription.text = "Controls";
                break;
            case 1:
                m_NextSlideDescription.text = "Gameplay tips";
                break;
            case 2:
                m_ArrowButton.interactable = false;
                m_ArrowButton.gameObject.SetActive(false);
                m_ExitButton.gameObject.SetActive(true);
                m_ExitButton.interactable = true;
                EventSystem.current.SetSelectedGameObject(m_ExitButton.gameObject);
                break;
        }

        m_Image.sprite = m_TutorialSlides[m_SlideIndex];
    }

    public void OnArrowButtonPressed()
    {
        m_MenuManager.ButtonSound();
        m_SlideIndex = (m_SlideIndex + 1) % 3;

        ShowImage();
    }

    public void OnExitButtonPressed()
    {
        m_SlideIndex = 0;
        m_ExitButton.gameObject.SetActive(false);

        m_MenuManager.OnDismissSubMenu();
    }

    private void Update()
    {
        if (InputHelper.GetButtonDown(ButtonAction.CANCEL))
        {
            if (m_SlideIndex == 2)
            {
                OnExitButtonPressed();
            }
            else
            {
                m_MenuManager.OnDismissSubMenu();
            }
        }
    }





}
