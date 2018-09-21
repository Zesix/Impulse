using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Impulse
{
    /// <summary>
    /// 	PlayerPrefs-based profile service for storing local player settings.
    /// </summary>
    /// <remarks>
    /// 	This should be loaded as part of the MainSystem factory. 
    /// </remarks>
    public class PlayerPrefsService : MonoBehaviour, ILocalPlayerProfileService
    {
        private const string PlayerProfileKey = "LocalPlayerProfile";

        /// <inheritdoc />
        public PlayerProfile LoadProfile()
        {
            var serializer = new XmlSerializer(typeof(PlayerProfile));
            var text = PlayerPrefs.GetString(PlayerProfileKey);

            if (text.Length == 0)
            {
                return new PlayerProfile();
            }
            else
            {
                using (var reader = new StringReader(text))
                {
                    return serializer.Deserialize(reader) as PlayerProfile;
                }
            }
        }

        /// <inheritdoc />
        public void SaveProfile(PlayerProfile playerProfile)
        {
            var serializer = new XmlSerializer(typeof(PlayerProfile));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, playerProfile);
                PlayerPrefs.SetString(PlayerProfileKey, stringWriter.ToString());
            }
        }

        /// <inheritdoc />
        public void ResetProfile()
        {
            PlayerPrefs.DeleteAll();
        }

    }
}
