using UnityEngine;
using System.Collections;

public class TubeDataOperation : TubeData
{
    [SerializeField]
    private OperationState _operation = OperationState.None;

    void Start()
    {
        _sides = new TubeSideData[4];
        _sides[0] = new TubeSideData() { Direction = DirectionState.North, State = _North };
        _sides[1] = new TubeSideData() { Direction = DirectionState.South, State = _South };
        _sides[2] = new TubeSideData() { Direction = DirectionState.West, State = _West };
        _sides[3] = new TubeSideData() { Direction = DirectionState.East, State = _East };

        if (CheckValidSides(2))
            Debug.Log(string.Format("GameObject: %s\nTileDataOperation Script: Input Output State is not valid. Need at least 2 input and output"));
        if(_operation == OperationState.None)
            Debug.Log(string.Format("GameObject: %s\nTileDataOperation Script: Missing operation."));

    }

    public int FindInputOperationValue()
    {
        int value = 0;
        //input at max has to be less than the total side given there is an output
        TubeSideData[] sortOrder = new TubeSideData[_sides.Length - 1];

        foreach(TubeSideData side in _sides)
        {
            if(side.State == InputOutputState.Input)
            {

            }
        }


        return value;
    }

    private int CalculateOperation(int v1, int v2)
    {
        switch (_operation)
        {
            case OperationState.Addition:
                return v1 + v2;

            case OperationState.Subtraction:
                return v1 - v2;

            case OperationState.Multiplication:
                return v1 * v2;

            case OperationState.Division:
                return v1 / v2;

            default:
                return 0;
        }
    }

}
