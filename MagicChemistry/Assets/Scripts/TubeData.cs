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

    protected Camera _cam;
    protected TubeSideData[] _sides;

    public AudioSource _audioSource;
    public float tileSize = 1;

    protected float _value;
    protected byte xCord;
    protected byte yCord;

    protected void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _cam = Camera.main;
        _sides = new TubeSideData[4];
        _sides[0] = new TubeSideData() { Direction = DirectionState.North, State = _North };
        _sides[1] = new TubeSideData() { Direction = DirectionState.South, State = _South };
        _sides[2] = new TubeSideData() { Direction = DirectionState.West, State = _West };
        _sides[3] = new TubeSideData() { Direction = DirectionState.East, State = _East };

        if (!CheckValidSides())
            Debug.Log(string.Format("GameObject: {0}\nTileData Script: Input Output State is not valid. Need at least 1 input and output",gameObject.name));
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

    public void SetValue(float value) { _value = value; }
    public void SetCoordinates(byte x, byte y) {
        xCord = x;
        yCord = y;
    }

    /// <summary>
    // Check if mouse is hovering over the tube
    /// </summary>
    /// <returns></returns>
    protected bool MouseOnMe()
    {
        float leftBound = transform.position.x - tileSize / 2f;
        float rightBound = transform.position.x + tileSize / 2f;
        float topBound = transform.position.y + tileSize / 2f;
        float botBound = transform.position.y - tileSize / 2f;
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x >= leftBound && mousePos.x <= rightBound
            && mousePos.y >= botBound && mousePos.y <= topBound)
        {
            return true;
        }
        return false;
    }


    #region Rotation
    public void RotateCounterClockWise()
    {
        transform.Rotate(Vector3.forward * 90);
        foreach (TubeSideData side in _sides)
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
        transform.Rotate(Vector3.forward * -90);
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
