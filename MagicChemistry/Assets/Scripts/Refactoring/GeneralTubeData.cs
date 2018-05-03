using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GeneralTubeData : AbstractTube
{
    [SerializeField]
    private Direction[] _holeLocations = new Direction[2];

    protected AbstractTube _prevHole;
    protected AbstractTube _nextHole;

    protected Dictionary<Direction, AbstractTube> _holeDict;

    protected Operation toBeOperatated { get; set; }

    private void Start()
    {
        _holeDict = new Dictionary<Direction, AbstractTube>();
        //will be reassign upon connecting
        _holeDict.Add(_holeLocations[0], _prevHole);
        _holeDict.Add(_holeLocations[1], _nextHole);

        Value = 0;
        Replacable = true;
    }

    //mainly for operation
    public override bool Valid()
    {
        return true;
    }

    public override void RotateClockwise()
    {
        transform.Rotate(Vector3.forward, -90);
        for (int i = 0; i < _holeLocations.Length; i++)
        {
            _holeLocations[i] = DirectionExtensions.RotateClockwise(_holeLocations[i]);
        }

        Dictionary<Direction, AbstractTube> temp = new Dictionary<Direction, AbstractTube>();

        foreach (Direction dir in _holeDict.Keys)
        {
            temp.Add(DirectionExtensions.RotateClockwise(dir), _holeDict[dir]);
            //Debug.Log(string.Format("Dir: {0} => {1}", dir, DirectionExtensions.RotateClockwise(dir)));
        }

        _holeDict = temp;
    }

    //public override void CheckSurrounding()
    //{
    //    foreach(Direction hole in _holeLocations)
    //    {
    //        Point targetPoint = GetPoint().AddPoint(DirectionExtensions.GetDirectionPoint(hole));

    //        GameObject obj = LevelManager_v2.Instance.GetTubePositioning(targetPoint);

    //        if(obj != null)
    //        {
    //            AbstractTube connectingTube = 
    //        }

    //    }
    //}

    public void AddConnectingTube(AbstractTube obj, Direction dirComingFrom)
    {
        if(_holeDict.ContainsKey(dirComingFrom))
        {
            if(_holeDict[dirComingFrom].Equals(_prevHole))
            {
                _prevHole = obj;
            }
            else if (_holeDict[dirComingFrom].Equals(_nextHole))
            {
                _nextHole = obj;
            }
        }
    }



    public override Direction Flow(Direction comingFrom, int value)
    {
        Value = value;

        List<Direction> remainingSpot = _holeDict.Keys.ToList();

        remainingSpot.Remove(DirectionExtensions.GetOppositeDirection(comingFrom));

        return remainingSpot[0];

    }

    public override bool Incoming(Direction dir)
    {
        List<Direction> hole = _holeDict.Keys.ToList();

        if (hole.Contains(DirectionExtensions.GetOppositeDirection(dir)))
        {
            return true;
        }

        return false;
    }
}
