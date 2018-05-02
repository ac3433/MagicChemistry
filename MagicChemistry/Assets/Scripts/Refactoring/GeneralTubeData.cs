using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralTubeData : AbstractTube
{
    [SerializeField]
    private Direction[] _holeLocations = new Direction[2];

    private AbstractTile _prevHole;
    private AbstractTile _nextHole;

    [SerializeField]
    private GameObject _hoverSprite;

    private Dictionary<Direction, AbstractTile> _holeDict;

    private int value;

    private void Start()
    {
        _holeDict = new Dictionary<Direction, AbstractTile>();
        //will be reassign upon connecting
        _holeDict.Add(_holeLocations[0], _prevHole);
        _holeDict.Add(_holeLocations[1], _nextHole);

        value = 0;

    }

    private void OnMouseOver()
    {
        if(_hoverSprite != null)
        {
            _hoverSprite.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (_hoverSprite != null)
        {
            _hoverSprite.SetActive(false);
        }
    }

    public override bool Validation()
    {
        throw new System.NotImplementedException();
    }

    public override void RotateClockwise()
    {
        transform.Rotate(Vector3.forward, -90);
        for (int i = 0; i < _holeLocations.Length; i++)
        {
            _holeLocations[i] = DirectionExtensions.RotateClockwise(_holeLocations[i]);
        }

        Dictionary<Direction, AbstractTile> temp = new Dictionary<Direction, AbstractTile>();

        foreach (Direction dir in _holeDict.Keys)
        {
            temp.Add(DirectionExtensions.RotateClockwise(dir), _holeDict[dir]);
        }

        _holeDict = temp;
    }

    public override void CheckSurrounding()
    {
        throw new System.NotImplementedException();
    }
}
