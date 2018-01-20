using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private AudioClip[] m_Sounds;

    // --------------------------------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlayRandom(m_Sounds);
    }

}
