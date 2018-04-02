using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Collections.Generic;

public class SquareTileMapGenerator : MonoBehaviour
{
    // Name of this map.
    [SerializeField] private string _mapName = "My new map";

    // Are we loading a map?
    [SerializeField] private SquareTileMapData _loadedMap;

    // Tile Prefab
    [SerializeField] private Transform _tilePrefab;

    // Tiles parent.
    [SerializeField] private Transform _tilesParent;

    // Obstacle Prefab
    [SerializeField] private Transform _obstaclePrefab;

    // Obstacle parent
    [SerializeField] private Transform _obstaclesParent;

    // Navmesh Floor
    [SerializeField] private Transform _navmeshFloor;

    // Navmesh Floor Mask.
    // Prevents areas of the navmesh floor not covered by tiles to be marked as walkable.
    [SerializeField] private Transform _navmeshMaskPrefab;

    // Navmesh Floor Masks parent.
    [SerializeField] private Transform _navmeshMasksParent;

    // Map parent
    [SerializeField] private Transform _mapParent;

    // Settings
    [SerializeField] private Vector2 _minMapSize;
    public Vector2 MinMapSize => _minMapSize;

    [SerializeField] private Vector2 _maxMapSize;

    [SerializeField]
    [Range(0, 1)] private float _obstaclePercent;
    public float ObstaclePercent => _obstaclePercent;

    [SerializeField] private int _obstaclePlaceSeed;
    public int ObstaclePlaceSeed => _obstaclePlaceSeed;

    [SerializeField] private float _minObstacleHeight;
    public float MinObstacleHeight => _minObstacleHeight;

    [SerializeField] private float _maxObstacleHeight;
    public float MaxObstacleHeight => _maxObstacleHeight;

    [SerializeField] private Color _foregroundColor;
    public Color ForegroundColor => _foregroundColor;

    [SerializeField] private Color _backgroundColor;
    public Color BackgroundColor => _backgroundColor;

    [SerializeField] private float _tileSize = 1f;

    [SerializeField]
    [Range(0, 1)] private float _tileOutlinePercent;
    public float TileOutlinePercent => _tileOutlinePercent;

    // Points list.
    private List<Point> _points = new List<Point>();

    // Point - Tile dictionary.
    private Dictionary<Point, SquareTile> _tiles = new Dictionary<Point, SquareTile>();
    // Tile - Obstacle dictionary.
    Dictionary<SquareTile, Transform> _obstacles = new Dictionary<SquareTile, Transform>();
    // Used for getting a random point.
    private Queue<Point> _shuffledTilePoints;
    // Used for getting a random tile that is not occupied.
    private Queue<Point> _unOccupiedTilePoints;

    private Point MapCenter => new Point((int)_minMapSize.x / 2, (int)_minMapSize.y / 2);

