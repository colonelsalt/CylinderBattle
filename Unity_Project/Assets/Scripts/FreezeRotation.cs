using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    // --------------------------------------------------------------

    private Quaternion m_StartRotation;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_StartRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = m_StartRotation;
    }

}
