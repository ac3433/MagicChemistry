using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : TubeData {

    protected bool _placed = false; // Tube starts out attached to the mouse cursor from dragging on the template.

    new void Start() {
        base.Start();
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }


	// Update is called once per frame
	void Update () {
        // Tube attached to cursor before being placed
		if (!_placed)
        {
            Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            
            if (!_placed)
            {
                // Place the tube
                _placed = true;
                SnapToClosestTile();
            } else
            {
                // Rotate the tube
                if (MouseOnMe())
                {
                    RotateClockwise();
                }
            }
            
        }

        // Delete the tube
        if (Input.GetMouseButtonUp(1) && _placed)
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
}
