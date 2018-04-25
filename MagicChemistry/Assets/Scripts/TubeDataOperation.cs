using UnityEngine;
using System.Collections;

public class TubeDataOperation : Tube
{
    [SerializeField]
    private OperationState _operation = OperationState.None;

    new void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _sides = new TubeSideData[4];
        _cam = Camera.main;
        _sides[0] = new TubeSideData() { Direction = DirectionState.North, State = _North };
        _sides[1] = new TubeSideData() { Direction = DirectionState.South, State = _South };
        _sides[2] = new TubeSideData() { Direction = DirectionState.West, State = _West };
        _sides[3] = new TubeSideData() { Direction = DirectionState.East, State = _East };

        if (CheckValidSides(2))
            Debug.Log(string.Format("GameObject: {0}\nTileDataOperation Script: Input Output State is not valid. Need at least 2 input and output",gameObject.name));
        if(_operation == OperationState.None)
            Debug.Log(string.Format("GameObject: {0}\nTileDataOperation Script: Missing operation.",gameObject.name));

    }

    //terrible sorting method
    public int FindInputOperationValue()
    {
        int value = 0;
        //input at max has to be less than the total side given there is an output
        TubeSideData[] sortOrder = new TubeSideData[_sides.Length - 1];

        foreach(TubeSideData side in _sides)
        {
            if(side.State == InputOutputState.Input)
            {
                switch(side.Direction)
                {
                    case DirectionState.West:
                        sortOrder[0] = side;
                        break;
                    case DirectionState.North:
                        sortOrder[1] = side;
                        break;
                    case DirectionState.South:
                        sortOrder[2] = side;
                        break;
                    case DirectionState.East:
                        sortOrder[3] = side;
                        break;
                }
            }
        }

        value = sortOrder[0].incomingValue;

        for(int i = 1; i < sortOrder.Length; i++)
        {
            value = CalculateOperation(value, sortOrder[i].incomingValue);
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
