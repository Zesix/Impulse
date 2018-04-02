using UnityEngine;

[System.Serializable]
public class SquareTileMap : MonoBehaviour
{
    // Sizes of this map, given in point format (X,Y).
    [SerializeField] private Vector2 _minMapSize;
    public Vector2 MinMapSize => _minMapSize;

    // Percentage of obstacles.
    [SerializeField]
    [Range(0, 1)] private float _obstaclePercent;
    public float ObstaclePercent => _obstaclePercent;

    // Obstacle placement seed.
    [SerializeField] private int _obstaclePlaceSeed;
    public int ObstaclePlaceSeed => _obstaclePlaceSeed;

    // Obstacle heights.
    [SerializeField] private float _minObstacleHeight;
    public float MinObstacleHeight => _minObstacleHeight;

    [SerializeField] private float _maxObstacleHeight;
    public float MaxObstacleHeight => _maxObstacleHeight;

    // Colors.
    [SerializeField] private Color _foregroundColor;
    public Color ForegroundColor => _foregroundColor;

    [SerializeField] private Color _backgroundColor;
    public Color BackgroundColor => _backgroundColor;

    public Point MapCenter => new Point((int)_minMapSize.x / 2, (int)_minMapSize.y / 2);

    public void Setup (Vector2 minMapSize, float obstaclePercent, int obstaclePlaceSeed, float minObstacleHeight, float maxObstacleHeight, Color foregroundColor, Color backgroundColor)
    {
        _minMapSize = minMapSize;
        _obstaclePercent = obstaclePercent;
        _obstaclePlaceSeed = obstaclePlaceSeed;
        _minObstacleHeight = minObstacleHeight;
        _maxObstacleHeight = maxObstacleHeight;
        _foregroundColor = foregroundColor;
        _backgroundColor = backgroundColor;
    }
}
