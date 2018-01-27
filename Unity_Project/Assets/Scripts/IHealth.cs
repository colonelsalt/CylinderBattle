using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{

    void TakeDamage(int amount, GameObject attacker);

    void Die(GameObject killer);

    bool IsAlive();

}
