using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeTemplate : TubeData {

    bool copyCreated = false;

    public GameObject tube;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && !copyCreated && MouseOnMe())
        {
            Debug.Log("here?");
            Instantiate(tube, gameObject.transform.position, gameObject.transform.rotation);
            copyCreated = true;
        }
        if (Input.GetMouseButtonUp(0) && copyCreated)
        {
            copyCreated = false;
        }
	}
}
