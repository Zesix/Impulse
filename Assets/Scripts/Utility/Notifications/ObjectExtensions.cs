using System;
using System.Collections;

public static class ObjectExtensions
{
    public static void PostNotification(this object obj, string notificationName)
    {
        NotificationCenter.instance.PostNotification(notificationName, obj);
    }

    public static void PostNotification(this object obj, string notificationName, EventArgs e)
    {
        NotificationCenter.instance.PostNotification(notificationName, obj, e);
    }

    public static void AddObserver(this object obj, EventHandler handler, string notificationName)
    {
        NotificationCenter.instance.AddObserver(handler, notificationName);
    }

    public static void AddObserver(this object obj, EventHandler handler, string notificationName, object sender)
    {
        NotificationCenter.instance.AddObserver(handler, notificationName, sender);
    }

    public static void RemoveObserver(this object obj, EventHandler handler)
    {
        NotificationCenter.instance.RemoveObserver(handler);
    }

    public static void RemoveObserver(this object obj, EventHandler handler, string notificationName)
    {
        NotificationCenter.instance.RemoveObserver(handler, notificationName);
    }

    public static void RemoveObserver(this object obj, EventHandler handler, string notificationName, System.Object sender)
    {
        NotificationCenter.instance.RemoveObserver(handler, notificationName, sender);
    }
}