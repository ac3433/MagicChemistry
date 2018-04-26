using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTube : Tube {

    [SerializeField] private float[] fractions;

    protected new void FlowTick() {
        float currentFrac = 0;
        for(int i = 0; i < masks.Length; i++) {
            bool done = false;
            currentFrac += fractions[i];
            while (!done) {
                float fracJourney = ((Time.time - flowStartTime) / maxTimeTillFill);
                masks[i].transform.localScale = new Vector3(masks[i].transform.localScale.x,
                                                        Mathf.Lerp(maskScale[i], 0.01f, fracJourney * (1 / currentFrac)),
                                                        masks[i].transform.localScale.z);
                if (fracJourney > 1.0f) {
                    FlowToNext();
                    CancelInvoke("FlowTick");
                } else if (fracJourney > currentFrac) {
                    done = true;
                }
            }
        }
    }

}
