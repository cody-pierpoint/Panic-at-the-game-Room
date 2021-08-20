using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private Transform path;
    [SerializeField] private Color colour = Color.white;
    
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

        //set the colour of path to draw
        Gizmos.color = this.colour;
        foreach (Transform waypoint in path)
        {
            Gizmos.DrawLine(prevpos, waypoint.position);
            prevpos = waypoint.position;
        }
        //connect last and first to loop the path
        //end of foreach above already gives path.GetChild(path.childCount-1).position
        Gizmos.DrawLine(prevpos, startpos);
    }
}
