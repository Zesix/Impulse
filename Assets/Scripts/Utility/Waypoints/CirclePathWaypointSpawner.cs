using UnityEngine;
using System.Collections;

/// <summary>
/// Used to spawn empty gameObjects in a circular path around a center position.
/// </summary>
public class CirclePathWaypointSpawner : MonoBehaviour
{

    [SerializeField]
    // The parent gameObject these waypoints will be placed.
    GameObject parentTransform;

    [SerializeField]
    // Should we spawn the path on start?
    bool spawnOnStart = false;

    [SerializeField]
    // Number of waypoints.
    int numPoints = 20;

    [SerializeField]
    // The radius of the circle upon which the waypoints would be generated.
    float radius = 10.0f;

    // Waypoints will be spawned around our center position.
    Vector3 centerPosition;

    void Start()
    {
        centerPosition = transform.position;
        if (spawnOnStart)
        {
            SpawnPath();
        }
    }

    public void SpawnPath()
    {
        for (int pointNum = 0; pointNum < numPoints; pointNum++)
        {
            // "i" now represents the progress around the circle from 0-1
            // we multiply by 1.0 to ensure we get a fraction as a result.
            float i = (float)((pointNum * 1.0) / numPoints);

            // get the angle for this step (in radians, not degrees)
            float angle = i * Mathf.PI * 2;

            // the X and Y position for this angle are calculated using Sin and Cos
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;

            Vector3 waypointPosition = new Vector3(x, y, 0) + centerPosition;

            // no need to assign the instance to a variable unless you're using it afterwards:
            GameObject waypoint = new GameObject("Waypoint " + pointNum);
            waypoint.transform.position = waypointPosition;

            if (parentTransform != null)
            {
                waypoint.transform.parent = parentTransform.transform;
            }
        }
    }

    #region Debug

    /// <summary>
    /// Executed on the draw gizmos event
    /// </summary>
    void OnDrawGizmos()
    {
        // Shows the radius in Editor.
        if (radius > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    #endregion
}
