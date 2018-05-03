using UnityEngine;
using System.Collections;

public enum DirectionState
{
    North, West, South, East
}

/// <summary>
/// Utility class adding more functionality to the Direction
/// Usage: Call this specific class name to access the function.
/// Note: Couldn't extend the proper functionality to the enum itself.
/// </summary>
public static class DirectionExtension
{

    public static int GetDirectionPointX(DirectionState dir)
    {
        switch (dir)
        {
            case DirectionState.North:
                return 0;
            case DirectionState.East:
                return 1;
            case DirectionState.West:
                return -1;
            case DirectionState.South:
                return 0;
            default:
                return 0;
        }
    }

    public static int GetDirectionPointY(DirectionState dir)
    {
        switch (dir)
        {
            case DirectionState.North:
                return 1;
            case DirectionState.East:
                return 0;
            case DirectionState.West:
                return 0;
            case DirectionState.South:
                return -1;
            default:
                return 0;
        }
    }
}