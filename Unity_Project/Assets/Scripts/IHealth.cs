using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for objects with health
public interface IHealth
{

    void TakeDamage(int amount, GameObject attacker);

    void Die(GameObject killer);

    bool IsAlive();

}
