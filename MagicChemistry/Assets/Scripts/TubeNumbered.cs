using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeNumbered : Tube {

	// Use this for initialization
	new void Start () {
        base.Start();
        if (_thisVal != 0)
        {
            numberText.text = _thisVal.ToString();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (flowing && !filled)
        {
            if (timeTillFill > 0)
            {
                timeTillFill -= Time.deltaTime;
            }
            else
            {
                CancelInvoke();
                FlowToNext();
                filled = true;
            }
        }
    }
}
