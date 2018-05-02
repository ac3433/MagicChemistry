using UnityEngine;
using System.Collections;

public abstract class AbstractTile : MonoBehaviour
{
    private Point _Point;

    private void Awake()
    {
        _Point = new Point();
    }

    public void SetPoint(int x, int y)
    {
        _Point.X = x;
        _Point.Y = y;
    }

    public void SetPoint(Point point)
    {
        _Point = point;
    }

    public Point GetPoint()
    {
        return _Point;
    }
}
