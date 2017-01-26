using System.Collections.Generic;
using UnityEngine;

public class PoolData
{
    public GameObject Prefab;
    public int MaxCount;
    public Queue<Poolable> Pool;
}

public class GameObjectPoolController : MonoBehaviour
{
    private static GameObjectPoolController _instance;
    private static GameObjectPoolController Instance
    {
        get
        {
            if (_instance == null)
                CreateSharedInstance();
            return _instance;
        }
    }

    private static Dictionary<string, PoolData> _pools = new Dictionary<string, PoolData>();

    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
            Destroy(this);
        else
            _instance = this;
    }

    public static void SetMaxCount(string key, int maxCount)
    {
        if (!_pools.ContainsKey(key))
            return;
        PoolData data = _pools[key];
        data.MaxCount = maxCount;
    }

    public static bool AddEntry(string key, GameObject prefab, int prepopulateAmount, int maxCount)
    {
        if (_pools.ContainsKey(key))
            return false;
        PoolData data = new PoolData();
        data.Prefab = prefab;
        data.MaxCount = maxCount;
        data.Pool = new Queue<Poolable>(prepopulateAmount);
        _pools.Add(key, data);

        for (int i = 0; i < prepopulateAmount; ++i)
            Enqueue(CreateInstance(key, prefab));
        return true;
    }

    public static void ClearEntry(string key)
    {
        if (!_pools.ContainsKey(key))
            return;

        PoolData data = _pools[key];
        while (data.Pool.Count > 0)
        {
            Poolable obj = data.Pool.Dequeue();
            Destroy(obj.gameObject);
        }
        _pools.Remove(key);
    }

    public static void Enqueue(Poolable sender)
    {
        if (sender == null || sender.IsPooled || !_pools.ContainsKey(sender.Key))
            return;

        PoolData data = _pools[sender.Key];
        if (data.Pool.Count >= data.MaxCount)
        {
            Destroy(sender.gameObject);
            return;
        }

        data.Pool.Enqueue(sender);
        sender.IsPooled = true;
        sender.transform.SetParent(Instance.transform);
        sender.gameObject.SetActive(false);
    }

    public static Poolable Dequeue(string key)
    {
        if (!_pools.ContainsKey(key))
            return null;

        PoolData data = _pools[key];
        if (data.Pool.Count == 0)
            return CreateInstance(key, data.Prefab);

        Poolable obj = data.Pool.Dequeue();
        obj.IsPooled = false;
        return obj;
    }

    private static void CreateSharedInstance()
    {
        GameObject obj = new GameObject("GameObject Pool Controller");
        DontDestroyOnLoad(obj);
        _instance = obj.AddComponent<GameObjectPoolController>();
    }

    private static Poolable CreateInstance(string key, GameObject prefab)
    {
        GameObject instance = Instantiate(prefab);
        Poolable poolableComponent = instance.AddComponent<Poolable>();
        poolableComponent.Key = key;
        return poolableComponent;
    }
}
