using UnityEngine;
using System.Collections;

public class TubeOperation : Tube, IFlowable
{
    [SerializeField]
    private OperationState _operation = OperationState.None;
    public int numInputs;
    private DirectionState[] _inFlowSides;
    private int[] _inputVals;
    int numInputsFilled = 0;

    new void Start()
    {
        _inFlowSides = new DirectionState[numInputs];
        _inputVals = new int[numInputs];
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

        value = sortOrder[0].IncomingValue;

        for(int i = 1; i < sortOrder.Length; i++)
        {
            value = CalculateOperation(value, sortOrder[i].IncomingValue);
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

    public override void FlowStart(DirectionState inFlowSide, int val)
    {
        
        _inFlowSides[numInputsFilled] = inFlowSide;
        _inputVals[numInputsFilled] = val;
        numInputsFilled++;

        // If all inputs are filled, continue
        if (numInputsFilled == numInputs)
        {
            flowing = true;
            for (int i = 0; i < numInputsFilled-1; i++)
            {
                _value = CalculateOperation(_inputVals[i], _inputVals[i + 1]);
            }
            flowStartTime = Time.time;
            timeTillFill = maxTimeTillFill;
            InvokeRepeating("FlowTick", 0.0f, 1f);
        }
    }

    public override void FlowTick()
    {
        base.FlowTick();
    }

    bool NotInputSide(DirectionState dir)
    {
        for (int i = 0; i < _inFlowSides.Length; i++)
        {
            if (_inFlowSides[i] == dir)
            {
                return false;
            }
        }
        return true;
    }

    public override void FlowToNext()
    {
        bool done = false;
        foreach (TubeSideData flowOut in _sides)
        {
            if (!done && (flowOut.State == InputOutputState.Both || flowOut.State == InputOutputState.Output) && NotInputSide(flowOut.Direction))
            {
                //get next grid tile in the valid tile's direction.
                bool valid = false;
                DirectionState flowTo = DirectionState.West;
                byte newX = xCord;
                byte newY = yCord;
                Debug.Log(newY);

                switch (flowOut.Direction)
                {
                    case DirectionState.North:
                        flowTo = DirectionState.South;
                        newY--;
                        break;
                    case DirectionState.South:
                        flowTo = DirectionState.North;
                        newY++;
                        break;
                    case DirectionState.East:
                        flowTo = DirectionState.West;
                        newX++;
                        break;
                    case DirectionState.West:
                        flowTo = DirectionState.East;
                        newX--;
                        break;
                    default:
                        Debug.Log("It's all on fire...");
                        break;
                }

                //Coordinate validation and validation of input flow.
                GameObject[,] grid = manager.GetGrid();
                Tube nextTube = null;
                if ((newX >= 0 && newX < grid.GetLength(0)) && (newY >= 0 && newY < grid.GetLength(1)))
                {
                    nextTube = grid[newX, newY].GetComponent<Tube>();
                    if (nextTube != null)
                    {
                        Debug.Log("Should be here.");
                        Debug.Log("Target: " + flowTo);
                        TubeSideData[] sides = nextTube.GetSides();
                        for (int i = 0; i < sides.Length; i++)
                        {
                            Debug.Log(sides[i].Direction);
                            Debug.Log(sides[i].State);
                            if (sides[i].Direction == flowTo && (sides[i].State != InputOutputState.Output && sides[i].State != InputOutputState.None))
                            {
                                valid = true;
                            }
                        }
                    }

                }


                //If there is no tile or correct input side, end the game as our player has failed us (yet again).
                if (valid == false)
                {
                    Debug.Log("Game sucks.");
                    if (manager.CheckWinState(xCord, yCord, flowOut.Direction, _value))
                    {
                        manager.GameWin();
                    }
                    else
                    {
                        manager.GameOver();
                    }
                }
                else
                {
                    //start the flow on that tile if a proper input/both side is connected. (Out=North, then In=South, etc.)
                    nextTube.FlowStart(flowTo, _value);
                    done = true;
                }


            }
        }
    }
}
