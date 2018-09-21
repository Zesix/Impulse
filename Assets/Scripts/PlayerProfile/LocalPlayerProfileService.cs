using Zenject;

namespace Impulse
{
    public class LocalPlayerProfileService : Singleton<LocalPlayerProfileService>
    {

        private PersistentLocalProfileService _localSaveService;
        [Inject] private ILocalPlayerProfileService _localPlayerProfileService;

        public void Initialize()
        {
            _localSaveService = new PersistentLocalProfileService();
            _localSaveService.Initialize(_localPlayerProfileService);
        }

        public PlayerProfile GetPlayerProfile()
        {
            return _localSaveService.LocalPlayerProfile;
        }

        public void SaveData()
        {
            _localSaveService.SaveProfile();
        }

        public void ResetData()
        {
            _localSaveService.ResetProfile();
        }

        public void LoadData()
        {
            _localSaveService.LoadProfile();
        }

        public PlayerProfile GetLocalData()
        {
            if (_localSaveService.LocalPlayerProfile == null)
                return new PlayerProfile();

            return _localSaveService.LocalPlayerProfile;
        }
    }
}
