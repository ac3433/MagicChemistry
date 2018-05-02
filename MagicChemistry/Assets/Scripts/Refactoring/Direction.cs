using UnityEngine;
using System.Collections;

public enum Direction
{
    None, North, South, East, West
}

/// <summary>
/// Utility class adding more functionality to the Direction
/// Usage: Call this specific class name to access the function.
/// Note: Couldn't extend the proper functionality to the enum itself.
/// </summary>
public static class DirectionExtensions
{
    private static Point north = new Point() { X = 0, Y = -1 };
    private static Point south = new Point() { X = 0, Y = 1 };
    private static Point east = new Point() { X = 1, Y = 0 };
    private static Point west = new Point() { X = -1, Y = 0 };
    private static Point none = new Point() { X = 0, Y = 0 };

    public static Point GetDirectionPoint(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return north;
            case Direction.East:
                return east;
            case Direction.West:
                return west;
            case Direction.South:
                return south;
            default:
                return none;
        }
    }

    public static Direction GetOppositeDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return Direction.South;
            case Direction.East:
                return Direction.West;
            case Direction.West:
                return Direction.East;
            case Direction.South:
                return Direction.North;
            default:
                return Direction.None;
        }
    }

    public static Direction RotateClockwise(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return Direction.East;
            case Direction.East:
                return Direction.South;
            case Direction.South:
                return Direction.West;
            case Direction.West:
                return Direction.North;
            default:
                return Direction.None;

        }
    }
}