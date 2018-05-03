using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct GridPlacement
{
    public int xCoordinate;
    public int yCoordinate;
    public int value;
    [Tooltip("Select the sprite from the tile list")]
    public int spriteID;
}

public class LevelManager : MonoBehaviour {


    #region Singleton
    private static LevelManager _instance;
    //Used only once to ensure when one thread have access to create the instance
    private static readonly object _Lock = new object();

    public static LevelManager Instance
    {
        get
        {
            //thread safe!
            lock (_Lock)
            {
                if (_instance != null)
                    return _instance;
                LevelManager[] instances = FindObjectsOfType<LevelManager>();
                //see if there are any already more instance of this
                if (instances.Length > 0)
                {
                    //yay only 1 instance so give it back
                    if (instances.Length == 1)
                        return _instance = instances[0];

                    //remove all other instance of it other than the 1st one
                    for (int i = 1; i < instances.Length; i++)
                        Destroy(instances[i]);
                    return _instance = instances[0];
                }

                GameObject manage = new GameObject("LevelManager");
                manage.AddComponent<GameController>();

                return _instance = manage.GetComponent<LevelManager>();
            }
        }
    }
    #endregion
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
    private int _startValue;
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
    public Text inputText;
    public Text currentOutText;
    public int currentOut;

    public AudioSource mainMusic;

    [SerializeField]
    private List<GridPlacement> _specialTile;


    [SerializeField]
    [Tooltip("It will create a perfect sqaure grid based on the number. Ex. 8 will create 8x8 grid.")]
    [Range(2,8)]
    private byte _gridSize = 2;

    private GameObject[,] _grid;

	void Start () {
        currentOut = _startValue;
        outputText.text = _flowEndVal.ToString();
        inputText.text = _startValue.ToString();
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
            flowTimer.text = "Start in: " + Mathf.Round(timeBeforeFlowStarts).ToString();
        }
        currentOutText.text = "Current value: " + currentOut;
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

        tube.FlowStart(dir, val, OperationState.None);

    }

    private void StartFlow()
    {
        Debug.Log("Flow starting...");
        Flow(_flowStartX, _flowStartY, _flowStartDirection, _startValue);
        mainMusic.Play();
    }

    public bool CheckWinState(int xCord, int yCord, DirectionState dir, float val)
    {
        if (xCord == _flowEndX && yCord == _flowEndY && dir == _flowEndDirection && val == _flowEndVal)
        {
            return true;
        }
        return false;
    }

    public float TileSize() { return _tile[0].GetComponentInChildren<SpriteRenderer>().sprite.bounds.size.x; }
	
    private void CreateInteractableTile()
    {
        _grid = new GameObject[_gridSize, _gridSize];

        foreach (GridPlacement placement in _specialTile)
        {
            GameObject newObj = PlaceTile(placement.xCoordinate, placement.yCoordinate, placement.spriteID);
            TubeData tube = newObj.GetComponent<TubeData>();
            if (tube != null)
            {
                tube.SetValue(placement.value);
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
        SceneManager.LoadScene("GameOver");
    }

    public void GameWin()
    {
        Debug.Log("Win");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
