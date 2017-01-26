using UnityEngine;
using System;

public abstract class BasePooler : MonoBehaviour
{
    // Events
    public Action<Poolable> WillEnqueue;
    public Action<Poolable> DidDequeue;

    // Fields / Properties
    public string Key = string.Empty;
    public GameObject Prefab = null;
    public int PrepopulateAmt = 0;
    public int MaxCount = int.MaxValue;
    public bool AutoRegister = true;
    public bool AutoClear = true;
    public bool IsRegistered { get; private set; }

    protected virtual void Awake()
    {
        if (AutoRegister)
            Register();
    }

    protected virtual void OnDestroy()
    {
        EnqueueAll();
        if (AutoClear)
            UnRegister();
    }

    protected virtual void OnApplicationQuit()
    {
        EnqueueAll();
    }

    public void Register()
    {
        if (string.IsNullOrEmpty(Key))
            Key = Prefab.name;
        GameObjectPoolController.AddEntry(Key, Prefab, PrepopulateAmt, MaxCount);
        IsRegistered = true;
    }

    public void UnRegister()
    {
        GameObjectPoolController.ClearEntry(Key);
        IsRegistered = false;
    }

    public virtual void Enqueue(Poolable item)
    {
        if (WillEnqueue != null)
            WillEnqueue(item);
        GameObjectPoolController.Enqueue(item);
    }

    public virtual void EnqueueObject(GameObject obj)
    {
        Poolable item = obj.GetComponent<Poolable>();
        if (item != null)
            Enqueue(item);
    }

    public virtual void EnqueueScript(MonoBehaviour script)
    {
        Poolable item = script.GetComponent<Poolable>();
        if (item != null)
            Enqueue(item);
    }

    public virtual Poolable Dequeue()
    {
        Poolable item = GameObjectPoolController.Dequeue(Key);
        if (DidDequeue != null)
            DidDequeue(item);
        return item;
    }

    public virtual U DequeueScript<U>() where U : MonoBehaviour
    {
        Poolable item = Dequeue();
        return item.GetComponent<U>();
    }

    public abstract void EnqueueAll();
}