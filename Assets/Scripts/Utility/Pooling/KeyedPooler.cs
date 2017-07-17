using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class KeyedPooler<T> : BasePooler
{
    // Events
    public Action<Poolable, T> WillEnqueueForKey;
    public Action<Poolable, T> DidDequeueForKey;

    // Fields / Properties
    public Dictionary<T, Poolable> Collection = new Dictionary<T, Poolable>();

    public bool HasKey(T key)
    {
        return Collection.ContainsKey(key);
    }

    public Poolable GetItem(T key)
    {
        if (Collection.ContainsKey(key))
            return Collection[key];
        return null;
    }

    public TU GetScript<TU>(T key) where TU : MonoBehaviour
    {
        var item = GetItem(key);
        if (item != null)
            return item.GetComponent<TU>();
        return null;
    }

    public virtual void EnqueueByKey(T key)
    {
        var item = GetItem(key);
        if (item != null)
        {
            WillEnqueueForKey?.Invoke(item, key);
            Enqueue(item);
            Collection.Remove(key);
        }
    }

    public virtual Poolable DequeueByKey(T key)
    {
        if (Collection.ContainsKey(key))
            return Collection[key];

        var item = Dequeue();
        Collection.Add(key, item);
        DidDequeueForKey?.Invoke(item, key);
        return item;
    }

    public virtual TU DequeueScriptByKey<TU>(T key) where TU : MonoBehaviour
    {
        var item = DequeueByKey(key);
        return item.GetComponent<TU>();
    }

    public override void EnqueueAll()
    {
        var keys = new T[Collection.Count];
        Collection.Keys.CopyTo(keys, 0);
        foreach (var t in keys)
            EnqueueByKey(t);
    }
}