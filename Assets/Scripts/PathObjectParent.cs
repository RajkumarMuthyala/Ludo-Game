using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObjectParent : MonoBehaviour
{
    public PathPoint[] commonPathPoint;
    public PathPoint[] redPathPoint;
    public PathPoint[] bluePathPoint;
    public PathPoint[] greenPathPoint;
    public PathPoint[] yellowPathPoint;
    public PathPoint[] basePoint;

    [Header("Scale and Position")]
    public float[] scale;
    public float[] positionDifference;
    
}
