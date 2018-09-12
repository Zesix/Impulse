using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LoadingSceneConfig : ScriptableObject {
    public LoadingScreenPresenter[] PossibleLoadingScreens;

    public LoadingScreenPresenter GetLoadScreen(bool usePlayerInput)
    {
        var possible = GetPossibleScreens(usePlayerInput);

        return possible[Random.Range(0, possible.Length - 1)];
    }

    private LoadingScreenPresenter[] GetPossibleScreens(bool usePlayerInput)
    {
        var possibleScreens = new List<LoadingScreenPresenter>();

        foreach(var loadScreen in PossibleLoadingScreens)
        {
            if (loadScreen.RequiresUserInput == usePlayerInput)
                possibleScreens.Add(loadScreen);
        }

        return possibleScreens.ToArray();
    }
}
