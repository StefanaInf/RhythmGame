using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointData 
{
    public float bandValue;
    public int bandIndex;
    public float timestamp;

    public PointData(float bandValue, int bandIndex, float timestamp)
    {
        this.bandValue = bandValue;
        this.bandIndex = bandIndex;
        this.timestamp = timestamp;
    }
}
