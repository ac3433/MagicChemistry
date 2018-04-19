using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DnDTarget : MonoBehaviour {

    SpriteRenderer sr;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("AAA.");
        if (other.gameObject.CompareTag("Pipe")) {
            Debug.Log("Here.");
            sr.sprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;
            sr.color = Color.magenta;
            transform.localScale = new Vector3(1,1,1);
            transform.localRotation = other.gameObject.transform.localRotation;
            Destroy(other.gameObject);
        }
    }
}
