using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] float m_DelayBeforeFade = 5f;

    [SerializeField] float m_FadeTime = 3f;

    [SerializeField] bool m_AutoStart = true;

    // --------------------------------------------------------------

    private bool m_HasStarted;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_HasStarted = m_AutoStart;
    }

    public void Begin()
    {
        m_HasStarted = true;
    }

    private void Update()
    {
        if (m_HasStarted)
        {
            m_DelayBeforeFade -= Time.deltaTime;
            if (m_DelayBeforeFade <= 0)
            {
                StartCoroutine(FadeAway());
            }
        }
    }

    private IEnumerator FadeAway()
    {
        Renderer rend = GetComponent<Renderer>();
        float remainingFadeTime = m_FadeTime;
        while (remainingFadeTime > 0f)
        {
            Color colour = rend.material.color;
            colour.a = remainingFadeTime / m_FadeTime;
            rend.material.color = colour;

            remainingFadeTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
