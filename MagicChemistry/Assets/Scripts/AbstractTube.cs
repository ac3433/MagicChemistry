using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTube : MonoBehaviour {

    protected Camera cam;
    public float tileSize = 1;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
    }

    // Check if mouse is hovering over the tube
    protected bool MouseOnMe()
    {
        float leftBound = transform.position.x - tileSize / 2f;
        float rightBound = transform.position.x + tileSize / 2f;
        float topBound = transform.position.y + tileSize / 2f;
        float botBound = transform.position.y - tileSize / 2f;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x >= leftBound && mousePos.x <= rightBound
            && mousePos.y >= botBound && mousePos.y <= topBound)
        {
            return true;
        }
        return false;
    }
}
