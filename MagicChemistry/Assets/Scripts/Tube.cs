using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : TubeData {

    protected bool _placed = false; // Tube starts out attached to the mouse cursor from dragging on the template.
    public LevelManager manager;

    [SerializeField]
    protected GameObject gridTile; // Tile the tube is snapped to

    [SerializeField] protected GameObject[] mask;
    [SerializeField] protected float startDelaySec;
    [SerializeField] protected float maxTimeTillFill;

    protected float flowStartTime;
    protected float maskScale;
    protected bool flowing = false;
    protected DirectionState inFlowSide;

    new void Start() {
        base.Start();
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
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
            } else
            {
                // Rotate the tube
                if (MouseOnMe())
                {
                    RotateClockwise();
                }
            }
            
        }

        // Delete the tube
        if (Input.GetMouseButtonUp(1) && _placed)
        {
            if (MouseOnMe())
            {
                Destroy(gameObject);
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
        Vector3 tileBounds = gridTile.GetComponent<SpriteRenderer>().bounds.size;
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

    public void FlowStart() {
        flowStartTime = Time.time;
        InvokeRepeating("FlowTick", 0.0f, 0.075f);
    }

    protected void FlowTick() {
        Debug.Log("We flowin'");
    }

    protected void FlowToNext() {
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

                
                //If there is no tile or correct input side, end the game as our player has failed us (yet again).
                if (valid == false) {
                    //EndGame();
                    Debug.Log("Game sucks.");
                } else {
                    //start the flow on that tile if a proper input/both side is connected. (Out=North, then In=South, etc.)
                    nextTube.FlowStart();
                    nextTube.inFlowSide = flowTo;  
                    done = true;
                }


            }
        }
    }
}
