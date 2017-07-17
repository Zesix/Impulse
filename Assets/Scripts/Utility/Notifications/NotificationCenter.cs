using UnityEngine;
using System.Collections.Generic;

#pragma warning disable 1587
/// <summary>
/// This delegate is similar to an EventHandler:
///     The first parameter is the sender, 
///     The second parameter is the arguments / info to pass
/// </summary>
using Handler = System.Action<System.Object, System.Object>;

/// <summary>
/// The SenderTable maps from an object (sender of a notification), 
/// to a List of Handler methods
///     * Note - When no sender is specified for the SenderTable, 
///         the NotificationCenter itself is used as the sender key
/// </summary>
#pragma warning restore 1587
using SenderTable = System.Collections.Generic.Dictionary<System.Object, System.Collections.Generic.List<System.Action<System.Object, System.Object>>>;

public class NotificationCenter
{
    #region Properties
    /// <summary>
    /// The dictionary "key" (string) represents a notificationName property to be observed
    /// The dictionary "value" (SenderTable) maps between sender and observer sub tables
    /// </summary>
    private Dictionary<string, SenderTable> _table = new Dictionary<string, SenderTable>();
    private HashSet<List<Handler>> _invoking = new HashSet<List<Handler>>();
    #endregion

    // Singleton Pattern
    public static readonly NotificationCenter Instance = new NotificationCenter();
    private NotificationCenter() { }

    public void AddObserver(Handler handler, string notificationName)
    {
        AddObserver(handler, notificationName, null);
    }

    public void AddObserver(Handler handler, string notificationName, System.Object sender)
    {
        if (handler == null)
        {
            Debug.LogError("Can't add a null event handler for notification, " + notificationName);
            return;
        }

        if (string.IsNullOrEmpty(notificationName))
        {
            Debug.LogError("Can't observe an unnamed notification");
            return;
        }

        if (!_table.ContainsKey(notificationName))
            _table.Add(notificationName, new SenderTable());

        var subTable = _table[notificationName];

        var key = (sender != null) ? sender : this;

        if (!subTable.ContainsKey(key))
            subTable.Add(key, new List<Handler>());

        var list = subTable[key];
        if (!list.Contains(handler))
        {
            if (_invoking.Contains(list))
                subTable[key] = list = new List<Handler>(list);

            list.Add(handler);
        }
    }

    public void RemoveObserver(Handler handler, string notificationName)
    {
        RemoveObserver(handler, notificationName, null);
    }

    public void RemoveObserver(Handler handler, string notificationName, object sender)
    {
        if (handler == null)
        {
            Debug.LogError("Can't remove a null event handler for notification, " + notificationName);
            return;
        }

        if (string.IsNullOrEmpty(notificationName))
        {
            Debug.LogError("A notification name is required to stop observation");
            return;
        }

        // No need to take action if we dont monitor this notification
        if (!_table.ContainsKey(notificationName))
            return;

        var subTable = _table[notificationName];
        var key = sender != null ? sender : this;

        if (!subTable.ContainsKey(key))
            return;

        var list = subTable[key];
        var index = list.IndexOf(handler);
        if (index != -1)
        {
            if (_invoking.Contains(list))
                subTable[key] = list = new List<Handler>(list);
            list.RemoveAt(index);
        }
    }

    public void Clean()
    {
        var notKeys = new string[_table.Keys.Count];
        _table.Keys.CopyTo(notKeys, 0);

        for (var i = notKeys.Length - 1; i >= 0; --i)
        {
            var notificationName = notKeys[i];
            var senderTable = _table[notificationName];

            var senKeys = new object[senderTable.Keys.Count];
            senderTable.Keys.CopyTo(senKeys, 0);

            for (var j = senKeys.Length - 1; j >= 0; --j)
            {
                var sender = senKeys[j];
                var handlers = senderTable[sender];
                if (handlers.Count == 0)
                    senderTable.Remove(sender);
            }

            if (senderTable.Count == 0)
                _table.Remove(notificationName);
        }
    }

    public void PostNotification(string notificationName)
    {
        PostNotification(notificationName, null);
    }

    public void PostNotification(string notificationName, System.Object sender)
    {
        PostNotification(notificationName, sender, null);
    }

    public void PostNotification(string notificationName, System.Object sender, System.Object e)
    {
        if (string.IsNullOrEmpty(notificationName))
        {
            Debug.LogError("A notification name is required");
            return;
        }

        // No need to take action if we dont monitor this notification
        if (!_table.ContainsKey(notificationName))
            return;

        // Post to subscribers who specified a sender to observe
        var subTable = _table[notificationName];
        if (sender != null && subTable.ContainsKey(sender))
        {
            var handlers = subTable[sender];
            _invoking.Add(handlers);
            foreach (var t in handlers)
                t(sender, e);
            _invoking.Remove(handlers);
        }

        // Post to subscribers who did not specify a sender to observe
        if (subTable.ContainsKey(this))
        {
            var handlers = subTable[this];
            _invoking.Add(handlers);
            foreach (var t in handlers)
                t(sender, e);
            _invoking.Remove(handlers);
        }
    }
}