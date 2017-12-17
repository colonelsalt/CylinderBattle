using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus : MonoBehaviour
{
    // --------------------------------------------------------------

    // Events
    public delegate void PlusCapture(int playerNum);
    public static event PlusCapture OnPlusCaptured;

    // --------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        PlayerController capturedBy = other.GetComponent<PlayerController>();
        if (capturedBy != null)
        {
            OnPlusCaptured(capturedBy.GetPlayerNum());
            Vanish();
        }
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}
