using UnityEngine;
using System.Collections;

[System.Serializable]
public class SquareTileMap : MonoBehaviour
{
    // Sizes of this map, given in point format (X,Y).
    [SerializeField]
    Vector2 minMapSize;
    public Vector2 MinMapSize
    {
        get { return minMapSize; }
    }

    // Percentage of obstacles.
    [SerializeField]
    [Range(0, 1)]
    float obstaclePercent;
    public float ObstaclePercent
    {
        get { return obstaclePercent; }
    }

    // Obstacle placement seed.
    [SerializeField]
    int obstaclePlaceSeed;
    public int ObstaclePlaceSeed
    {
        get { return obstaclePlaceSeed; }
    }

    // Obstacle heights.
    [SerializeField]
    float minObstacleHeight;
    public float MinObstacleHeight
    {
        get { return minObstacleHeight; }
    }

    [SerializeField]
    float maxObstacleHeight;
    public float MaxObstacleHeight
    {
        get { return maxObstacleHeight; }
    }

    // Colors.
    [SerializeField]
    Color foregroundColor;
    public Color ForegroundColor
    {
        get { return foregroundColor; }
    }

    [SerializeField]
    Color backgroundColor;
    public Color BackgroundColor
    {
        get { return backgroundColor; }
    }

    public Point MapCenter
    {
        get { return new Point((int)minMapSize.x / 2, (int)minMapSize.y / 2); }
    }

    public void Setup (Vector2 minMapSize, float obstaclePercent, int obstaclePlaceSeed, float minObstacleHeight, float maxObstacleHeight, Color foregroundColor, Color backgroundColor)
    {
        this.minMapSize = minMapSize;
        this.obstaclePercent = obstaclePercent;
        this.obstaclePlaceSeed = obstaclePlaceSeed;
        this.minObstacleHeight = minObstacleHeight;
        this.maxObstacleHeight = maxObstacleHeight;
        this.foregroundColor = foregroundColor;
        this.backgroundColor = backgroundColor;
    }
}
