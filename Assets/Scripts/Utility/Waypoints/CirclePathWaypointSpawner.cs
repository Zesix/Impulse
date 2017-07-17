using UnityEngine;

/// <summary>
/// Used to spawn empty gameObjects in a circular path around a center position.
/// </summary>
public class CirclePathWaypointSpawner : MonoBehaviour
{
    [SerializeField]
    // The parent gameObject these waypoints will be placed.
    private GameObject _parentTransform;

    [SerializeField]
    // Should we spawn the path on start?
    private bool _spawnOnStart;

    [SerializeField]
    // Number of waypoints.
    private int _numPoints = 20;

    [SerializeField]
    // The radius of the circle upon which the waypoints would be generated.
    private float _radius = 10.0f;

    // Waypoints will be spawned around our center position.
    private Vector3 _centerPosition;

    private void Start()
    {
        _centerPosition = transform.position;
        if (_spawnOnStart)
        {
            SpawnPath();
        }
    }

    public void SpawnPath()
    {
        for (var pointNum = 0; pointNum < _numPoints; pointNum++)
        {
            // "i" now represents the progress around the circle from 0-1
            // we multiply by 1.0 to ensure we get a fraction as a result.
            var i = (float)(pointNum * 1.0 / _numPoints);

            // get the angle for this step (in radians, not degrees)
            var angle = i * Mathf.PI * 2;

            // the X and Y position for this angle are calculated using Sin and Cos
            var x = Mathf.Sin(angle) * _radius;
            var y = Mathf.Cos(angle) * _radius;

            var waypointPosition = new Vector3(x, y, 0) + _centerPosition;

            // no need to assign the instance to a variable unless you're using it afterwards:
            var waypoint = new GameObject("Waypoint " + pointNum);
            waypoint.transform.position = waypointPosition;

            if (_parentTransform != null)
            {
                waypoint.transform.parent = _parentTransform.transform;
            }
        }
    }

    /// <summary>
    /// Executed on the draw gizmos event
    /// </summary>
    private void OnDrawGizmos()
    {
        // Shows the radius in Editor.
        if (_radius > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
