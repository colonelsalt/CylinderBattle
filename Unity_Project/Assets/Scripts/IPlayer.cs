using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic interface that abstracts away bare essential Player information for interaction with other components
public interface IPlayer
{

    int PlayerNum();

    int NumPis();

    Vector3 Position();

    // Okay, this one is a little hacky, but needed for achievement functionality
    GameObject GetGameObject();

}
