/*****************************************
 * This file is part of Impulse Framework.

    Impulse Framework is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    Impulse Framework is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Impulse Framework.  If not, see <http://www.gnu.org/licenses/>.
*****************************************/

using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns objects randomly from a list.
/// The spawning bounds are the bounds of the Box Collider.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class ObjectSpawner : MonoBehaviour
{
    // Objects to spawn.
    [SerializeField]
    GameObject[] ObjectSpawnPrefabs;

    // Number of objects to spawn.
    [SerializeField]
    [Range(1, 100)]
    int NumberToSpawn = 10;

    // The minimum distance between each spawned object.
    [SerializeField]
    [Range(0, 10000)]
    float minDistanceBetweenSpawnedObjects = 10.0f;

    // The object that culling distance will be compared to. This object must have a Collider AND a Rigidbody.
    // Ex: If you want objects to be hidden until they are within a certain range of the player, set the player object here.
    [SerializeField]
    Collider CullingTarget;

    // The distance from the culling target in which further objects should be hidden.
    [SerializeField]
    [Range(10, 10000)]
    float hideSpawnedObjectDistance = 50.0f;

    // The currently spawned game objects. For internal tracking.
    protected GameObject[] CurrentSpawnedObjects;

    // Our Box Collider.
    BoxCollider Bounds;

    /// <summary>
    /// Spawns random objects within the box collider bounds.
    /// </summary>
    public void SpawnObjects()
    {
        CurrentSpawnedObjects = new GameObject[NumberToSpawn];
        Bounds = GetComponent<BoxCollider>();

        for (int i = 0; i < NumberToSpawn; i++)
        {
            // Get random position within this transform.
            Vector3 rndPosWithin = new Vector3(Random.Range(-1f, 1f) * Bounds.size.x / 2,
                                               Random.Range(-1f, 1f) * Bounds.size.x / 2,
                                               Random.Range(-1f, 1f) * Bounds.size.z / 2);
            rndPosWithin += transform.position;

            if (!isObjectTooClose(rndPosWithin))
            {
                CurrentSpawnedObjects[i] = (GameObject)Instantiate(ObjectSpawnPrefabs[Random.Range(0, ObjectSpawnPrefabs.Length)], rndPosWithin, Quaternion.identity);
                CurrentSpawnedObjects[i].transform.parent = transform;

                // We use a sphere collider to determine whether the object should be rendered.
                SphereCollider spawnCollider = CurrentSpawnedObjects[i].AddComponent<SphereCollider>();
                spawnCollider.radius = hideSpawnedObjectDistance;

                // The CullObject script determines whether to show or hide the object.
                CullObject spawnCuller = CurrentSpawnedObjects[i].AddComponent<CullObject>();
                spawnCuller.CullingTarget = CullingTarget;

            }
        }
    }

    /// <summary>
    /// Check if we have an object too close to another object.
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    bool isObjectTooClose(Vector3 targetPosition)
    {
        for (int i = 0; i < CurrentSpawnedObjects.Length; i++)
        {
            if (CurrentSpawnedObjects[i] != null)
            {
                if (Vector3.Distance(targetPosition, CurrentSpawnedObjects[i].transform.position) <= minDistanceBetweenSpawnedObjects)
                {
                    return true;
                }
            }
        }
        return false;
    }

}
