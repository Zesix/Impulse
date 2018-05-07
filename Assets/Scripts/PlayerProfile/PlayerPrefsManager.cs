using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// 	Player persistant data manager
/// </summary>
/// <remarks>
/// 	This should be loaded as part of the MainSystem factory. 
/// </remarks>
public class PlayerPrefsManager : MonoBehaviour,ILocalDataManager
{
    private const string _playerProfileKey = "LocalPlayerData";

    /// <summary>
    /// Load player profile to current data structures
    /// </summary>
    public PlayerProfile LoadProfile()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerProfile));
        string text = PlayerPrefs.GetString(_playerProfileKey);

        if (text.Length == 0)
        {
            return new PlayerProfile();
        }
        else
        {
            using (StringReader reader = new StringReader(text))
            {
                return serializer.Deserialize(reader) as PlayerProfile;
            }
        }
    }

    /// <summary>
    /// Save in persitant system the current values of the player profile data
    /// </summary>
    public void SaveProfile(PlayerProfile playerProfile)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerProfile));
        using (StringWriter sw = new StringWriter())
        {
            serializer.Serialize(sw,playerProfile);
            PlayerPrefs.SetString(_playerProfileKey, sw.ToString());
        }
    }

    /// <summary>
    /// Requests de profile data reset
    /// </summary>
    public void ResetProfile()
    {
        PlayerPrefs.DeleteAll();
    }

}
