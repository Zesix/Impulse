using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InterfaceManager : MonoBehaviour
{
    private string _interfaceScreens;
    public InterfaceScreen GameplayUi;
    public InterfaceScreen GameOverUi;
    public InterfaceScreen ActiveScreen;

    public bool InTransition { get; } = false;

    // Use this for initialization
    private void Awake()
    {
#if UNITY_EDITOR
        if (SceneService.Instance == null)
            SceneManager.LoadScene(0);
#endif
    }

    public void ChangeScreenAndFade(InterfaceScreen screen)
    {
        StartCoroutine(ChangeScreen(screen, true));
    }

    public void ChangeScreen(InterfaceScreen screen)
    {
        StartCoroutine(ChangeScreen(screen, false));
    }

    public void LoadScene(int index, bool animate)
    {
        SceneService.Instance.LoadLevelFadeInDelegate(index, animate);
    }

    public void LoadScene(string sceneName, bool animate)
    {
        SceneService.Instance.LoadLevelFadeInDelegate(sceneName);
    }

    public void LoadSceneFadeIn(string sceneName)
    {
        SceneService.Instance.LoadLevelFadeInDelegate(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneService.Instance.LoadLevelFadeInDelegate(sceneName, false);
    }

    public void ClearScreen()
    {
        if (ActiveScreen != null)
        {
            ActiveScreen.gameObject.SetActive(false);
        }
    }

    IEnumerator ChangeScreen(InterfaceScreen target, bool animate)
    {
        if (animate)
        {
            SceneService.Instance.SetCanvasEnabled(true);
            yield return StartCoroutine(SceneService.Instance.PlayFadeAnimation(0f, 1f, SceneService.Instance.BlackOverlay));
        }
        if (ActiveScreen != null)
        {
            ActiveScreen.gameObject.SetActive(false);
        }
        ActiveScreen = target;
        ActiveScreen.gameObject.SetActive(true);
        if (animate)
        {
            yield return StartCoroutine(SceneService.Instance.PlayFadeAnimation(1f, 0f, SceneService.Instance.BlackOverlay));
            SceneService.Instance.SetCanvasEnabled(false);
        }
    }

    public void QuitRequest()
    {
        Application.Quit();
    }
}

