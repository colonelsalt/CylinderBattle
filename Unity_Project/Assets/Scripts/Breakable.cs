using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private GameObject m_ShatteredVersion;

    // How large change in object's velocity in one frame should be for it to break 
    [SerializeField] private float m_BreakThreshold;

    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_BreakSounds;

    // --------------------------------------------------------------

    private Rigidbody m_Body;

    private Vector3 m_LastVelocity;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        Vector3 velocityChange = m_LastVelocity - m_Body.velocity;
        if (velocityChange.magnitude > m_BreakThreshold)
        {
            Break();
        }
        m_LastVelocity = m_Body.velocity;
    }

    private void Break()
    {
        SoundManager.Instance.PlayRandom(m_BreakSounds);
        Instantiate(m_ShatteredVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
