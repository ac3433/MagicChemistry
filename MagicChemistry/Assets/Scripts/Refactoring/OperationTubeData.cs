using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OperationTubeData : GeneralTubeData
{
    [SerializeField]
    private Operation _operationType;

    //rewrite too lazy to check all given path now
    public override bool Valid()
    {
        foreach (Direction hole in _holeDict.Keys)
        {
            Point targetPoint = GetPoint().AddPoint(DirectionExtensions.GetDirectionPoint(hole));

            GameObject obj = LevelManager_v2.Instance.GetTubePositioning(targetPoint);

            if (obj != null)
            {
                if (obj.tag.ToLower().Contains("operator"))
                    return false;
            }
        }

        return true;
    }


    public override Direction Flow(Direction comingFrom, int value)
    {
        Value = value;

        toBeOperatated = _operationType;

        List<Direction> remainingSpot = _holeDict.Keys.ToList();

        remainingSpot.Remove(DirectionExtensions.GetOppositeDirection(comingFrom));

        return remainingSpot[0];

    }
}
