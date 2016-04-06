using UnityEngine;
using System.Collections;

public class SquareTile : MonoBehaviour
{
    #region Properties
    // The (x,y) point of this square tile.
    [SerializeField]
    Point pos;
    public Point Pos
    {
        get { return pos; }
    }

    // The height of this tile.
    [SerializeField]
    float height = 0;
    public float Height
    {
        get { return height; }
        set { height = value; }
    }

    // The center of this tile.
    public Vector3 center
    {
        get { return new Vector3(pos.x, height, pos.y); }
    }
    #endregion

    /// <summary>
    /// Updates this tile's transform based on point position.
    /// </summary>
    public void UpdateTile()
    {
        transform.localPosition = new Vector3(pos.x, height, pos.y);
    }

    /// <summary>
    /// Sets the Pos coordinates of this tile.
    /// </summary>
    /// <param name="x">X Point coordinate.</param>
    /// <param name="y">Y Point coordinate.</param>
    public void SetPos (int x, int y)
    {
        pos.x = x;
        pos.y = y;
    }
}
