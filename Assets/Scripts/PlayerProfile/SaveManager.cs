using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class SaveManager : Singleton<SaveManager> {

    private PersistentLocalDataService _localSaveService;
    private ILocalDataManager _dataManager;

    private void Start()
    {
        _dataManager = GetComponentInChildren<ILocalDataManager>(true);

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
}
