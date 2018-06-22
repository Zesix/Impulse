using UnityEngine;

/// <summary>
/// 	Calls the SceneManager to change scene.
/// </summary>
/// <remarks>
/// 	Attach to a game object in a scene in order to use UGUI OnClick() for changing scenes in the SceneManager.
/// </remarks>
public class ChangeScene : MonoBehaviour
{
    public void LoadScene(int index, bool animate,bool useLoadingScreen, bool usePlayerInput)
    {
        SceneService.Instance.LoadLevelFadeInDelegate(index, animate, useLoadingScreen, usePlayerInput);
    }

    public void LoadScene(string sceneName, bool animate, bool useLoadingScreen, bool usePlayerInput)
    {
        SceneService.Instance.LoadLevelFadeInDelegate(sceneName,animate, useLoadingScreen, usePlayerInput);
    }

}
