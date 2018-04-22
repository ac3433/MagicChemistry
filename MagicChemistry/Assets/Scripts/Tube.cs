using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : TubeData {

    protected bool _placed = false; // Tube starts out attached to the mouse cursor from dragging on the template.
    protected LevelManager manager;

    [SerializeField]
    protected GameObject gridTile; // Tile the tube is snapped to

    new void Start() {
        base.Start();
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    public void SetManager(LevelManager manager)
    {
        this.manager = manager;
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
        Vector3 tileBounds = tile.GetComponent<SpriteRenderer>().bounds.size;
        Vector3 tilePos = tile.transform.position;
        float offsetX = tileBounds.x / 2;
        float offsetY = tileBounds.y / 2;
        transform.position = new Vector3(tilePos.x + offsetX, tilePos.y - offsetY, tilePos.z);
        gridTile = tile;
    }

    // Find the tile our mouse is over. If none is found, return null
    GameObject FindOverlappedTile(GameObject[,] grid)
    {
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Vector3 tileBounds = grid[i,j].GetComponent<SpriteRenderer>().bounds.size;
                Vector3 tilePos = grid[i, j].transform.position;
                float xSize = tileBounds.x;
                float ySize = tileBounds.y;
                float xMin = tilePos.x;
                float yMax = tilePos.y;
                float xMax = tilePos.x + xSize;
                float yMin = tilePos.y - ySize;

                if (mousePos.x <= xMax && mousePos.x > xMin && mousePos.y <= yMax && mousePos.y > yMin)
                {
                    return grid[i, j];
                }
            }
        }
        return null;
    }
}
