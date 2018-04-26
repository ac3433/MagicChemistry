using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flask : MonoBehaviour {

    public DirectionState outputDir;
    public int value;
    public int xCord;
    public int yCord;

    public int nextXCord;
    public int nextYCord;
    public DirectionState flowTo;

    private void Start()
    {
        nextXCord = xCord;
        nextYCord = yCord;
        switch (outputDir)
        {
            case DirectionState.North:
                flowTo = DirectionState.South;
                nextYCord--;
                break;
            case DirectionState.South:
                flowTo = DirectionState.North;
                nextYCord++;
                break;
            case DirectionState.East:
                flowTo = DirectionState.West;
                nextXCord++;
                break;
            case DirectionState.West:
                flowTo = DirectionState.East;
                nextXCord--;
                break;
            default:
                Debug.Log("It's all on fire...");
                break;

        }
    }
}
