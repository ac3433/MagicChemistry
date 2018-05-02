using UnityEngine;
using System.Collections;

public abstract class AbstractTube : AbstractTile
{
    public abstract void RotateClockwise();

    public abstract bool Validation();

    public abstract void CheckSurrounding();
}
