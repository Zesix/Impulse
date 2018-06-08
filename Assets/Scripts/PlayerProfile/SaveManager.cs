using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class SaveManager : Singleton<SaveManager> {

    private PersistentLocalDataService _localSaveService;
    [Inject]
    private ILocalDataManager _dataManager;

    public void Initialize()
    {
        _localSaveService = new PersistentLocalDataService();
        _localSaveService.Initialize(_dataManager);
    }

    public PlayerProfile GetPlayerProfile()
    {
        return _localSaveService.LocalPlayerData;
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
        if (_localSaveService.LocalPlayerData == null)
            return new PlayerProfile();

        return _localSaveService.LocalPlayerData;
    }
}
