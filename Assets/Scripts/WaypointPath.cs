using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    // get the trans form of a specific waypoint(pos, rot, scale)
    public Transform GetWaypoint(int index)
    {
        return transform.GetChild(index).transform;
    }

    // return the number of waypoints in the path
    public int GetWaypointCount()
    {
        return transform.childCount;
    }
}
