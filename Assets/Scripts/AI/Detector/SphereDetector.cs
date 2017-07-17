using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generic Detector Script (Detects Generic Faction Objects)
/// </summary>
public class SphereDetector : MonoBehaviour
{

    // Detection parameters
    [SerializeField]
    [Range(0, 1000)] private float _detectionRange = 15.0f;
    public float DetectionRange => _detectionRange;
    
    [SerializeField]
    [Range(0, 1000)] private float _attackRange = 5.0f;
    public float AttackRange => _attackRange;
    
    [SerializeField]
    [Range(0, 1000)] private float _avoidRange = 1.0f;
    public float AvoidRange => _avoidRange;
    
    [SerializeField]
    [Range(0, 5)] private float _detectionRate = 0.1f;
    public float DetectionRate => _detectionRate;
    
    [SerializeField] private float _largestRange;

    // Faction parameters
    [SerializeField] private List<Faction.Factions> _allyFactions = new List<Faction.Factions>();
    [SerializeField] private List<Faction.Factions> _enemyFactions = new List<Faction.Factions>();

    // Detected Objects
    public List<Faction> DetectedEnemies = new List<Faction>();
    public List<Faction> DetectedAllies = new List<Faction>();
    public List<Faction> DetectedNeutral = new List<Faction>();

    // Debug
    [SerializeField] private bool _displayDetectionRange = true;
    [SerializeField] private bool _displayAttackRange = true;
    [SerializeField] private bool _displayAvoidRange = true;

    private void Start()
    {
        // Get largest search range
        var ranges = new[] { _detectionRange, _attackRange, _avoidRange };
        _largestRange = Mathf.Max(ranges);

        // Start Automatic Faction Detection
        InvokeRepeating(nameof(DetectFactionObjects), 0.0f, _detectionRate);
    }

    /// <summary>
    /// Detects Faction Objects in Range
    /// </summary>
    private void DetectFactionObjects()
    {
        // Clear previous detected characters
        DetectedEnemies.Clear();
        DetectedAllies.Clear();
        DetectedNeutral.Clear();

        // Detect all characters in range
        var hitColliders = Physics.OverlapSphere(transform.position, _largestRange);
        var i = 0;
        while (i < hitColliders.Length)
        {
            //Ignore self object
            if (hitColliders[i].gameObject == gameObject)
			{
	            // Move to the next object
				i++;
                continue;
			}

            //Check if the object has a faction
            var currentObject = hitColliders[i].GetComponent<Faction>();

            //Organize the detected factions
            if (currentObject != null)
            {
				// Check if it is an ally
                if (_allyFactions.Contains(currentObject.FactionName))
                {
                    DetectedAllies.Add(currentObject);
                }
                // Check if it is an enemy
                else if (_enemyFactions.Contains(currentObject.FactionName))
                {
                    DetectedEnemies.Add(currentObject);
                }
                //Otherwise, set it as a neutral object
                else
                    DetectedNeutral.Add(currentObject);
            }

            // Move to the next object
            i++;
        }
    }

    /// <summary>
    /// Returns closest enemy game object within detection range.
    /// </summary>
    /// <returns></returns>
    public GameObject ClosestEnemy()
    {
        GameObject closestEnemy = null;
        const float currentMinDistance = Mathf.Infinity;

        // Analize all detected enemies
        foreach (var current in DetectedEnemies)
        {
            // Get distance to the enemy
            var distance = Vector3.Distance(transform.position, current.transform.position);

            // First check the enemy is in the detection range and is the closest distance found until now
            if (distance <= _detectionRange && distance < currentMinDistance)
                closestEnemy = current.gameObject;
        }

        return closestEnemy;
    }

    /// <summary>
    /// Returns closest ally game object within detection range.
    /// </summary>
    /// <returns></returns>
    public GameObject ClosestAlly()
    {
        GameObject closestAlly = null;
        const float currentMinDistance = Mathf.Infinity;

        // Analize all detected enemies
        foreach (var current in DetectedAllies)
        {
            // Get distance to the enemy
            var distance = Vector3.Distance(transform.position, current.transform.position);

            // First check the enemy is in the detection range and is the closest distance found until now
            if (distance <= _detectionRange && distance < currentMinDistance)
                closestAlly = current.gameObject;
        }

        return closestAlly;
    }

    /// <summary>
    /// Returns closest enemy game object of a specific faction.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject ClosestFactionType(Faction type)
    {
        GameObject closestEnemy = null;
        const float currentMinDistance = Mathf.Infinity;

        // Analize all detected enemies
        foreach (var current in DetectedEnemies)
        {
            // Check if the current faction object belongs to the desired faction
            if (current.FactionName == type.FactionName)
            {
                // Get distance to the enemy
                var distance = Vector3.Distance(transform.position, current.transform.position);

                // First check the enemy is in the detection range and is the closest distance found until now
                if (distance <= _detectionRange && distance < currentMinDistance)
                    closestEnemy = current.gameObject;
            }
        }

        return closestEnemy;
    }

    /// <summary>
    /// Returns random enemy game object within detection range.
    /// </summary>
    /// <returns></returns>
    public GameObject RandomEnemy()
    {
        GameObject randomEnemy = null;

        // Analize all detected enemies
        var possibleEnemies = new List<Faction>();
        foreach (var current in DetectedEnemies)
        {
            // Get distance to the enemy
            var distance = Vector3.Distance(transform.position, current.transform.position);

            // First check the enemy is in the detection range and is the closest distance found until now
            if (distance <= _detectionRange)
                possibleEnemies.Add(current);
        }

        // Select random enemy
        if (possibleEnemies.Count > 0)
            randomEnemy = possibleEnemies[Random.Range(0, possibleEnemies.Count - 1)].gameObject;

        return randomEnemy;
    }

    /// <summary>
    /// Returns a random enemy game object of a specific faction, within detection range.    /// </summary>
    /// <returns></returns>
    public GameObject RandomFactionEnemy(Faction type)
    {
        GameObject randomEnemy = null;

        // Analize all detected enemies
        var possibleEnemies = new List<Faction>();
        foreach (var current in DetectedEnemies)
        {
            // Check if the current faction object belongs to the desired faction
            if (current.FactionName == type.FactionName)
            {
                // Get distance to the enemy
                var distance = Vector3.Distance(transform.position, current.transform.position);

                // First check the enemy is in the detection range and is the closest distance found until now
                if (distance <= _detectionRange)
                    possibleEnemies.Add(current);
            }
        }

        // Select random enemy
        if (possibleEnemies.Count > 0)
            randomEnemy = possibleEnemies[Random.Range(0, possibleEnemies.Count - 1)].gameObject;

        return randomEnemy;
    }

    /// <summary>
    /// Executed on the draw gizmos event
    /// </summary>
    private void OnDrawGizmos()
    {
        // Shows the detection range in editor
        if (_displayDetectionRange)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _detectionRange);
        }

        // Shows the attack range in editor
        if (_displayAttackRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }

        // Shows the avoid range in editor
        if (_displayAvoidRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _avoidRange);
        }
    }
}
