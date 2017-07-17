using UnityEngine;
using System.Collections.Generic;

public class SquareTileMapData : ScriptableObject
{
    public Vector2 MinMapSize;
    public Vector2 MaxMapSize;
    [Range(0,1)]
    public float ObstaclePercent;
    public int ObstaclePlaceSeed;
    public float MinObstacleHeight;
    public float MaxObstacleHeight;
    public Color ForegroundColor;
    public Color BackgroundColor;
    public float TileSize;
    [Range(0,1)]
    public float TileOutlinePercent;
    public List<Vector3> SquareTiles;
    public Dictionary<Point, SquareTile> Tiles;
    public Dictionary<SquareTile, Transform> Obstacles;
}
