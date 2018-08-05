using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Handles the transition between interfaces
/// </summary>
public class InterfaceManager : MonoBehaviour
{
    [SerializeField]
    private InterfaceScreen _currentActiveScreen;

    public bool InTransition { get; } = false;

    #region Transition Requests

    /// <summary>
    /// Use this to request the transition to other scene
    /// </summary>
    /// <param name="sceneId"></param>
    public void RequestSceneLoad(string sceneId)
    {
        SceneService.Instance.LoadLevelLoadScreen(sceneId, false);
    }

    /// <summary>
    /// Core transition method, use this to transition from one screen to another with fade
    /// </summary>
    public void ChangeScreenWithFade(InterfaceScreen screen)
    {
        StartCoroutine(ChangeScreenRoutine(screen, true));
    }

    /// <summary>
    /// Core transition method, use this to transition from one screen to another
    /// </summary>
    public void ChangeScreen(InterfaceScreen screen)
    {
        StartCoroutine(ChangeScreenRoutine(screen, false));
    }
    /// <summary>
    /// Request application exit
    /// </summary>
    public void QuitRequest()
    {
        Application.Quit();
    }
    #endregion

    /// <summary>
    /// Internal request for scene transition
    /// </summary>
    private IEnumerator ChangeScreenRoutine(InterfaceScreen target, bool animate)
    {
        // Execute fade out animation
        if (animate)
        {
            SceneService.Instance.SetCanvasEnabled(true);
            yield return StartCoroutine(SceneService.Instance.PlayFadeAnimation(0f, 1f, SceneService.Instance.BlackOverlay));
        }

        // Deactivate previous screen 
        if (_currentActiveScreen != null)
        {
            _currentActiveScreen.gameObject.SetActive(false);
        }

        // Activate next screen
        _currentActiveScreen = target;
        _currentActiveScreen.gameObject.SetActive(true);

        // Execute fade in animation
        if (animate)
        {
            yield return StartCoroutine(SceneService.Instance.PlayFadeAnimation(1f, 0f, SceneService.Instance.BlackOverlay));
            SceneService.Instance.SetCanvasEnabled(false);
        }
    }
}

