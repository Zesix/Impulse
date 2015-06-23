using UnityEngine;
using System.Collections;

/// <summary>
/// Manages waypoints. To create waypoints, attach this to an empty gameobject, then create empty child objects where you want the waypoints to be.
/// </summary>
public class WaypointPathManager : MonoBehaviour
{

    [ExecuteInEditMode]

    // this script simply gives us a visual path to make it easier to edit
    // our waypoints
    private ArrayList transforms; // arraylist for easy access to transforms
    private Vector3 firstPoint; // store our first waypoint so we can loop the path
    private float distance; // used to calculate distance between points
    private Transform TEMPtrans; // a temporary holder for a transform
    private int TEMPindex; // a temporary holder for an index number
    private int totalTransforms;

    private Vector3 diff;
    private float curDistance;
    private Transform closest;

    private Vector3 currentPos;
    private Vector3 lastPos;
    private Transform pointT;

    [SerializeField]
    bool closed = true;
    [SerializeField]
    bool shouldReverse;

    void Start()
    {
        // make sure that when this script starts (on the device) that
        // we have grabbed the transforms for each waypoint
        GetTransforms();
    }

    void OnDrawGizmos()
    {
        // we only want to draw the waypoints when we're editing, not when we are playing the game
        if (Application.isPlaying)
            return;

        GetTransforms();

        // if we didn't populate our list of transforms, do that now
        //if(transforms==null)
        //	GetTransforms();

        // make sure that we have more than one transform in the list, otherwise
        // we can't draw lines between them
        if (totalTransforms < 2)
            return;

        // draw our path
        // first, we grab the position of the very first waypoint
        // so that our line has a start point
        TEMPtrans = (Transform)transforms[0];
        lastPos = TEMPtrans.position;

        // we point each waypoint at the next, so that we can use this rotation
        // data to find out when the player is going the wrong way or to position
        // the player after a reset facing the correct direction. so first we need
        // to hold a reference to the transform we are going to point
        pointT = (Transform)transforms[0];

        // also, as this is the first point we store it to use for closing the path later
        firstPoint = lastPos;

        // now we loop through all of the waypoints drawing lines between them
        for (int i = 1; i < totalTransforms; i++)
        {
            TEMPtrans = (Transform)transforms[i];
            if (TEMPtrans == null)
            {
                GetTransforms();
                return;
            }

            // grab the current waypoint position
            currentPos = TEMPtrans.position;

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(currentPos, 2);

            // draw the line between the last waypoint and this one
            Gizmos.color = Color.red;
            Gizmos.DrawLine(lastPos, currentPos);

            // point our last transform at the latest position
            pointT.LookAt(currentPos);

            // update our 'last' waypoint to become this one as we
            // move on to find the next...
            lastPos = currentPos;

            // update the pointing transform
            pointT = (Transform)transforms[i];
        }

        // close the path
        if (closed)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(currentPos, firstPoint);
        }
    }

    public void GetTransforms()
    {
        // we store all of the waypoints transforms in an ArrayList,
        // which is initialised here (we always need to do this before we can
        // use ArrayLists)
        transforms = new ArrayList();

        // now we go through any transforms 'under' this transform, so all of
        // the child objects that act as our waypoints get put into our arraylist
        foreach (Transform t in transform)
        {
            // add this transform to our arraylist
            transforms.Add(t);
        }

        totalTransforms = (int)transforms.Count;
    }

    // Sets the value of shouldReverse, which determines whether or not the path should be reversed.
    public void SetReverseMode(bool rev)
    {
        shouldReverse = rev;
    }

    /// <summary>
    /// Finds the nearest waypoint to a given position.
    /// </summary>
    /// <param name="fromPos">A position, upon which the closest waypoint will be found.</param>
    /// <param name="maxRange">A distance limiter, in case you only want to grab the closest waypoint if it's within a certain distance.</param>
    /// <returns>The index number of the waypoint as an integer.</returns>
    public int FindNearestWaypoint(Vector3 fromPos, float maxRange)
    {
        // make sure that we have populated the transforms list, if not, populate it
        if (transforms == null)
            GetTransforms();

        // the distance variable is just used to hold the 'current' distance when
        // we are comparing, so that we can find the closest
        distance = Mathf.Infinity;

        // Iterate through them and find the closest one
        for (int i = 0; i < transforms.Count; i++)
        {
            // grab a reference to a transform
            TEMPtrans = (Transform)transforms[i];

            // calculate the distance between the current transform and the passed in transform's position vector
            diff = (TEMPtrans.position - fromPos);
            curDistance = diff.sqrMagnitude;

            // now compare distances - making sure that we are not 
            if (curDistance < distance)
            {
                if (Mathf.Abs(TEMPtrans.position.y - fromPos.y) < maxRange)
                {

                    // set our current 'winner' (closest transform) to the transform we just found
                    closest = TEMPtrans;

                    // store the index of this waypoint
                    TEMPindex = i;

                    // set our 'winning' distance to the distance we just found
                    distance = curDistance;
                }
            }
        }

        // now we make sure that we did actually find something, then return it
        if (closest)
        {
            // return the waypoint we found in this test
            return TEMPindex;
        }
        else
        {
            // no waypoint was found, so return -1 (this should be acccounted for at the other end!)
            return -1;
        }
    }

    // this function has the addition of a check to avoid finding the same transform as one passed in. we use
    // this to make sure that when we are looking for the nearest waypoint we don't find the same one as
    // we just passed

    public int FindNearestWaypoint(Vector3 fromPos, Transform exceptThis, float maxRange)
    {
        // make sure that we have populated the transforms list, if not, populate it
        if (transforms == null)
            GetTransforms();

        // the distance variable is just used to hold the 'current' distance when
        // we are comparing, so that we can find the closest
        distance = Mathf.Infinity;

        // Iterate through them and find the closest one
        for (int i = 0; i < totalTransforms; i++)
        {
            // grab a reference to a transform
            TEMPtrans = (Transform)transforms[i];

            // calculate the distance between the current transform and the passed in transform's position vector
            diff = (TEMPtrans.position - fromPos);
            curDistance = diff.sqrMagnitude;

            // now compare distances - making sure that we are not 
            if (curDistance < distance && TEMPtrans != exceptThis)
            {
                if (Mathf.Abs(TEMPtrans.position.y - fromPos.y) < maxRange)
                {

                    // set our current 'winner' (closest transform) to the transform we just found
                    closest = TEMPtrans;

                    // store the index of this waypoint
                    TEMPindex = i;

                    // set our 'winning' distance to the distance we just found
                    distance = curDistance;
                }
            }
        }

        // now we make sure that we did actually find something, then return it
        if (closest)
        {
            // return the waypoint we found in this test
            return TEMPindex;
        }
        else
        {
            // no waypoint was found, so return -1 (this should be acccounted for at the other end!)
            return -1;
        }
    }

    public Transform GetWaypoint(int index)
    {
        if (shouldReverse)
        {
            // send back the reverse index'd waypoint
            index = (totalTransforms - 1) - index;

            if (index < 0)
                index = 0;
        }

        // make sure that we have populated the transforms list, if not, populate it
        if (transforms == null)
            GetTransforms();

        // first, lets check to see if this index is higher than our waypoint count
        // if so, we return null which needs to be handled on the other side'
        if (index > totalTransforms - 1)
            return null;

        return (Transform)transforms[index];
    }

    public int GetTotal()
    {
        return totalTransforms;
    }

}
