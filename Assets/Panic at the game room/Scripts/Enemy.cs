using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform path;
    [SerializeField] private float speed = 5f;
    [SerializeField] [Tooltip("angle degrees per second")] private float turnSpeed = 180;
    [SerializeField] [Tooltip("1 is forward, -1 is backward")] private int moveDirection = 1;
    [SerializeField] private float waitTime = 1f;

    [SerializeField] private Light spotlight;
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask obstructionLayer;
    private float viewAngle;
    [SerializeField] private Transform player;
    
    private Color defaultSpotlightColour;
    
    /// <summary>
    /// draw gizmos on the editor 
    /// </summary>
    private void OnDrawGizmos()
    {
        //spotlight colour
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }

    /// <summary>
    /// coroutine to move the object along the given waypoints of a path.
    /// </summary>
    /// <param name="waypoints"></param>
    /// <returns></returns>
    IEnumerator MoveAlongPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int nextIndex = 1;
        Vector3 next = waypoints[nextIndex];
        //set the object rotation to look at the next point from the start
        transform.LookAt(next);
        while (true)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, next, Time.deltaTime * speed);
            if (transform.position == next)
            {
                nextIndex = (nextIndex + moveDirection) % waypoints.Length;
                next = waypoints[nextIndex];
                //yield return new WaitForSeconds(waitTime);

                if (waitTime == 0)
                {
                    transform.LookAt(next);
                }
                else
                {
                    yield return StartCoroutine(TurnToFace(next));
                }
            }
            yield return null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //keep original spotlight colour before any detection
        defaultSpotlightColour = spotlight.color;
        viewAngle = spotlight.spotAngle;
        
        //intialise waypoints array and run start the coroutine 
        Vector3[] waypoints = new Vector3[path.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = path.GetChild(i).position;
            //make the waypoints aligned to the enemy object on the Y axis
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        StartCoroutine(MoveAlongPath(waypoints));
    }

    /// <summary>
    /// animate rotation towards the next point
    /// </summary>
    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 lookDirection = (lookTarget - transform.position).normalized;
        //not sure how the math formula works but this is how you find the angle of the next object from current direction  
        float targetAngle = 90 - Mathf.Atan2(lookDirection.z, lookDirection.x) * Mathf.Rad2Deg;
        // Debug.Log($"target angle {targetAngle}");
        
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            // Debug.Log($"delta angle {Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)}");
            float angle = Mathf.MoveTowardsAngle
                (transform.eulerAngles.y, targetAngle, Time.deltaTime * turnSpeed);
        transform.eulerAngles = new Vector3(0, angle,0);
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if player is spotted, the spotlight becomes red, else, normal colour
        spotlight.color = canSeePlayer() ? Color.red : defaultSpotlightColour;
    }

    bool canSeePlayer()
    {
        if (Vector3.Distance(this.transform.position, player.position) < viewDistance)
        {
            Vector3 directionToPlayer = (player.position - this.transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (!(angleBetweenGuardAndPlayer < viewAngle / 2f)) return false;
            if (!Physics.Linecast(transform.position, player.position, obstructionLayer)) return true;
        }
        return false;
    }
}
