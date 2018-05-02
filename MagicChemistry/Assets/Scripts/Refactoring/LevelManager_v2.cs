using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TubeFlowPlacement
{
    public GameObject TubePrefab;
    public Point Point;
    public int Number;
}

public class LevelManager_v2 : MonoBehaviour {

    #region Singleton
    private static LevelManager_v2 _instance;
    //Used only once to ensure when one thread have access to create the instance
    private static readonly object _Lock = new object();

    public static LevelManager_v2 Instance
    {
        get
        {
            //thread safe!
            lock (_Lock)
            {
                if (_instance != null)
                    return _instance;
                LevelManager_v2[] instances = FindObjectsOfType<LevelManager_v2>();
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

                GameObject manage = new GameObject("LevelManager_v2");
                manage.AddComponent<GameController>();

                return _instance = manage.GetComponent<LevelManager_v2>();
            }
        }
    }
    #endregion

    #region Basic Grid Information Variables

    [SerializeField]
    [Range(4,8)]
    private byte _gridSize = 4;

    [SerializeField]
    private GameObject _startGridPosition;

    [SerializeField]
    private GameObject _droppableTilePrefab;

    private Point _start;
    private Point _end;

    //contain only the tiles you can drop
    //used for to get the points
    private GameObject[,] _gridDroppableTile;

    //contain only locations for the tubes on the grid
    //mirror grid
    private GameObject[,] _gridTube;

    #endregion

    #region Flow Tube 
    [SerializeField]
    private GameObject _generalTubePosition;
    [SerializeField]
    private GameObject[] _tubeFlowPrefabs;

    [SerializeField]
    private TubeFlowPlacement[] _tubeFlowPlacement;
    #endregion

    #region Operation Tube
    [SerializeField]
    private GameObject _operatorTubePosition;
    [SerializeField]
    private GameObject[] _operationPrefabs;
    #endregion

    void Start () {
        //top left of the grid
        _start = new Point() { X = 0, Y = 0 };
        //bottom right of the grid
        _end = new Point() { X = _gridSize - 1, Y = _gridSize - 1 };

        InitalizeGridTile();
        InitalizeGeneralTube();
        InitializeOperatorTube();
    }

    public float TileSize() { return _droppableTilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x; }

    private void InitalizeGridTile()
    {
        _gridDroppableTile = new GameObject[_gridSize, _gridSize];

        GenerateDroppableTile();
    }

    private void GenerateDroppableTile()
    {
        for(int y = 0; y < _gridSize; y++)
        {
            for(int x = 0; x < _gridSize; x++)
            {
                if(_gridDroppableTile[y,x] == null)
                {
                    GameObject tile = Instantiate(_droppableTilePrefab);
                    tile.transform.parent = _startGridPosition.transform;
                    tile.transform.position = new Vector3(_startGridPosition.transform.position.x + TileSize() * x, _startGridPosition.transform.position.y - (TileSize() * y), 0f);
                    tile.name = string.Format("Tile {0}x{1}", x, y);
                    DroppableTileData tileData = tile.GetComponent<DroppableTileData>();
                    if (tileData != null)
                    {
                        tileData.SetPoint(x, y);
                    }
                    else
                        Debug.LogWarning(string.Format("GameObject: {0}\nScript: LevelManager\nError: Generated tile, but cannot get DroppableTileData in gameobject '{1}'", gameObject.name, tile.name));

                    PlaceGameobjectOnGrid(x, y, _gridDroppableTile, tile);
                }
            }
        }
    }

    private void InitalizeGeneralTube()
    {
        _gridTube = new GameObject[_gridSize, _gridSize];

        GenerateGeneralTube();
    }

    private void GenerateGeneralTube()
    {
        for(int y = 0; y < _tubeFlowPrefabs.Length; y++)
        {
            GameObject tube = Instantiate(_tubeFlowPrefabs[y]);
            tube.transform.parent = _generalTubePosition.transform;
            tube.transform.position = new Vector3(_generalTubePosition.transform.position.x, _generalTubePosition.transform.position.y - (TileSize() * y), 0f);
            tube.name = _tubeFlowPrefabs[y].name;
        }
    }

    private void InitializeOperatorTube()
    {
        GenerateOperatorTube();
    }

    private void GenerateOperatorTube()
    {
        float xl = 0;
        float yl = 0;
        bool stop = true;
        for (int y = 0; y < _operationPrefabs.Length; y++)
        {
            GameObject tube = Instantiate(_operationPrefabs[y]);
            tube.transform.parent = _operatorTubePosition.transform;
            tube.transform.position = new Vector3(_operatorTubePosition.transform.position.x + (TileSize() * xl), _operatorTubePosition.transform.position.y - (TileSize() * yl), 0f);
            tube.name = _operationPrefabs[y].name;
            yl++;
            if(y > 2 && stop)
            {
                xl = 1;
                yl = 0;
                stop = false;
            }
        }

    }

    private void PlaceGameobjectOnGrid(int x, int y, GameObject[,] grid, GameObject generated)
    {
        if(x < 0 || x >= _gridSize)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: X coordinate is out of bound.", gameObject.name));
            return;
        }

        if(y < 0 || y >= _gridSize)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Y coordinate is out of bound.", gameObject.name));
            return;
        }

        grid[y, x] = generated;

    }

    public GameObject GetTubePositioning(Point p)
    {
        if(p.ValidateCoordinate(_gridSize, _gridSize))
        {
            return _gridTube[p.Y, p.X];
        }

        return null;
    }

    public void PlaceTubeOnGrid(GameObject tube, Point p)
    {
        if(p.ValidateCoordinate(_gridSize, _gridSize))
        {
            _gridTube[p.Y, p.X] = tube;
        }
        else
        {
            Debug.LogWarning(string.Format("GameObject: {0}\nScript: LevelManager\nError: Cannot place gameobject '{1}' into position X={2}, Y={3}", gameObject.name, tube.name, p.X, p.Y));
        }
    }

}
