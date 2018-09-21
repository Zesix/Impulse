using UnityEngine;

namespace Impulse
{
    /// <summary>
    /// Manages the presentation for the game
    /// </summary>
    public class PresentationService : Singleton<PresentationService>
    {
        public void Initialize()
        {
            SetResolution(LocalPlayerProfileService.Instance.GetLocalData().Resolution);
        }

        /// <summary>
        /// Set display resolution.
        /// </summary>
        public void SetResolution(string resolutionString)
        {
            var words = resolutionString.Split('x');
            var width = int.Parse(words[0]);
            var height = int.Parse(words[1]);

            Screen.SetResolution(width, height, Screen.fullScreen);
        }
    }
}
