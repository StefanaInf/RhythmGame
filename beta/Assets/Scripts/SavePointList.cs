using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavePointList
{
    public List<PointData> points;

    public SavePointList() 
    { 
        points = new List<PointData>();
    }

    public void AddPoint(PointData point)
    {
        points.Add(point);
    }
}
