using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : AbstractTube {

    public enum Direction { N, E, S, W };

    public Direction direction = Direction.N;
    
    public Direction[] inputs;
    public Direction output;

    bool placed = false; // Tube starts out attached to the mouse cursor from dragging on the template.
	
	// Update is called once per frame
	void Update () {
        // Tube attached to cursor before being placed
		if (!placed)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            
            if (!placed)
            {
                // Place the tube
                placed = true;
                SnapToClosestTile();
            } else
            {
                // Rotate the tube
                if (MouseOnMe())
                {
                    RotateCW();
                }
            }
            
        }

        // Delete the tube
        if (Input.GetMouseButtonUp(1) && placed)
        {
            if (MouseOnMe())
            {
                Destroy(gameObject);
            }
        }
	}

    void SnapToClosestTile()
    {
        //TODO
    }

    // Rotate sprite and directions 90 degrees clockwise
    void RotateCW()
    {
        transform.Rotate(Vector3.forward * 90);
        direction = RotateDirectionCW(direction);
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = RotateDirectionCW(inputs[i]);
        }
        output = RotateDirectionCW(output);
    }

    // Rotate direction 90 degrees clockwise
    Direction RotateDirectionCW(Direction direction)
    {
        switch (direction)
        {
            case Direction.N:
                return Direction.E;
            case Direction.E:
                return Direction.S;
            case Direction.S:
                return Direction.W;
            case Direction.W:
                return Direction.N;
            default:
                Debug.Log("Tube direction is missing");
                return Direction.N;
        }
    }
}
