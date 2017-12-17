using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pi : MonoBehaviour
{
    // --------------------------------------------------------------

    // Events
    public delegate void PiCaptured(int playerNum);
    public static event PiCaptured OnPiCaptured;

    // --------------------------------------------------------------
    private const int MAX_NUM_PIS = 2;
    private static int numPisInPlay = 0;
    // --------------------------------------------------------------

    private void Awake()
    {
        if (numPisInPlay >= MAX_NUM_PIS)
        {
            Destroy(gameObject);
        }
        else
        {
            numPisInPlay++;
        }
    }

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
        numPisInPlay--;
        Destroy(gameObject);
    }
}
