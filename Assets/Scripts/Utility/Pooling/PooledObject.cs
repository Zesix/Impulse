using UnityEngine;

public class PooledObject : MonoBehaviour
{
    // This is a reference to a prefab so the object can stay in the pool across multiple scenes.
    [System.NonSerialized]
    private ObjectPool _poolInstanceForPrefab;

    public ObjectPool Pool { get; set; }

    /// <summary>
    /// If the pool exists, returns this object to the pool. Otherwise, destroy it.
    /// </summary>
    public void ReturnToPool()
    {
        if (Pool)
        {
            Pool.AddObject(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Get an instance of this object from the pool. If one does not exist, it is created and added to this pool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetPooledInstance<T>() where T : PooledObject
    {
        if (!_poolInstanceForPrefab)
        {
            _poolInstanceForPrefab = ObjectPool.GetPool(this);
        }
        return (T)_poolInstanceForPrefab.GetObject();
    }
}