    public void GenerateMap()
    {
        // If empty parent transforms exist, then create them.
        if (_mapParent == null)
        {
            _mapParent = new GameObject("Generated SquareTileMap").transform;

            if (_tilesParent == null)
            {
                _tilesParent = new GameObject("Square Tiles").transform;
                _tilesParent.parent = _mapParent;
            }

            if (_obstaclesParent == null)
            {
                _obstaclesParent = new GameObject("Obstacles").transform;
                _obstaclesParent.parent = _mapParent;
            }

            if (_navmeshMasksParent == null)
            {
                _navmeshMasksParent = new GameObject("Navmesh Floor Masks").transform;
                _navmeshMasksParent.parent = _mapParent;
            }
        }

        // Set floor box collider size.
        _navmeshFloor.GetComponent<BoxCollider>().size = new Vector3(_minMapSize.x * _tileSize, 0.05f, _minMapSize.y * _tileSize);

        // Random number seed for placing obstacles and modifying obstacle height.
        var pRng = new System.Random(_obstaclePlaceSeed);

        // List of points in the map.
        _points = new List<Point>();
        for (var x = 0; x < _minMapSize.x; x++)
        {
            for (var y = 0; y < _minMapSize.y; y++)
            {
                // Add the associated point to the points array.
                _points.Add(new Point(x, y));
            }
        }

        // List of unoccupied points in the map. We remove points from this list during obstacle generation.
        var unoccupiedPoints = new List<Point>(_points);

        _shuffledTilePoints = new Queue<Point>(SquareTileMapUtility.ShuffleArray(_points.ToArray(), _obstaclePlaceSeed));

        // Destroy previous tiles.
        if (_tiles != null)
        {
            for (var i = _tilesParent.childCount - 1; i >= 0; --i)
            {
                DestroyImmediate(_tilesParent.GetChild(i).gameObject);
            }
            _tiles.Clear();
        }

        // Destroy previous obstacles.
        for (var i = _obstaclesParent.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(_obstaclesParent.GetChild(i).gameObject);
        }

        // Loop through axes to generate map.
        for (var x = 0; x < _minMapSize.x; x++)
        {
            for (var y = 0; y < _minMapSize.y; y++)
            {
                var tilePosition = PointToPosition(x, y);
                var newTile = Instantiate(_tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTile.localScale = Vector3.one * (1 - _tileOutlinePercent) * _tileSize;
                newTile.transform.parent = _tilesParent;

                if (newTile.GetComponent<SquareTile>() == null)
                    newTile.gameObject.AddComponent<SquareTile>();

                var newSquareTile = newTile.gameObject.GetComponent<SquareTile>();
                newSquareTile.SetPos(x, y);
                newSquareTile.Height = 0;
                // Add to tiles dictionary.
                _tiles.Add(new Point(x, y), newSquareTile);
                // Since we haven't generated obstacles yet, add this squaretile to the obstacles dictionary with a null obstacle.
                _obstacles.Add(newSquareTile, null);
            }
        }

        // Used in ensuring map connectivity.
        var obstacleMap = new bool[(int)_minMapSize.x, (int)_minMapSize.y];
        var obstacleFillAmount = (int)(_minMapSize.x * _minMapSize.y * _obstaclePercent);
        var obstacleCount = 0;

        // Generate obstacles.
        for (var i = 0; i < obstacleFillAmount; i++)
        {
            var randomPoint = GetRandomPoint();

            // We presume we are instantiating an obstacle at the point before we can check if the obstacle closes anything off.
            obstacleMap[randomPoint.X, randomPoint.Y] = true;
            obstacleCount++;

            // We never want to spawn an obstacle in the center of the map or in a place that would cut off a path.
            if (randomPoint != MapCenter && MapIsFullyAccessible(obstacleMap, obstacleCount))
            {
                var obstacleHeight = Mathf.Lerp(_minObstacleHeight, _maxObstacleHeight, (float)pRng.NextDouble());
                var obstaclePosition = PointToPosition(randomPoint.X, randomPoint.Y);
                var newObstacle = Instantiate(_obstaclePrefab, obstaclePosition + (Vector3.up * obstacleHeight / 2f), Quaternion.identity);
                newObstacle.localScale = new Vector3((1 - _tileOutlinePercent) * _tileSize, obstacleHeight, (1 - _tileOutlinePercent) * _tileSize);
                newObstacle.parent = _obstaclesParent;

                // Remove this point from our unoccupied points list.
                unoccupiedPoints.Remove(randomPoint);

                // Add to obstacles dictionary.
                // Get the tile at our random point.
                var ourTile = _tiles[randomPoint];
                // Add obstacle transform to obstacles dictionary.
                _obstacles[ourTile] = newObstacle;

                // Color settings based on foreground/background colors.
                var obstacleRenderer = newObstacle.GetComponent<Renderer>();
                var obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);

                // Determine how far 'forward' this obstacle is.
                var colorPercent = randomPoint.Y / _minMapSize.y;

                obstacleMaterial.color = Color.Lerp(_foregroundColor, _backgroundColor, colorPercent);
                obstacleRenderer.sharedMaterial = obstacleMaterial;
            }
            else
            {
                // If the check fails, then we remove the obstacle location from the obstacle map.
                obstacleMap[randomPoint.X, randomPoint.Y] = false;
                obstacleCount--;
            }
        }

        _unOccupiedTilePoints = new Queue<Point>(SquareTileMapUtility.ShuffleArray(unoccupiedPoints.ToArray(), _obstaclePlaceSeed));

        // Destroy previous navmesh floor masks.
        for (var i = _navmeshMasksParent.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(_navmeshMasksParent.GetChild(i).gameObject);
        }

        // Destroy previous navmesh floor.
        for (var i = 0; i < _mapParent.childCount; i++)
        {
            if (_mapParent.GetChild(i).GetComponent<NavmeshFloor>())
            {
                DestroyImmediate(_mapParent.GetChild(i).gameObject);
                break;
            }
        }

        // Generate navmesh floor.
        var ourFloor = Instantiate(_navmeshFloor);
        ourFloor.localRotation = Quaternion.Euler(90f, 0f, 0f);
        ourFloor.localScale = new Vector3(_maxMapSize.x, _maxMapSize.y) * _tileSize;
        ourFloor.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 0);
        ourFloor.parent = _mapParent;

