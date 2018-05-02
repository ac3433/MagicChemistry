using UnityEngine;
using System.Collections;

public enum Operation
{
    None, Multiply, Division, Add, Subtraction
}

public static class OperationExtension
{
    public static int CalculateValue(Operation op, int v1, int v2)
    {
            switch (op)
            {
                case Operation.Add:
                    return v1 + v2;

                case Operation.Subtraction:
                    return v1 - v2;

                case Operation.Multiply:
                    return v1 * v2;

                case Operation.Division:
                    return v1 / v2;

                default:
                    return 0;
            }
    }
}
