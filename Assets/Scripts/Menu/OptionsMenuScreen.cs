using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScreen : MenuScreen
{

    [SerializeField]
    protected Dropdown _dropdownSystem;

    [SerializeField]
    protected List<string> _resolutionOptions = new List<string>()
    {
        "1024x768", "1280x800","1360x768","1366x768","1440x900","1600x900","1680x1050","1920x1080","2560x1080","2560x1440","2560x1600"
    };

    /// <summary>
    /// Use this for initialization
    /// </summary>
    protected override void Start()
    {
        _dropdownSystem.ClearOptions();
        _dropdownSystem.AddOptions(_resolutionOptions);

        // Load saved resolution
        string savedResolution = SaveManager.Instance.GetLocalData().Resolution;
        int indexOfSavedResolution = _resolutionOptions.IndexOf(savedResolution);
        _dropdownSystem.value = indexOfSavedResolution;
        _dropdownSystem.RefreshShownValue();
    }

    public void SetResolution()
    {
        // Notify resolution manager
        PresentationManager.Instance.SetResolution(_resolutionOptions[_dropdownSystem.value]);

        // Update player prefs
        string resolutionToSave = _resolutionOptions[_dropdownSystem.value];
        SaveManager.Instance.GetLocalData().Resolution = resolutionToSave;
        SaveManager.Instance.SaveData();
    }
}
