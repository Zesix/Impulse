namespace Impulse
{
    /// <summary>
    /// Service for CRUD operations for local player profile data.
    /// </summary>
    public class PersistentLocalProfileService
    {
        public PlayerProfile LocalPlayerProfile;

        private ILocalPlayerProfileService _playerProfileService;

        public void Initialize(ILocalPlayerProfileService playerProfileService)
        {
            _playerProfileService = playerProfileService;

            // Load initial profile
            LoadProfile();
        }

        /// <summary>
        /// Requests the profile data to be reset.
        /// </summary>
        public void ResetProfile()
        {
            _playerProfileService.ResetProfile();

            LocalPlayerProfile = new PlayerProfile();
        }

        /// <summary>
        /// Save the current values of the player profile data.
        /// </summary>
        public void SaveProfile()
        {
            _playerProfileService.SaveProfile(LocalPlayerProfile);
        }

        /// <summary>
        /// Load player profile.
        /// </summary>
        public void LoadProfile()
        {
            LocalPlayerProfile = _playerProfileService.LoadProfile();
        }
    }
}
