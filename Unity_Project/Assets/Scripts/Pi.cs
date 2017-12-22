﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pi : MonoBehaviour
{
    // --------------------------------------------------------------

    // Events
    public delegate void PiCaptured(int playerNum);
    public static event PiCaptured OnPiCaptured;

    // --------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        PlayerController capturedBy = other.GetComponent<PlayerController>();
        if (capturedBy != null)
        {
            OnPiCaptured(capturedBy.GetPlayerNum());
            Vanish();    
        }
    }

    private void Vanish()
    {
        // play vanish animation
        Destroy(gameObject);
    }
}
