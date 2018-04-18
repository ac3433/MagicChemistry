using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeData : MonoBehaviour {

    [SerializeField]
    protected InputOutputState _North;
    [SerializeField]
    protected InputOutputState _South;
    [SerializeField]
    protected InputOutputState _West;
    [SerializeField]
    protected InputOutputState _East;

    protected TubeSideData[] _sides;


    private void Start()
    {
        _sides = new TubeSideData[4];
        _sides[0] = new TubeSideData() { Direction = DirectionState.North, State = _North };
        _sides[1] = new TubeSideData() { Direction = DirectionState.South, State = _South };
        _sides[2] = new TubeSideData() { Direction = DirectionState.West, State = _West };
        _sides[3] = new TubeSideData() { Direction = DirectionState.East, State = _East };

        if (CheckValidSides())
            Debug.Log(string.Format("GameObject: %s\nTileData Script: Input Output State is not valid. Need at least 1 input and output"));
    }

    public void AssignInputOutputStateSide(DirectionState dir, InputOutputState io)
    {
        foreach(TubeSideData side in _sides)
        {
            if (side.Direction == dir)
                side.State = io;
        }
    }

    /// <summary>
    /// Check if there is an input and an output of the tile
    /// </summary>
    /// <returns></returns>
    public bool CheckValidSides(byte minimalInput = 1)
    {
        byte input = 0;
        byte output = 0;

        foreach(TubeSideData side in _sides)
        {
            if (side.State == InputOutputState.Input)
                input++;
            if (side.State == InputOutputState.Output)
                output++;
        }

        //a minimum of 1 input of the side
        if (input < minimalInput)
            return false;
        if (output != 1)
            return false;

        return true;
    }


    #region Rotation
    public void RotateCounterClockWise()
    {
        foreach(TubeSideData side in _sides)
        {
            switch (side.Direction)
            {
                case DirectionState.North:
                    side.Direction = DirectionState.West;
                    break;
                case DirectionState.West:
                    side.Direction = DirectionState.South;
                    break;
                case DirectionState.South:
                    side.Direction = DirectionState.East;
                    break;
                case DirectionState.East:
                    side.Direction = DirectionState.North;
                    break;
            }

        }
    }

    public void RotateClockwise()
    {
        foreach (TubeSideData side in _sides)
        {
            switch (side.Direction)
            {
                case DirectionState.North:
                    side.Direction = DirectionState.East;
                    break;
                case DirectionState.East:
                    side.Direction = DirectionState.South;
                    break;
                case DirectionState.South:
                    side.Direction = DirectionState.West;
                    break;
                case DirectionState.West:
                    side.Direction = DirectionState.North;
                    break;
            }
        }
    }
#endregion Rotation
}
