using UnityEngine;
using System.Collections;

public class TubeSideData
{
    public DirectionState Direction { set; get; }
    public InputOutputState State { set; get; }
    public TubeData NextTile { set; get; }
}
