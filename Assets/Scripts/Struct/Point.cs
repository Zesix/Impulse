using System;
using UnityEngine;

[Serializable]
// A Point consists of x,y coordinates that are usually represented in x,z space.
public struct Point : IEquatable<Point>
{
    // Point components.
    public int X;
    public int Y;

    // Constructor.
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Neighbors.
    public Point Right()
    {
        return new Point(X + 1, Y);
    }

    public Point Left()
    {
        return new Point(X - 1, Y);
    }

    public Point Up()
    {
        return new Point(X, Y + 1);
    }

    public Point Down()
    {
        return new Point(X, Y - 1);
    }

    public Point UpRight()
    {
        return new Point(X + 1, Y + 1);
    }

    public Point UpLeft()
    {
        return new Point(X - 1, Y + 1);
    }

    public Point DownRight()
    {
        return new Point(X + 1, Y - 1);
    }

    public Point DownLeft()
    {
        return new Point(X - 1, Y - 1);
    }

    // Operators
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.X + b.X, a.Y + b.Y);
    }

    public static Point operator -(Point a, Point b)
    {
        return new Point(a.X - b.X, a.Y - b.Y);
    }

    public static bool operator ==(Point a, Point b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is Point)
        {
            var p = (Point)obj;
            return X == p.X && Y == p.Y;
        }
        return false;
    }

    public bool Equals(Point p)
    {
        return X == p.X && Y == p.Y;
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    // Override ToString() to make it easier for debugging.
    public override string ToString()
    {
        return string.Format("({0},{1})", X, Y);
    }

    public static explicit operator Point(Vector2 v)
    {
        return new Point((int)v.x, (int)v.y);
    }
}
