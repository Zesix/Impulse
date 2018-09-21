using System.Collections.Generic;
using Impulse;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScreen : InterfaceScreen
{
    [SerializeField]
    protected Dropdown DropdownSystem;

    [SerializeField]
    protected List<string> ResolutionOptions = new List<string>
    {
        "1024x768", "1280x800","1360x768","1366x768","1440x900","1600x900","1680x1050","1920x1080","2560x1080","2560x1440","2560x1600"
    };

    protected override void Start()
    {
        DropdownSystem.ClearOptions();
        DropdownSystem.AddOptions(ResolutionOptions);

        // Load saved resolution
        var savedResolution = LocalPlayerProfileService.Instance.GetLocalData().Resolution;
        var indexOfSavedResolution = ResolutionOptions.IndexOf(savedResolution);
        DropdownSystem.value = indexOfSavedResolution;
        DropdownSystem.RefreshShownValue();
    }

    public void SetResolution()
    {
        PresentationService.Instance.SetResolution(ResolutionOptions[DropdownSystem.value]);

        // Update player profile settings.
        var resolutionToSave = ResolutionOptions[DropdownSystem.value];
        LocalPlayerProfileService.Instance.GetLocalData().Resolution = resolutionToSave;
        LocalPlayerProfileService.Instance.SaveData();
    }
}
