using UnityEngine;

public class SquareTile : MonoBehaviour
{
    // The (x,y) point of this square tile.
    [SerializeField] private Point _pos;
    public Point Pos => _pos;

    // The height of this tile.
    [SerializeField] private float _height;
    public float Height
    {
        get { return _height; }
        set { _height = value; }
    }

    // The center of this tile.
    public Vector3 Center => new Vector3(_pos.X, _height, _pos.Y);

    /// <summary>
    /// Updates this tile's transform based on point position.
    /// </summary>
    public void UpdateTile()
    {
        transform.localPosition = new Vector3(_pos.X, _height, _pos.Y);
    }

    /// <summary>
    /// Sets the Pos coordinates of this tile.
    /// </summary>
    /// <param name="x">X Point coordinate.</param>
    /// <param name="y">Y Point coordinate.</param>
    public void SetPos (int x, int y)
    {
        _pos.X = x;
        _pos.Y = y;
    }
}
