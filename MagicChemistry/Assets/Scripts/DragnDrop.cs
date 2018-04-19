using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragnDrop : MonoBehaviour {

    private GameObject track;
    Vector3 mousePos;
    BoxCollider2D box;

    private void Start() {
        mousePos = Vector3.zero;
        box = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDrag() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        box.enabled = false;
    }

    private void OnMouseUp() {
        box.enabled = true;
    }

    private void Update() {
        if (!mousePos.Equals(Vector3.zero)) {
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        } else {
            mousePos = Vector3.zero;
        }
    }

}
