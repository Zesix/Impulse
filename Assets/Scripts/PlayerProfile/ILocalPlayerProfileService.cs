namespace Impulse
{
    /// <summary>
    /// Interface for local profile management.
    /// </summary>
    public interface ILocalPlayerProfileService
    {
        /// <summary>
        /// Requests the profile data to be reset.
        /// </summary>
        void ResetProfile();

        /// <summary>
        /// Save the current values of the player profile data.
        /// </summary>
        void SaveProfile(PlayerProfile playerProfile);

        /// <summary>
        /// Load player profile.
        /// </summary>
        PlayerProfile LoadProfile();
    }
}
