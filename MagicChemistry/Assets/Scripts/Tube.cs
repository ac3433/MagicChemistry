using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tube : TubeData {

    protected bool _placed = false; // Tube starts out attached to the mouse cursor from dragging on the template.
    public LevelManager manager;
    public Text numberText;

    [SerializeField]
    protected GameObject gridTile; // Tile the tube is snapped to
  
    [SerializeField] protected GameObject[] masks;
    [SerializeField] protected float startDelaySec;
    [SerializeField] protected float maxTimeTillFill;
    [SerializeField] protected SpriteRenderer fill;
    protected float timeTillFill;

    protected float flowStartTime;
    protected float[] maskScale;
    protected bool flowing = false;
    protected bool filled = false;
    protected DirectionState inFlowSide;

    new void Start() {
        base.Start();
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        for(int i = 0; i < masks.Length; i++) {
            maskScale[i] = masks[i].transform.localScale.y;
        }
        _pickupSound.Play();
    }

    public void SetManager(LevelManager manager)
    {
        this.manager = manager;
        Debug.Log(manager);
    }

	// Update is called once per frame
	void Update () {
        // Tube attached to cursor before being placed
		if (!_placed)
        {
            Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            
            if (!_placed)
            {
                // Place the tube
                _placed = true;
                SnapToTile();
                _placeSound.Play();
            } 
        }
        if (Input.GetMouseButtonDown(0) && _placed && !flowing)
            {
                // Rotate the tube
                if (MouseOnMe())
                {
                    RotateClockwise();
                }
            }

        // Delete the tube
        if (Input.GetMouseButtonUp(1) && _placed && !flowing)
        {
            if (MouseOnMe())
            {
                manager.SetTile(xCord, yCord, gridTile);
                Destroy(gameObject);
            }
        }

        if (flowing && !filled)
        {
            if (timeTillFill > 0)
            {
                timeTillFill -= Time.deltaTime;
	        }
            else
            {
                CancelInvoke();
                FlowToNext();
                filled = true;
            }
        }
    }

    void SnapToTile()
    {
        GameObject[,] grid = manager.GetGrid();
        GameObject tile = FindOverlappedTile(grid);
        if (tile == null)
        {
            Destroy(gameObject);
            return;
        }

        // Place in the center of the empty tile
        Vector3 tileBounds = gridTile.GetComponentInChildren<SpriteRenderer>().bounds.size;
        Vector3 tilePos = gridTile.transform.position;
        float offsetX = tileBounds.x / 2;
        float offsetY = tileBounds.y / 2;
        transform.position = new Vector3(tilePos.x + offsetX, tilePos.y - offsetY, tilePos.z);
    }

    // Find the tile our mouse is over. If none is found, return null
    GameObject FindOverlappedTile(GameObject[,] grid)
    {
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                // Tile is occupied by a tube, ignore
                if (grid[i,j].GetComponent<TubeData>() != null)
                {
                    continue;
                }
                Vector3 tileBounds = grid[i,j].GetComponentInChildren<SpriteRenderer>().bounds.size;
                Vector3 tilePos = grid[i, j].transform.position;
                float xSize = tileBounds.x;
                float ySize = tileBounds.y;
                float xMin = tilePos.x;
                float yMax = tilePos.y;
                float xMax = tilePos.x + xSize;
                float yMin = tilePos.y - ySize;

                if (mousePos.x <= xMax && mousePos.x > xMin && mousePos.y <= yMax && mousePos.y > yMin)
                {
                    xCord = (byte)i;
                    yCord = (byte)j;
                    gridTile = grid[i,j];
                    manager.SetTile(i,j,gameObject);
                    return grid[i, j];
                }
            }
        }
        return null;
    }

    public virtual void FlowStart(DirectionState inFlowSide, int inVal, OperationState operation) {
        _flowSound.Play();
        _inValue = inVal;
        if (_operation == OperationState.None)
        {
            _operation = operation;
        }
        fill.color = Color.red;
        flowing = true;
        this.inFlowSide = inFlowSide;
        flowStartTime = Time.time;
        timeTillFill = maxTimeTillFill;
        if (_thisVal != 0 && _operation != OperationState.None)
        {
            // If this is a numbered tube, perform the stored operation on it
            _outValue = CalculateOperation(_inValue, _thisVal);
            _operation = OperationState.None;
        }
        else if (_thisVal != 0)
        {
            // No operation was between this number & the last number, so lose
            manager.GameOver();
        }
        else
        {
            // Otherwise, no operations are performed and the out value is passed the in value
            _outValue = _inValue;
        }

        InvokeRepeating("FlowTick", startDelaySec, 0.075f);
    }

    public virtual void FlowTick() {
        Debug.Log("flow pos: (" + xCord + ", " + yCord + ") | timer: " + timeTillFill + " | inVal: " + _inValue + " | op: " + _operation + "| thisVal: " + _thisVal + " | outVal: " + _outValue);
        float fracJourney = ((Time.time - flowStartTime) / maxTimeTillFill);
        //masks[0].transform.localScale = new Vector3(masks[0].transform.localScale.x,
        //                                        Mathf.Lerp(maskScale[0], 0.01f, fracJourney),
        //                                        masks[0].transform.localScale.z);
        //if (fracJourney > 1.0f) {
        //    FlowToNext();
        //    CancelInvoke("FlowTick");
        //}
    }

    public virtual void FlowToNext() {
        bool done = false;
        foreach (TubeSideData flowOut in _sides) {
            if (!done && (flowOut.State == InputOutputState.Both || flowOut.State == InputOutputState.Output) && flowOut.Direction != inFlowSide) {
                //get next grid tile in the valid tile's direction.
                bool valid = false;
                DirectionState flowTo = DirectionState.West;
                byte newX = xCord;
                byte newY = yCord;
                Debug.Log(newY);

                switch (flowOut.Direction) {
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
                if ((newX >= 0 && newX < grid.GetLength(0)) && (newY >= 0 && newY < grid.GetLength(1))) {
                    nextTube = grid[newX,newY].GetComponent<Tube>();
                    if (nextTube != null) {
                        Debug.Log("Should be here.");
                        Debug.Log("Target: " + flowTo);
                        for(int i = 0; i < nextTube._sides.Length; i++) {
                            Debug.Log(nextTube._sides[i].Direction);
                            Debug.Log(nextTube._sides[i].State);
                            if (nextTube._sides[i].Direction == flowTo && (nextTube._sides[i].State != InputOutputState.Output && nextTube._sides[i].State != InputOutputState.None)) {
                                valid = true;
                            }
                        }
                    }
                    
                }


                //If there is no tile or correct input side, check if we've reached the end of the grid with the right number. if so, win
                // else, end the game as our player has failed us (yet again).
                if (valid == false) {
                    Debug.Log("Game sucks.");
                    if (manager.CheckWinState(xCord, yCord, flowOut.Direction, _outValue))
                    {
                        manager.GameWin();
                    } else
                    {
                        manager.GameOver();
                    }
                } else {
                    //start the flow on that tile if a proper input/both side is connected. (Out=North, then In=South, etc.)
                    nextTube.FlowStart(flowTo, _outValue, _operation); 
                    done = true;
                }


            }
        }
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
