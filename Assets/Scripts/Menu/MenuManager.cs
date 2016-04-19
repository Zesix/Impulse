/*****************************************
 * This file is part of Impulse Framework.

    Impulse Framework is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    Impulse Framework is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Impulse Framework.  If not, see <http://www.gnu.org/licenses/>.
*****************************************/

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
    string MenuScreens;
    public MenuScreen ActiveScreen;
    public MenuScreen FirstScreen;
    // Use this for initialization
    void Awake()
    {
#if UNITY_EDITOR
        if (GameSceneManager.Instance == null)
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
        GameSceneManager.Instance.LoadLevelFadeInDelegate(index, animate);
    }

    public void LoadScene(string name, bool animate)
    {
        GameSceneManager.Instance.LoadLevelFadeInDelegate(name);
    }

    public void LoadSceneFadeIn(string name)
    {
        GameSceneManager.Instance.LoadLevelFadeInDelegate(name);
    }

    public void LoadScene(string name)
    {
        GameSceneManager.Instance.LoadLevelFadeInDelegate(name, false);
    }

    IEnumerator ChangeScreen(MenuScreen target, bool animate)
    {
        if (animate)
        {
            GameSceneManager.Instance.SetCanvasEnabled(true);
            yield return StartCoroutine(GameSceneManager.Instance.PlayFadeAnimation(0f, 1f, GameSceneManager.Instance.blackOverlay));
        }
        ActiveScreen.gameObject.SetActive(false);
        ActiveScreen = target;
        ActiveScreen.gameObject.SetActive(true);
        if (animate)
        {
            yield return StartCoroutine(GameSceneManager.Instance.PlayFadeAnimation(1f, 0f, GameSceneManager.Instance.blackOverlay));
            GameSceneManager.Instance.SetCanvasEnabled(false);
        }
    }

    public void QuitRequest()
    {
        Application.Quit();
    }
}

