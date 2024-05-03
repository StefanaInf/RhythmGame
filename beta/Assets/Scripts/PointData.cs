using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PointData 
{
    public float[] bandValues;
    public float amplitude;
    public float timestamp;
     
    public PointData(float[] bandValues, float amplitude, float timestamp)
    {
        this.bandValues = bandValues;
        this.amplitude = amplitude;
        this.timestamp = timestamp;
    }

    public bool Equals(PointData other)
    {
        if(!other.bandValues.SequenceEqual(bandValues)) return false;
        if(other.amplitude!= amplitude) return false;
        return true;
    }
}
