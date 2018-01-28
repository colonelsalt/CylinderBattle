using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    // --------------------------------------------------------------

    public delegate void DroppedItemEvent(IPlayer receiver);
    public static event DroppedItemEvent OnDroppedItemPickedUp;

    // --------------------------------------------------------------

    private IPlayer m_Owner;

    // --------------------------------------------------------------

    public void AssignOwner(IPlayer owner)
    {

    }

}