        // Generate navmesh floor masks.
        var maskLeft = Instantiate(_navmeshMaskPrefab, Vector3.left * (_minMapSize.x + _maxMapSize.x) / 4f * _tileSize, Quaternion.identity);
        maskLeft.gameObject.AddComponent<BoxCollider>();
        maskLeft.parent = _navmeshMasksParent;
        var maskRight = Instantiate(_navmeshMaskPrefab, Vector3.right * (_minMapSize.x + _maxMapSize.x) / 4f * _tileSize, Quaternion.identity);
        maskRight.gameObject.AddComponent<BoxCollider>();
        maskRight.parent = _navmeshMasksParent;
        var maskTop = Instantiate(_navmeshMaskPrefab, Vector3.forward * (_minMapSize.y + _maxMapSize.y) / 4f * _tileSize, Quaternion.identity);
        maskTop.gameObject.AddComponent<BoxCollider>();
        maskTop.parent = _navmeshMasksParent;
        var maskBottom = Instantiate(_navmeshMaskPrefab, Vector3.back * (_minMapSize.y + _maxMapSize.y) / 4f * _tileSize, Quaternion.identity);
        maskBottom.gameObject.AddComponent<BoxCollider>();
        maskBottom.parent = _navmeshMasksParent;

        maskLeft.localScale = new Vector3((_maxMapSize.x - _minMapSize.x) / 2f, 1, _minMapSize.y) * _tileSize;
        maskRight.localScale = new Vector3((_maxMapSize.x - _minMapSize.x) / 2f, 1, _minMapSize.y) * _tileSize;
        maskTop.localScale = new Vector3(_maxMapSize.x, 1, (_maxMapSize.y - _minMapSize.y) / 2f) * _tileSize;
        maskBottom.localScale = new Vector3(_maxMapSize.x, 1, (_maxMapSize.y - _minMapSize.y) / 2f) * _tileSize;

        // Save to map parent.
        if (_mapParent.GetComponent<SquareTileMap>() == null)
            _mapParent.gameObject.AddComponent<SquareTileMap>();

        _mapParent.GetComponent<SquareTileMap>().Setup(_minMapSize, _obstaclePercent, _obstaclePlaceSeed, _minObstacleHeight, _maxObstacleHeight, _foregroundColor, _backgroundColor);

    }

#if UNITY_EDITOR
    /// <summary>
    /// Saves the current squaretilemap as a data asset file.
    /// </summary>
    public void SaveMap()
    {
        var filePath = Application.dataPath + "/Resources/SquareTileMaps/";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();

        var savedSquareTileMap = ScriptableObject.CreateInstance<SquareTileMapData>();

        // Populate data values.
        savedSquareTileMap.SquareTiles = new List<Vector3>(_tiles.Count);
        foreach (var t in _tiles.Values)
            savedSquareTileMap.SquareTiles.Add(new Vector3(t.Pos.X, t.Height, t.Pos.Y));
        savedSquareTileMap.MinMapSize = _minMapSize;
        savedSquareTileMap.MaxMapSize = _maxMapSize;
        savedSquareTileMap.ObstaclePercent = _obstaclePercent;
        savedSquareTileMap.ObstaclePlaceSeed = _obstaclePlaceSeed;
        savedSquareTileMap.MinObstacleHeight = _minObstacleHeight;
        savedSquareTileMap.MaxObstacleHeight = _maxObstacleHeight;
        savedSquareTileMap.ForegroundColor = _foregroundColor;
        savedSquareTileMap.BackgroundColor = _backgroundColor;
        savedSquareTileMap.TileSize = _tileSize;
        savedSquareTileMap.TileOutlinePercent = _tileOutlinePercent;
        savedSquareTileMap.Tiles = _tiles;
        savedSquareTileMap.Obstacles = _obstacles;

        var fileName = string.Format("Assets/Resources/SquareTileMaps/{0}.asset", _mapName);
        AssetDatabase.CreateAsset(savedSquareTileMap, fileName);
    }

    public void LoadMap()
    {
        if (_loadedMap == null)
            return;

        _tiles.Clear();
        _minMapSize = _loadedMap.MinMapSize;
        _maxMapSize = _loadedMap.MaxMapSize;
        _obstaclePercent = _loadedMap.ObstaclePercent;
        _obstaclePlaceSeed = _loadedMap.ObstaclePlaceSeed;
        _minObstacleHeight = _loadedMap.MinObstacleHeight;
        _maxObstacleHeight = _loadedMap.MaxObstacleHeight;
        _foregroundColor = _loadedMap.ForegroundColor;
        _backgroundColor = _loadedMap.BackgroundColor;
        _tileSize = _loadedMap.TileSize;
        _tileOutlinePercent = _loadedMap.TileOutlinePercent;
        GenerateMap();
    }

    private void CreateSaveDirectory()
    {
        var filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/SquareTileMaps";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "SquareTileMaps");
        AssetDatabase.Refresh();
    }
