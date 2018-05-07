using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that contains the player information to be stored
/// </summary>
public class PlayerProfile {

    // Player parameters
    private string _playerID;
    public string PlayerID
    {
        get
        {
            return _playerID;
        }
        set
        {
            _playerID = value;
        }
    }
}
