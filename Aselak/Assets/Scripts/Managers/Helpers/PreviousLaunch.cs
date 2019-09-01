using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousLaunch {
    public Vector3 direction;
    public float burst;

    public PreviousLaunch(Vector3 direction, float burst)
    {
        this.direction = direction;
        this.burst = burst;
    }
}
