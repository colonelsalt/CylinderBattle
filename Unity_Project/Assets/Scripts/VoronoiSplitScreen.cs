using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiSplitScreen : MonoBehaviour
{
    // --------------------------------------------------------------

    [SerializeField] private float m_ActivationDistance = 9f;

    [SerializeField] private float m_CameraDistance = 20f;

    [SerializeField] private Camera m_PrimaryCamera;

    [SerializeField] private Camera m_SecondaryCamera;

    [SerializeField] private Transform m_Player1;

    [SerializeField] private Transform m_Player2;

    // --------------------------------------------------------------

    // Midpoint between Players
    private Vector3 m_Midpoint;

    private Renderer m_SplitScreenMask;

    private Renderer m_DividerLine;

    // Whether screen is currently split
    private bool m_SplitScreenActive = false;

    // Distance in front of secondary camera to place splitscreen mask
    private float m_MaskOffset;

    // --------------------------------------------------------------

    private void Awake()
    {
        m_SecondaryCamera.enabled = false;

        m_MaskOffset = m_PrimaryCamera.nearClipPlane + 0.1f;

        m_SplitScreenMask = GetComponentInChildren<Renderer>();
        m_DividerLine = m_SplitScreenMask.transform.GetChild(0).GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        // Set midpoint between Players
        m_Midpoint = (m_Player1.position + m_Player2.position) / 2f;

        // Check if Players are far enough apart to split screen
        float distanceFromMiddle = (m_Midpoint - m_Player1.position).magnitude;
        if (!m_SplitScreenActive && distanceFromMiddle >= m_ActivationDistance)
        {
            ActivateSplitScreen();
        }
        else if (m_SplitScreenActive && distanceFromMiddle < m_ActivationDistance)
        {
            DeactivateSplitScreen();
        }

        if (m_SplitScreenActive)
        {
            PositionCameras();
            PositionScreenMask();
        }
        else
        {
            // If splitscreen not active, just point camera at midpoint
            m_PrimaryCamera.transform.position = m_Midpoint - (m_PrimaryCamera.transform.forward * m_CameraDistance);
        }
    }

    private void ActivateSplitScreen()
    {
        m_SplitScreenActive = true;
        m_SecondaryCamera.enabled = true;

        m_SplitScreenMask.enabled = true;
        m_DividerLine.enabled = true;
    }

    private void DeactivateSplitScreen()
    {
        m_SplitScreenActive = false;
        m_SecondaryCamera.enabled = false;

        m_SplitScreenMask.enabled = false;
        m_DividerLine.enabled = false;
    }

    private void PositionCameras()
    {
        Vector3 cameraDisplacement = (m_Midpoint - m_Player1.position).normalized * m_ActivationDistance;

        m_PrimaryCamera.transform.position = m_Player1.position + cameraDisplacement - (m_PrimaryCamera.transform.forward * m_CameraDistance);
        m_SecondaryCamera.transform.position = m_Player2.position - cameraDisplacement - (m_SecondaryCamera.transform.forward * m_CameraDistance);
    }

    private void PositionScreenMask()
    {

        // Resize splitscreen mask to fill other half of screen
        float diagonalSlope = Mathf.Tan(m_SecondaryCamera.fieldOfView * 0.5f * Mathf.Deg2Rad) * m_SecondaryCamera.aspect;
        float scaleAmount = (2 * Mathf.Sqrt(2)) * m_MaskOffset * diagonalSlope;

        m_SplitScreenMask.transform.localScale = new Vector3(scaleAmount, scaleAmount, 1f);

        // Rotate mask based on split screen angle
        Vector3 cameraDisplacement = (m_Midpoint - m_Player1.position).normalized * m_ActivationDistance;
        Vector3 screenDisplacement = m_SecondaryCamera.WorldToScreenPoint(m_Player2.position) - m_SecondaryCamera.WorldToScreenPoint(m_Player2.position + cameraDisplacement);
        m_SplitScreenMask.transform.rotation = m_SecondaryCamera.transform.rotation;
        m_SplitScreenMask.transform.Rotate(m_SplitScreenMask.transform.forward, Mathf.Atan2(screenDisplacement.y, screenDisplacement.x) * Mathf.Rad2Deg, Space.World);

        // Place mask in front of camera
        m_SplitScreenMask.transform.position = m_SecondaryCamera.transform.position + (m_SecondaryCamera.transform.forward * m_MaskOffset) + (m_SplitScreenMask.transform.right * m_SplitScreenMask.transform.lossyScale.x * 0.5f);

    }

}
