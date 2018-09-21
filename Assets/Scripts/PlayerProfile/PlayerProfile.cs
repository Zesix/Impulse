namespace Impulse
{
    /// <summary>
    /// Class that contains the player information to be stored
    /// </summary>
    public class PlayerProfile
    {

        // Player parameters
        private string _playerId;

        public string PlayerId
        {
            get { return _playerId; }
            set { _playerId = value; }
        }

        // Presentation parameters
        private string _resolution;

        public string Resolution
        {
            get { return _resolution; }
            set { _resolution = value; }
        }

        // Default constructor
        public PlayerProfile()
        {
            _playerId = "NotDefined";
            _resolution = "1024x768";
        }
    }
}
