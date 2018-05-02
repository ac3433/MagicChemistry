public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public bool ValidateCoordinate(int xSize, int ySize)
    {
        if (X < 0 || X > xSize)
        {
            return false;
        }

        if (Y < 0 || Y > ySize)
        {
            return false;
        }

        return true;
    }

    public Point AddPoint(Point p2)
    {
        return new Point() { X = X + p2.X, Y = Y + p2.Y };
    }
}

