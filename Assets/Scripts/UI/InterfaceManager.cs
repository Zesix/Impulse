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

    public bool InTransition { get; private set; } = false;

    #region Transition Requests

    IEnumerator LoadLevel(string sceneId)
    {
        InTransition = true;
        yield  return StartCoroutine(SceneService.Instance.LoadLevelLoadScreen(sceneId, false));
        InTransition = false;
    }

    IEnumerator LoadScreen(InterfaceScreen screen, bool fade)
    {
        InTransition = true;
        yield return StartCoroutine(ChangeScreenRoutine(screen, fade));
        InTransition = false;
    }

    /// <summary>
    /// Use this to request the transition to other scene
    /// </summary>
    /// <param name="sceneId"></param>
    public void RequestSceneLoad(string sceneId)
    {
        if (InTransition)
            return;

        StartCoroutine(LoadLevel(sceneId));
    }

    /// <summary>
    /// Core transition method, use this to transition from one screen to another with fade
    /// </summary>
    public void ChangeScreenWithFade(InterfaceScreen screen)
    {
        if (InTransition)
            return;

        StartCoroutine(ChangeScreenRoutine(screen, true));
    }

    /// <summary>
    /// Core transition method, use this to transition from one screen to another
    /// </summary>
    public void ChangeScreen(InterfaceScreen screen)
    {
        if (InTransition)
            return;
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

