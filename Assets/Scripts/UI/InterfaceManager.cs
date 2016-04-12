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

namespace Impulse
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections;

    public class InterfaceManager : MonoBehaviour
    {
        string InterfaceScreens;
        public InterfaceScreen GameplayUI;
        public InterfaceScreen GameOverUI;
        public InterfaceScreen ActiveScreen;

        bool inTransition = false;
        public bool InTransition
        {
            get { return inTransition; }
        }
        // Use this for initialization
        void Awake()
        {
#if UNITY_EDITOR
            if (GameSceneManager.Instance == null)
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
                GameSceneManager.Instance.SetCanvasEnabled(true);
                yield return StartCoroutine(GameSceneManager.Instance.PlayFadeAnimation(0f, 1f, GameSceneManager.Instance.blackOverlay));
            }
            if (ActiveScreen != null)
            {
                ActiveScreen.gameObject.SetActive(false);
            }
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

}
