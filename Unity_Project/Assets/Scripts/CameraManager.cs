using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages camera movement; toggles between midpoint-centred single camera and dynamically rotating split-screen (Voronoi) using shader mask
public class CameraManager : MonoBehaviour
{
    // --------------------------------------------------------------

    // How far apart Players have to be (from midpoint) for split screen to activate
    [SerializeField] private float m_SplitDistance = 9f;

    // Distance to maintain between Players and each camera
    [SerializeField] private float m_CameraDistance = 20f;

    // Distance in front of secondary camera to place render mask
    [SerializeField] private float m_MaskOffset;

    // Mask to place in front of secondary camera to display primary camera feed through
    [SerializeField] private GameObject m_SplitScreenMask;

    // Follows both Players when in range; Player 1 when split screen active
    [SerializeField] private Camera m_PrimaryCamera;

    // Deactivated when both Players in range; follows Player 2 when split screen active
    [SerializeField] private Camera m_SecondaryCamera;

    // References to Player Transforms to track positions
    [SerializeField] private Transform m_Player1;
    [SerializeField] private Transform m_Player2;

    // --------------------------------------------------------------

    // Midpoint between Players
    private Vector3 m_Midpoint;

    // Offset from Player to place camera to allow room for both screens
    private Vector3 m_CameraOffset;

    // Whether split screen is currently active
    private bool m_SplitScreenActive = false;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_SecondaryCamera.enabled = false;
    }

    private void LateUpdate()
    {
        m_Midpoint = (m_Player1.position + m_Player2.position) / 2f;

        // Check if Players are far enough apart to split screen
        float distanceFromMiddle = Vector3.Distance(m_Midpoint, m_Player1.position);
        if (!m_SplitScreenActive && distanceFromMiddle >= m_SplitDistance)
        {
            SetSplitScreenActive(true);
        }
        else if (m_SplitScreenActive && distanceFromMiddle < m_SplitDistance)
        {
            SetSplitScreenActive(false);
        }

        if (m_SplitScreenActive)
        {
            m_CameraOffset = (m_Midpoint - m_Player1.position).normalized * m_SplitDistance;

            PositionCameras();
            RotateScreenMask();
        }
        else
        {
            // If splitscreen not active, just place primary camera at midpoint
            m_PrimaryCamera.transform.position = m_Midpoint - (m_PrimaryCamera.transform.forward * m_CameraDistance);

            // Update secondary camera position as well (needed for correct screen space coordinates for TutorialManager)
            m_SecondaryCamera.transform.position = m_PrimaryCamera.transform.position;
        }
    }

    // Activate or de-activate secondary camera and render mask
    private void SetSplitScreenActive(bool active)
    {
        m_SplitScreenActive = active;
        m_SecondaryCamera.enabled = active;

        m_SplitScreenMask.SetActive(active);
    }


    private void PositionCameras()
    {
        m_PrimaryCamera.transform.position = m_Player1.position + m_CameraOffset - (m_PrimaryCamera.transform.forward * m_CameraDistance);

        // Subtract because Player 2 is in opposite direction
        m_SecondaryCamera.transform.position = m_Player2.position - m_CameraOffset - (m_SecondaryCamera.transform.forward * m_CameraDistance);
    }

    private void RotateScreenMask()
    {
        m_SplitScreenMask.transform.rotation = m_SecondaryCamera.transform.rotation;

        // Perpendicular between Player 2's corner of the screen and screen midpoint
        Vector3 halfScreenDiagonal = m_SecondaryCamera.WorldToScreenPoint(m_Midpoint) - m_SecondaryCamera.WorldToScreenPoint(m_Player2.position + m_CameraOffset);   

        // Rotate mask away from camera by angle (formed by diagonal and bottom screen horizontal line)
        m_SplitScreenMask.transform.Rotate(m_SplitScreenMask.transform.forward, Mathf.Atan2(halfScreenDiagonal.y, halfScreenDiagonal.x) * Mathf.Rad2Deg, Space.World);

        // Shift mask over by half its width so it covers half of secondary camera
        Vector3 horizontalShift = m_SplitScreenMask.transform.right * m_SplitScreenMask.transform.lossyScale.x / 2f;

        // Place mask in front of camera + shift
        m_SplitScreenMask.transform.position = m_SecondaryCamera.transform.position + (m_SecondaryCamera.transform.forward * m_MaskOffset) + horizontalShift; 
    }

}
