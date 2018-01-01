using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] float m_FadeTime = 5f;

    // --------------------------------------------------------------

    private void Update()
    {
        m_FadeTime -= Time.deltaTime;
        if (m_FadeTime <= 0)
        {
            Fade();
        }
    }

    private void Fade()
    {
        Destroy(gameObject);
    }
}
