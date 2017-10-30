/* 
    READ ME 

    How to use:
        Create a root object with this script
        Make child objects under this script and assign the MenuScreen.cs on those objects
        On button OnClick event call ChangeMenu on this script

    You can start the game on any scene with a Menu Manager

    Assign a Menu Screen object to First Screen to start with that screen

    Disable all gameObjects with a MenuScreen component before running.

    */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    private string _menuScreens;
    public MenuScreen ActiveScreen;
    public MenuScreen FirstScreen;
    // Use this for initialization
    private void Awake()
    {
#if UNITY_EDITOR
        if (SceneController.Instance == null)
            SceneManager.LoadScene(0);
#endif
        FirstScreen.gameObject.SetActive(true);
        ActiveScreen = FirstScreen;
    }

    public void ChangeMenuAndFade(MenuScreen screen)
    {
        StartCoroutine(ChangeScreen(screen, true));
    }

    public void ChangeMenu(MenuScreen screen)
    {
        StartCoroutine(ChangeScreen(screen, false));
    }

    public void LoadScene(int index, bool animate)
    {
        SceneController.Instance.LoadLevelFadeInDelegate(index, animate);
    }

    public void LoadScene(string sceneName, bool animate)
    {
        SceneController.Instance.LoadLevelFadeInDelegate(sceneName);
    }

    public void LoadSceneFadeIn(string sceneName)
    {
        SceneController.Instance.LoadLevelFadeInDelegate(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneController.Instance.LoadLevelFadeInDelegate(sceneName, false);
    }

    private IEnumerator ChangeScreen(MenuScreen target, bool animate)
    {
        if (animate)
        {
            SceneController.Instance.SetCanvasEnabled(true);
            yield return StartCoroutine(SceneController.Instance.PlayFadeAnimation(0f, 1f, SceneController.Instance.BlackOverlay));
        }
        ActiveScreen.gameObject.SetActive(false);
        ActiveScreen = target;
        ActiveScreen.gameObject.SetActive(true);
        if (animate)
        {
            yield return StartCoroutine(SceneController.Instance.PlayFadeAnimation(1f, 0f, SceneController.Instance.BlackOverlay));
            SceneController.Instance.SetCanvasEnabled(false);
        }
    }

    public void QuitRequest()
    {
        Application.Quit();
    }
}