#endif

    // Uses a flood-fill algorithm to check for map connectivity.
    private bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        var mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        var queue = new Queue<Point>();

        // The center of the map should never have an obstacle, so preemptively set its location to true.
        // Note that this can be changed to any arbitrary point.
        queue.Enqueue(MapCenter);
        mapFlags[MapCenter.X, MapCenter.Y] = true;
        var accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            var tile = queue.Dequeue();

            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    // Loop through and check each adjacent tile.
                    var neighborX = tile.X + x;
                    var neighborY = tile.Y + y;

                    // Since we only care about cardinal adjacent tiles, exclude diagonal neighbors.
                    if (x == 0 || y == 0)
                    {
                        // Ensure the adjacent tile we are checking is actually a point in the obstacle map.
                        if (neighborX >= 0 && neighborX < obstacleMap.GetLength(0) && neighborY >= 0 && neighborY < obstacleMap.GetLength(1))
                        {
                            // Ignore this spot if we have checked this tile previously or if it is already marked as an obstacle's location.
                            if (!mapFlags[neighborX, neighborY] && !obstacleMap[neighborX, neighborY])
                            {
                                mapFlags[neighborX, neighborY] = true;
                                queue.Enqueue(new Point(neighborX, neighborY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        var targetAccessibleTilecount = (int)(_minMapSize.x * _minMapSize.y - currentObstacleCount);
        return targetAccessibleTilecount == accessibleTileCount;
    }

    /// <summary>
    /// Returns a random unoccupied tile.
    /// </summary>
    /// <returns>The transform of a random unoccupied tile.</returns>
    public Transform GetRandomUnoccupiedTile()
    {
        var randomPoint = _unOccupiedTilePoints.Dequeue();
        _unOccupiedTilePoints.Enqueue(randomPoint);
        return _tiles[randomPoint].transform;
    }

    /// <summary>
    /// Gets the transform at a position.
    /// </summary>
    /// <param name="position">Vector3 of the position.</param>
    /// <returns>The transform of the object at the position.</returns>
    public Transform GetTransformFromPosition (Vector3 position)
    {
        var x = Mathf.RoundToInt(position.x / _tileSize + (_minMapSize.x - 1) / 2f);
        var y = Mathf.RoundToInt(position.z / _tileSize + (_minMapSize.y - 1) / 2f);

        x = Mathf.Clamp(x, 0, (int) _maxMapSize.x);
        y = Mathf.Clamp(y, 0, (int) _maxMapSize.y);

        return _tiles[new Point(x, y)].transform;
    }

    private Point GetRandomPoint()
    {
        var randomPoint = _shuffledTilePoints.Dequeue();
        _shuffledTilePoints.Enqueue(randomPoint);

        return randomPoint;
    }

    private Vector3 PointToPosition(int x, int y)
    {
        return new Vector3(-_minMapSize.x / 2f + 0.5f + x, 0, -_minMapSize.y / 2f + 0.5f + y) * _tileSize;
    }

    private void Start()
    {
        if (_tilesParent == null)
            _tilesParent = transform;
    }
}
