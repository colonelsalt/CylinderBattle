using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fired from PortalGun; teleports anything in scene it triggers against between its two endpoints
public class Portal : MonoBehaviour
{

    // --------------------------------------------------------------

    public enum Type { FIRST, SECOND }

    // Keep static ref. to each portal in scene (allow only 2 at one time)
    private static Portal[] PORTALS_IN_PLAY = new Portal[2];

    // --------------------------------------------------------------

    [SerializeField] private Type m_Type;

    [SerializeField] private AudioClip[] m_SpawnSounds;

    [SerializeField] private AudioClip[] m_TransportSounds;

    [SerializeField] private AudioClip[] m_DeactivationSounds;

    // --------------------------------------------------------------

    // Reference to Trasform of Portal this one connects to
    private Transform m_SisterPortal;

    private Animator m_Animator;

    private bool m_PortalActivated = false;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();

        if (m_Type == Type.FIRST)
        {
            // Destroy all previously fired portals in scene
            DeactivateAll();

            PORTALS_IN_PLAY[0] = this;
        }
        else
        {
            PORTALS_IN_PLAY[1] = this;

            // If other portal already fired, activate link between them
            Activate();
            PORTALS_IN_PLAY[0].Activate();
        }
    }

    private void Start()
    {
        SoundManager.Instance.PlayRandom(m_SpawnSounds);
    }

    private void Activate()
    {
        int sisterIndex = (m_Type == Type.FIRST) ? 1 : 0;

        m_SisterPortal = PORTALS_IN_PLAY[sisterIndex].transform;
        m_PortalActivated = true;
    }

    public void Deactivate()
    {
        SoundManager.Instance.PlayRandom(m_DeactivationSounds);
        m_Animator.SetTrigger("deactivationTrigger");
        Destroy(gameObject, 1f);
    }

    public static void DeactivateAll()
    {
        if (PORTALS_IN_PLAY[0] != null) PORTALS_IN_PLAY[0].Deactivate();
        if (PORTALS_IN_PLAY[1] != null) PORTALS_IN_PLAY[1].Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_PortalActivated) return;

        if (other.gameObject.tag != "Wall" && other.GetComponent<PlayerFeet>() == null)
        {
            SoundManager.Instance.PlayRandom(m_TransportSounds);
            other.transform.position = m_SisterPortal.position + m_SisterPortal.forward;
            other.transform.rotation = m_SisterPortal.rotation;
        }
    }
}
