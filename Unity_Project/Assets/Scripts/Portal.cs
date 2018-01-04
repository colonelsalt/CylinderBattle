using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    // --------------------------------------------------------------

    public enum Type { GREEN_PORTAL, PURPLE_PORTAL }

    // --------------------------------------------------------------

    [SerializeField] private Type m_Type;

    // --------------------------------------------------------------

    private Transform m_OtherPortal;

    private bool m_PortalActivated = false;

    // --------------------------------------------------------------

    private void Awake()
    {
        if (m_Type == Type.GREEN_PORTAL)
        {
            // Destroy all previously fired portals in scene
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal != this) portal.Deactivate();
            }
        }
    }


    public void AttachToPortal(Transform otherPortal)
    {
        m_OtherPortal = otherPortal;
        m_PortalActivated = true;
    }

    public void Deactivate()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_PortalActivated) return;

        if (other.gameObject.tag != "Wall")
        {
            other.transform.position = m_OtherPortal.transform.position + m_OtherPortal.transform.forward;
            other.transform.rotation = m_OtherPortal.transform.rotation;
        }
    }
}
