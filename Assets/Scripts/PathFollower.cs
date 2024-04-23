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
    [SerializeField] private float speed;         // movement speed
    
    [SerializeField] private AudioClip attackSound;
    private bool paused;

    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        paused = false;

        TargetNextWaypoint();
    }

    // wait when they are at the point
    private IEnumerator WaitForASec()
    {
        paused = true;
        yield return new WaitForSeconds(0.6f);
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
        transform.position = Vector2.Lerp(sourceWP.position, targetWP.position, elapsedTimePercentage);

        // rotate
        
        //transform.rotation = Quaternion.Lerp(sourceWP.rotation, targetWP.rotation, elapsedTimePercentage);

        // check if we've reached our waypoint (based on time). If so, target the next waypoint
        if(elapsedTimePercentage >= 1)    
        {
            if (this.tag != "Vulture")     // if the gameobject is not "Vulture", they flip
            {
                transform.Rotate(Vector2.up, 180);
            }
            StartCoroutine(WaitForASec());
            TargetNextWaypoint();
        }

    }

    // when enemy collides with player, they make their sound if the sound effect is on
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isSoundEffectActive())
            {
                SoundManager.Instance.PlaySfx(attackSound);
            }
        }
    }

    // check if the sound effect is on
    bool isSoundEffectActive()
    {
        return PlayerPrefs.GetInt(PlayerPrefConstants.EFFECT_SOUND) != 0;
    }

    private void FixedUpdate()
    {
        if (!paused) {
            MoveTowardsWaypoint();
        }        
    }
}
