using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHelper
{
    public const string PLAYER1_INPUT_STRING = "_P1";
    public const string PLAYER2_INPUT_STRING = "_P2";

    public static bool GamePadConnected()
    {
        return (Input.GetJoystickNames().Length > 0);
    }

    public static float GetMovementX(int playerNum)
    {
        return Input.GetAxis("Horizontal" + PlayerString(playerNum));
    }

    public static float GetMovementY(int playerNum)
    {
        return Input.GetAxis("Vertical" + PlayerString(playerNum));
    }

    public static float GetRightStickX(int playerNum)
    {
        return Input.GetAxis("RightAxisX" + PlayerString(playerNum));
    }

    public static float GetRightStickY(int playerNum)
    {
        return Input.GetAxis("RightAxisY" + PlayerString(playerNum));
    }

    public static bool FireButtonPressed(int playerNum)
    {
        if (GamePadConnected())
        {
            return (Input.GetAxis("RightTriggerAxis" + PlayerString(playerNum)) > 0.5f);
        }
        return Input.GetButtonDown("Fire1" + PlayerString(playerNum));
    }

    public static bool FireButtonReleased(int playerNum)
    {
        if (GamePadConnected())
        {
            return (Input.GetAxis("RightTriggerAxis" + PlayerString(playerNum)) <= 0.5f);
        }
        return Input.GetButtonUp("Fire1" + PlayerString(playerNum));
    }

    public static bool JumpButtonPressed(int playerNum)
    {
        return Input.GetButtonDown("Jump" + PlayerString(playerNum));
    }

    public static bool JumpButtonReleased(int playerNum)
    {
        return Input.GetButtonUp("Jump" + PlayerString(playerNum));
    }

    public static bool CrouchButtonPressed(int playerNum)
    {
        if (GamePadConnected())
        {
            return (Input.GetAxis("LeftTriggerAxis" + PlayerString(playerNum)) > 0.8f);
        }
        return Input.GetButtonDown("Crouch" + PlayerString(playerNum));
    }

    public static bool CrouchButtonRealeased(int playerNum)
    {
        if (GamePadConnected())
        {
            return (Input.GetAxis("LeftTriggerAxis" + PlayerString(playerNum)) <= 0.8f);
        }
        return Input.GetButtonUp("Crouch" + PlayerString(playerNum));
    }


    private static string PlayerString(int playerNum)
    {
        switch(playerNum)
        {
            case 1:
                return PLAYER1_INPUT_STRING;
            case 2:
                return PLAYER2_INPUT_STRING;
            default:
                return "";
               
        }
    }
	
}
