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

    public U GetScript<U>(T key) where U : MonoBehaviour
    {
        Poolable item = GetItem(key);
        if (item != null)
            return item.GetComponent<U>();
        return null;
    }

    public virtual void EnqueueByKey(T key)
    {
        Poolable item = GetItem(key);
        if (item != null)
        {
            if (WillEnqueueForKey != null)
                WillEnqueueForKey(item, key);
            Enqueue(item);
            Collection.Remove(key);
        }
    }

    public virtual Poolable DequeueByKey(T key)
    {
        if (Collection.ContainsKey(key))
            return Collection[key];

        Poolable item = Dequeue();
        Collection.Add(key, item);
        if (DidDequeueForKey != null)
            DidDequeueForKey(item, key);
        return item;
    }

    public virtual U DequeueScriptByKey<U>(T key) where U : MonoBehaviour
    {
        Poolable item = DequeueByKey(key);
        return item.GetComponent<U>();
    }

    public override void EnqueueAll()
    {
        T[] keys = new T[Collection.Count];
        Collection.Keys.CopyTo(keys, 0);
        for (int i = 0; i < keys.Length; ++i)
            EnqueueByKey(keys[i]);
    }
}