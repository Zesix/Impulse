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

    public T GetPooledInstance<T>() where T : PooledObject
    {
        if (!_poolInstanceForPrefab)
        {
            _poolInstanceForPrefab = ObjectPool.GetPool(this);
        }
        return (T)_poolInstanceForPrefab.GetObject();
    }
}
