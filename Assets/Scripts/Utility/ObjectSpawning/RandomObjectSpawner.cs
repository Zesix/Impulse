using UnityEngine;

/// <summary>
/// Spawns objects randomly from a list.
/// The spawning bounds are the bounds of the Box Collider.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class RandomObjectSpawner : MonoBehaviour
{
    // Objects to spawn.
    [SerializeField] private GameObject[] _objectSpawnPrefabs;

    // Number of objects to spawn.
    [SerializeField]
    [Range(1, 100)] private int _numberToSpawn = 10;

    // The minimum distance between each spawned object.
    [SerializeField]
    [Range(0, 10000)] private float _minDistanceBetweenSpawnedObjects = 10.0f;

    // The object that culling distance will be compared to. This object must have a Collider AND a Rigidbody.
    // Ex: If you want objects to be hidden until they are within a certain range of the player, set the player object here.
    [SerializeField] private Collider _cullingTarget;

    // The distance from the culling target in which further objects should be hidden.
    [SerializeField]
    [Range(10, 10000)] private float _hideSpawnedObjectDistance = 50.0f;

    // The currently spawned game objects. For internal tracking.
    protected GameObject[] CurrentSpawnedObjects;

    // Our Box Collider.
    private BoxCollider _bounds;

    /// <summary>
    /// Spawns random objects within the box collider bounds.
    /// </summary>
    public void SpawnObjects()
    {
        CurrentSpawnedObjects = new GameObject[_numberToSpawn];
        _bounds = GetComponent<BoxCollider>();

        for (var i = 0; i < _numberToSpawn; i++)
        {
            // Get random position within this transform.
            var rndPosWithin = new Vector3(Random.Range(-1f, 1f) * _bounds.size.x / 2,
                                               Random.Range(-1f, 1f) * _bounds.size.x / 2,
                                               Random.Range(-1f, 1f) * _bounds.size.z / 2);
            rndPosWithin += transform.position;

            if (!IsObjectTooClose(rndPosWithin))
            {
                var spawnedObject = Instantiate(_objectSpawnPrefabs[Random.Range(0, _objectSpawnPrefabs.Length)], rndPosWithin, Quaternion.identity);
                CurrentSpawnedObjects[i] = spawnedObject;
                CurrentSpawnedObjects[i].transform.parent = transform;

                // Create a child game object, which we will attach the culling sphere to.
                var cullingSphere = new GameObject("Culling Sphere");
                cullingSphere.transform.position = rndPosWithin;
                cullingSphere.transform.parent = spawnedObject.transform;

                // We use a sphere collider to determine whether the object should be rendered.
                var spawnCollider = cullingSphere.AddComponent<SphereCollider>();
                spawnCollider.radius = _hideSpawnedObjectDistance;

                // The CullObject script determines whether to show or hide the object.
                var spawnCuller = cullingSphere.AddComponent<CullObject>();
                spawnCuller.CullingTarget = _cullingTarget;

            }
        }
    }

    /// <summary>
    /// Check if we have an object too close to another object.
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private bool IsObjectTooClose(Vector3 targetPosition)
    {
        foreach (var t in CurrentSpawnedObjects)
        {
            if (t != null)
            {
                if (Vector3.Distance(targetPosition, t.transform.position) <= _minDistanceBetweenSpawnedObjects)
                {
                    return true;
                }
            }
        }
        return false;
    }

}
