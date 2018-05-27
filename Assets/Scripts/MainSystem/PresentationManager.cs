using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the presentation fo the game
/// </summary>
public class PresentationManager : Singleton<PresentationManager> {

    public void Initialize()
    {
        // Set initial value
        SetResolution(SaveManager.Instance.GetLocalData().Resolution);
    }

    /// <summary>
    /// Set game presentation
    /// </summary>
    public void SetResolution(string resolutionString)
    {
        string[] words = resolutionString.Split('x');
        int width = int.Parse( words[0]);
        int height = int.Parse(words[1]);

        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}
