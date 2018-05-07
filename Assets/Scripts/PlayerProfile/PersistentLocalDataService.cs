using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Persitance data service provider
/// </summary>
public class PersistentLocalDataService  {

    public PlayerProfile LocalPlayerData;

    private ILocalDataManager _dataManager;

    public void Initialize(ILocalDataManager dataManager)
    {
        _dataManager = dataManager;

        // Load initial profile
        LoadProfile();
    }

    /// <summary>
    /// Requests de profile data reset
    /// </summary>
    public void ResetProfile()
    {
        _dataManager.ResetProfile();

        LocalPlayerData = new PlayerProfile();
    }

    /// <summary>
    /// Save in persitant system the current values of the player profile data
    /// </summary>
    public void SaveProfile()
    {
        _dataManager.SaveProfile(LocalPlayerData);
    }
    
    /// <summary>
    /// Load player profile to current data structures
    /// </summary>
    public void LoadProfile()
    {
        LocalPlayerData = _dataManager.LoadProfile();
    }
}
