using UnityEngine;

/// <summary>
/// Attach to anything that needs to have a faction specified.
/// </summary>
[System.Serializable]
public class Faction : MonoBehaviour
{
    // Once every faction is defined this should be replaced with an enum (much more efficient)
    [SerializeField] private Factions _factionName;
    public Factions FactionName => _factionName;

    // Modify this to add more factions
    public enum Factions
    {
        Players,
        Enemies
    }
}
