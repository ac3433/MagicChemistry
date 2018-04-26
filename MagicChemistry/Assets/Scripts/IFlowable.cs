using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlowable {

    void FlowStart(DirectionState inFlowSide, int val);
    void FlowTick();
    void FlowToNext();
}
