using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public enum Type { PLUS, PI };

    // --------------------------------------------------------------

    [SerializeField] private Type type;

    // --------------------------------------------------------------

    // Events
    public delegate void CollectiblePickup(Type t, int playerNum);
    public static event CollectiblePickup OnCollectiblePickup;

    // --------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        PlayerController capturedBy = other.GetComponent<PlayerController>();
        if (capturedBy != null)
        {
            OnCollectiblePickup(type, capturedBy.PlayerNum);
            Vanish();
        }
    }

    private void Vanish()
    {
        // TODO: play vanish animation
        Destroy(gameObject);
    }
}
