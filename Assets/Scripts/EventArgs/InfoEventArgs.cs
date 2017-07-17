using System;

/// <summary>
/// A simple class to hold a single field of any data type named info.
/// Used often with input events since a single field of data is all that is ever passed at a time.
/// </summary>
public class InfoEventArgs<T> : EventArgs
{
    public T Info;

    /// <summary>
    /// An empty constructor that inits itself using the default keyword (handles both reference and value types).
    /// </summary>
    public InfoEventArgs()
    {
        Info = default(T);
    }

    /// <summary>
    /// A constructor that allows the user to specify the initial value.
    /// </summary>
    /// <param name="info">Initial value.</param>
    public InfoEventArgs(T info)
    {
        Info = info;
    }
}
