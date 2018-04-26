﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct GridPlacement
{
    public int xCoordinate;
    public int yCoordinate;
    [Tooltip("Select the sprite from the tile list")]
    public int spriteID;
}

public class LevelManager : MonoBehaviour {
    
    [SerializeField]
    private GameObject[] _tile;

    [SerializeField]
    private GameObject _startPosition;

    [SerializeField]
    private int _flowStartX;
    [SerializeField]
    private int _flowStartY;
    [SerializeField]
    private DirectionState _flowStartDirection;

    [SerializeField]
    private int _flowEndX;
    [SerializeField]
    private int _flowEndY;
    [SerializeField]
    private DirectionState _flowEndDirection;
    [SerializeField]
    private float _flowEndVal;

    [SerializeField]
    private float maxTimeBeforeFlowStarts = 10; // in seconds
    private float timeBeforeFlowStarts;

    public Text flowTimer;
    public Text outputText;

    [SerializeField]
    private List<GridPlacement> _specialTile;
    private List<Flask> _flasks;

    [SerializeField]
    [Tooltip("It will create a perfect sqaure grid based on the number. Ex. 8 will create 8x8 grid.")]
    [Range(2,8)]
    private byte _gridSize = 2;

    private GameObject[,] _grid;

	void Start () {
        outputText.text = _flowEndVal.ToString();
        _flasks = new List<Flask>();
		if(_tile == null)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Missing tile game object.",gameObject.name));
            return;
        }

        if (_startPosition == null)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Missing start position game object.", gameObject.name));
            return;
        }

        CreateInteractableTile();
        timeBeforeFlowStarts = maxTimeBeforeFlowStarts;
        Invoke("StartFlow", maxTimeBeforeFlowStarts);
	}

    private void Update()
    {
        if (timeBeforeFlowStarts >= 0)
        {
            timeBeforeFlowStarts -= Time.deltaTime;
            flowTimer.text = Mathf.Round(timeBeforeFlowStarts).ToString();
        }
    }

    private void Flow(int x, int y, DirectionState dir, int val)
    {
        Debug.Log("Flow starting...");
        GameObject startObj = _grid[x, y];
        Tube tube = startObj.GetComponent<Tube>();
        // If no tube found in the start spot, game over
        if (tube == null)
        {
            GameOver();
            return;
        }
        // If the tube in the start spot is facing the wrong way, game over
        TubeSideData[] sides = tube.GetSides();
        bool inputCorrect = false;
        for (int i = 0; i < sides.Length; i++)
        {
            if (sides[i].Direction == dir && (sides[i].State == InputOutputState.Input || sides[i].State == InputOutputState.Both))
            {
                inputCorrect = true;
            }
        }
        if (!inputCorrect)
        {
            GameOver();
            return;
        }

        tube.FlowStart(dir, val);

    }

    private void StartFlow()
    {
        Debug.Log("Flow starting...");
        Flow(_flowStartX, _flowStartY, _flowStartDirection, 0);
        foreach (Flask flask in _flasks) {
            Flow(flask.nextXCord, flask.nextYCord, flask.flowTo, flask.value);
        }
    }

    public bool CheckWinState(int xCord, int yCord, DirectionState dir, float val)
    {
        if (xCord == _flowEndX && yCord == _flowEndY && dir == _flowEndDirection && val == _flowEndVal)
        {
            return true;
        }
        return false;
    }

    public float TileSize() { return _tile[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
	
    private void CreateInteractableTile()
    {
        _grid = new GameObject[_gridSize, _gridSize];

        foreach (GridPlacement placement in _specialTile)
        {
            GameObject newObj = PlaceTile(placement.xCoordinate, placement.yCoordinate, placement.spriteID);
            Flask flask = newObj.GetComponent<Flask>();
            if (flask != null)
            {
                flask.xCord = placement.xCoordinate;
                flask.yCord = placement.yCoordinate;
                _flasks.Add(newObj.GetComponent<Flask>());
            }
        }

        for (int y = 0; y < _gridSize; y++)
        {
            for(int x = 0; x < _gridSize; x++)
            {
                if(_grid[x,y] == null)
                    PlaceTile(x, y, 0);
            }
        }
    }

    public void SetTile(int x, int y, GameObject go_in) {
        _grid[x,y] = go_in;
    }

    public GameObject PlaceTile(int x, int y, int tileID)
    {
        if(x < 0 || x > _gridSize)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: X coordinate is out of bound.",gameObject.name));
            return null;
        }

        if (y < 0 || y > _gridSize)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Y coordinate is out of bound.",gameObject.name));
            return null;
        }

        if (tileID < 0 || tileID >_tile.Length)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: tile id is out of bound.",gameObject.name));
            return null;
        }

        GameObject newTile = Instantiate(_tile[tileID]);
        newTile.transform.parent = _startPosition.transform;
        newTile.transform.position = new Vector3( _startPosition.transform.position.x + (TileSize() * x), _startPosition.transform.position.y - (TileSize() * y));
        newTile.name = string.Format("Tile {0}x{1}", x, y);
        Tube newTube = newTile.GetComponent<Tube>();
        if (newTube != null) {
            newTube.SetManager(this);
            newTube.SetCoordinates((byte)x,(byte)y);
        }
        _grid[x,y] = newTile;
        return newTile;
    }

    public GameObject[,] GetGrid()
    {
        return _grid;
    }

    public void GameOver()
    {
        Debug.Log("Lose");
        SceneManager.LoadScene("Level_1");
    }

    public void GameWin()
    {
        Debug.Log("Win");
    }
}
