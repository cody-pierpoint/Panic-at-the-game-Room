using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform path;
    [SerializeField] private float speed = 5f;
    [SerializeField] [Tooltip("angle degrees per second")] private float turnSpeed = 90;
    [SerializeField] [Tooltip("1 is forward, -1 is backward")] private int moveDirection = 1;
    [SerializeField] private float waitTime = 1f;


    /// <summary>
    /// draw gizmos on the editor 
    /// </summary>
    private void OnDrawGizmos()
    {

        if (null == path)
        {
            Debug.LogError("need path, pls drag path to enemy script field");
            return;
        }
        
        Vector3 startpos = path.GetChild(0).position;
        Vector3 prevpos = startpos;

        foreach (Transform waypoint in path)
        {
            Gizmos.DrawLine(prevpos, waypoint.position);
            prevpos = waypoint.position;
        }
        //connect last and first to loop the path
        //end of foreach above already gives path.GetChild(path.childCount-1).position
        Gizmos.DrawLine(prevpos, startpos);
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
                //yield return StartCoroutine(TurnToFace(next));
                transform.LookAt(next);
            }
            yield return null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
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
        
    }
}
