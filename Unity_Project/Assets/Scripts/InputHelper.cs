using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public enum ButtonAction
{
    JUMP,
    FIRE,
    CROUCH,
    SPRINT,
    PAUSE,
    CANCEL
}

// Wrapper around XboxControllerInput and Unity Input classes
public static class InputHelper
{
    public const string PLAYER1_INPUT_STRING = "_P1";
    public const string PLAYER2_INPUT_STRING = "_P2";

    public static bool GamePadConnected(int playerNum)
    {
        return XCI.IsPluggedIn((XboxController)playerNum);
    }

    public static float GetMovementX(int playerNum)
    {
        if (GamePadConnected(playerNum))
        {
            return XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerNum);
        }
        else
        {
            return Input.GetAxis("Horizontal" + PlayerString(playerNum));
        }
    }

    public static float GetMovementY(int playerNum)
    {
        if (GamePadConnected(playerNum))
        {
            return XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerNum);
        }
        else
        {
            return Input.GetAxis("Vertical" + PlayerString(playerNum));
        }
    }

    public static float GetRightStickX(int playerNum)
    {
        return XCI.GetAxis(XboxAxis.RightStickX, (XboxController)playerNum);
    }

    public static float GetRightStickY(int playerNum)
    {
        return XCI.GetAxis(XboxAxis.RightStickY, (XboxController)playerNum);
    }

    public static bool FireButtonPressed(int playerNum)
    {
        if (GamePadConnected(playerNum))
        {
            return XCI.GetAxis(XboxAxis.RightTrigger, (XboxController)playerNum) > 0.5f;
        }
        else
        {
            return Input.GetButtonDown("Fire1" + PlayerString(playerNum));
        }
    }

    public static bool FireButtonReleased(int playerNum)
    {
        if (GamePadConnected(playerNum))
        {
            return XCI.GetAxis(XboxAxis.RightTrigger, (XboxController)playerNum) <= 0.5f;
        }
        else
        {
            return Input.GetButtonUp("Fire1" + PlayerString(playerNum));
        }
    }

    public static bool JumpButtonPressed(int playerNum)
    {
        if (GamePadConnected(playerNum))
        {
            return XCI.GetButtonDown(XboxButton.A, (XboxController)playerNum);
        }
        else
        {
            return Input.GetButtonDown("Jump" + PlayerString(playerNum));
        }
    }

    public static bool JumpButtonReleased(int playerNum)
    {
        if (GamePadConnected(playerNum))
        {
            return XCI.GetButtonUp(XboxButton.A, (XboxController)playerNum);
        }
        else
        {
            return Input.GetButtonUp("Jump" + PlayerString(playerNum));
        }
    }

    public static bool CrouchButtonPressed(int playerNum)
    {
        if (GamePadConnected(playerNum))
        {
            return XCI.GetAxis(XboxAxis.LeftTrigger, (XboxController)playerNum) > 0.8f;
        }
        else
        {
            return Input.GetButtonDown("Crouch" + PlayerString(playerNum));
        }
    }

    public static bool CrouchButtonRealeased(int playerNum)
    {
        if (GamePadConnected(playerNum))
        {
            return XCI.GetAxis(XboxAxis.LeftTrigger, (XboxController)playerNum) <= 0.8f;
        } 
        else
        {
            return Input.GetButtonUp("Crouch" + PlayerString(playerNum));
        }
    }

    public static bool SprintButtonPressed(int playerNum)
    {
        if (GamePadConnected(playerNum))
        {
            return XCI.GetButton(XboxButton.X, (XboxController)playerNum);
        }
        else
        {
            return Input.GetButton("Sprint" + PlayerString(playerNum));
        }
    }

    public static bool PauseButtonPressed()
    {
        return XCI.GetButtonDown(XboxButton.Start) || Input.GetButtonDown("Cancel");
    }

    public static bool GetButtonDown(ButtonAction button)
    {
        switch(button)
        {
            case ButtonAction.CANCEL:
                return XCI.GetButtonDown(XboxButton.B) || Input.GetButtonDown("Cancel");
            default:
                return false;
        }
    }

    public static string GetButtonName(ButtonAction button, int playerNum)
    {
        switch (button)
        {
            case ButtonAction.JUMP:
                if (GamePadConnected(playerNum)) return "A";
                return (playerNum == 1) ? "SPACE" : "RIGHT CTRL";
            case ButtonAction.FIRE:
                if (GamePadConnected(playerNum)) return "RT";
                return (playerNum == 1) ? "LEFT CTRL" : "RIGHT SHIFT";
            case ButtonAction.CROUCH:
                if (GamePadConnected(playerNum)) return "LT";
                return (playerNum == 1) ? "Z" : ".";
            case ButtonAction.SPRINT:
                if (GamePadConnected(playerNum)) return "X";
                return (playerNum == 1) ? "X" : "/";
            default:
                Debug.LogError("InputHelper: No button " + button + " found for playerNum " + playerNum);
                return "";
        }
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
