using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for data management
/// </summary>
public interface ILocalDataManager {

    /// <summary>
    /// Requests de profile data reset
    /// </summary>
    void ResetProfile();

    /// <summary>
    /// Save in persitant system the current values of the player profile data
    /// </summary>
    void SaveProfile(PlayerProfile playerProfile);

    /// <summary>
    /// Load player profile to current data structures
    /// </summary>
    PlayerProfile LoadProfile();
}
