using UnityEngine;
using System;

public abstract class BasePooler : MonoBehaviour
{
    // Events
    public Action<Poolable> WillEnqueue;
    public Action<Poolable> DidDequeue;

    // Fields / Properties
    public string Key = string.Empty;
    public GameObject Prefab;
    public int PrepopulateAmt;
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
        WillEnqueue?.Invoke(item);
        GameObjectPoolController.Enqueue(item);
    }

    public virtual void EnqueueObject(GameObject obj)
    {
        var item = obj.GetComponent<Poolable>();
        if (item != null)
            Enqueue(item);
    }

    public virtual void EnqueueScript(MonoBehaviour script)
    {
        var item = script.GetComponent<Poolable>();
        if (item != null)
            Enqueue(item);
    }

    public virtual Poolable Dequeue()
    {
        var item = GameObjectPoolController.Dequeue(Key);
        DidDequeue?.Invoke(item);
        return item;
    }

    public virtual U DequeueScript<U>() where U : MonoBehaviour
    {
        var item = Dequeue();
        return item.GetComponent<U>();
    }

    public abstract void EnqueueAll();
}