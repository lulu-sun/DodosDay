using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Controls
{
    public static bool GetSelectKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
    }

    public static bool GetDeselectKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    public static bool GetRightKeyDown()
    {
        return Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
    }

    public static bool GetLeftKeyDown()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
    }

    public static bool GetUpKeyDown()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
    }

    public static bool GetDownKeyDown()
    {
        return Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
    }
}
