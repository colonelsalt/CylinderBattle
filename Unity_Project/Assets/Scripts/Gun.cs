using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    // --------------------------------------------------------------
    [SerializeField] private GameObject m_LaserPrefab;
    // --------------------------------------------------------------

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            // Fire laser
        }
    }

}
