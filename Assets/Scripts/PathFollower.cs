using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PathFollower : MonoBehaviour
{
    [SerializeField]
    private WaypointPath path;          // the path we are following
    private Transform sourceWP;         // the waypoint transform we are travelling from
    private Transform targetWP;         // the waypoint transform we are travelling to
    private int targetWPIndex = 0;      // the waypoint index we are travelling to

    private float totalTimeToWP;        // the total time to get from source WP to targetWP
    private float elapsedTimeToWP = 0;  // the elapsed time (sourceWP to targetWP)
    private float speed = 6.0f;         // movement speed

    private bool paused;

    void Start()
    {
        paused = false;

        TargetNextWaypoint();
    }

    void Update()
    {
        
    }

    private IEnumerator WaitForASec()
    {
        paused = true;
        yield return new WaitForSeconds(1.0f);
        paused = false;
    }

    // Determine what waypoint we are going to next, and set associated variables
    private void TargetNextWaypoint()
    {
        // reset the elapsed time
        elapsedTimeToWP = 0;

        // determine the source waypoint
        sourceWP = path.GetWaypoint(targetWPIndex);

        // determine the target waypoint
        targetWPIndex++;

        // if we exhausted our waypoints, go the to first waypoint
        if(targetWPIndex >= path.GetWaypointCount() ) {
            targetWPIndex = 0;
        }

        targetWP = path.GetWaypoint(targetWPIndex);

        // calculate source to target distance
        float distanceToWP = Vector2.Distance(sourceWP.position, targetWP.position);

        // calculate time to waypoint
        totalTimeToWP = distanceToWP / speed;
    }

    // Travel towards the target waypoint (call this from FixedUpdate())
    private void MoveTowardsWaypoint()
    {
        // calculate the elapsed time spent on the way to this waypoint
        elapsedTimeToWP += Time.deltaTime;

        // calculate percent complete
        float elapsedTimePercentage = elapsedTimeToWP / totalTimeToWP;

        // make spped slower at beginning and end
        elapsedTimePercentage = Mathf.SmoothStep(0, 1, elapsedTimePercentage);

        // move
        transform.position = Vector3.Lerp(sourceWP.position, targetWP.position, elapsedTimePercentage);

        // rotate
        transform.Rotate(Vector3.up, 180);
        //transform.rotation = Quaternion.Lerp(sourceWP.rotation, targetWP.rotation, elapsedTimePercentage);

        // check if we've reached our waypoint (based on time). If so, target the next waypoint
        if(elapsedTimePercentage >= 1)
        {
            StartCoroutine(WaitForASec());
            TargetNextWaypoint();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // other.transform.parent = this.gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // other.transform.parent = null;
        }
    }

    private void FixedUpdate()
    {
        if (!paused) {
            MoveTowardsWaypoint();
        }
        
    }
}
