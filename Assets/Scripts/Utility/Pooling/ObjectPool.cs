using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{

    PooledObject prefab;

    List<PooledObject> availableObjects = new List<PooledObject>();

    /// <summary>
    /// Gets the last pooled object available in the list. If no object is available, creates one.
    /// </summary>
    /// <returns></returns>
    public PooledObject GetObject()
    {
        PooledObject obj;
        var lastAvailableIndex = availableObjects.Count - 1;
        if (lastAvailableIndex >= 0)
        {
            obj = availableObjects[lastAvailableIndex];
            availableObjects.RemoveAt(lastAvailableIndex);
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = Instantiate<PooledObject>(prefab);
            obj.transform.SetParent(transform, false);
            obj.Pool = this;
        }
        return obj;
    }

    /// <summary>
    /// Gets an instance of the object pool for a specific prefab, or creates one if it doesn't exist.
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static ObjectPool GetPool (PooledObject prefab)
    {
        GameObject obj;
        ObjectPool pool;

        if (Application.isEditor)
        {
            obj = GameObject.Find(prefab.name + " Pool");
            if (obj)
            {
                pool = obj.GetComponent<ObjectPool>();
                if (pool)
                {
                    return pool;
                }
            }
        }

        obj = new GameObject(prefab.name + " Pool");
        DontDestroyOnLoad(obj);
        pool = obj.AddComponent<ObjectPool>();
        pool.prefab = prefab;
        return pool;
    }

    /// <summary>
    /// Adds an object to the pool.
    /// </summary>
    /// <param name="obj"></param>
    public void AddObject(PooledObject obj)
    {
        obj.gameObject.SetActive(false);
        availableObjects.Add(obj);
    }
}
