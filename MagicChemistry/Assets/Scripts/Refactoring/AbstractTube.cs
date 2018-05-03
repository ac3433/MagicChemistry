using UnityEngine;
using System.Collections;

public abstract class AbstractTube : AbstractTile
{
    public int Value { get; protected set; }

    public bool Replacable { get; protected set; }

    public abstract void RotateClockwise();

    public abstract bool Valid();

    public abstract Direction Flow(Direction comingFrom, int value);

    public abstract bool Incoming(Direction dir);

}
