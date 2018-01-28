using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for modularising Enemy AI behaviours 
public interface IEnemyBehaviour
{
    // Called when Player spotted
    void Execute();

    // Called when lost sight of Player and Enemy returns to default Waypoint patrolling
    void Disable();

}
