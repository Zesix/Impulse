using UnityEngine;
using System.Collections;

namespace Impulse
{
    /// <summary>
    /// Handles the transition between user interfaces, such as changing menu screens or HUD displays.
    /// </summary>
    public class InterfaceManager : MonoBehaviour
    {
        [SerializeField] private InterfaceScreen _currentActiveScreen;
        public bool InTransition { get; set; }

        /// <summary>
        /// Use this to request the transition to other scene with load screen and no user input (show scene once loaded).
        /// </summary>
        /// <param name="sceneId"></param>
        public void RequestSceneLoadWithLoadScreenNoUserInput(string sceneId)
        {
            if (InTransition)
                return;

            StartCoroutine(RequestSceneLoadWithLoadScreenEnumerator(sceneId, false));
        }
        
        /// <summary>
        /// Use this to request the transition to other scene with load screen and user input (prompt user input once scene is ready).
        /// </summary>
        /// <param name="sceneId"></param>
        public void RequestSceneLoadWithLoadScreenAndUserInput(string sceneId)
        {
            if (InTransition)
                return;

            StartCoroutine(RequestSceneLoadWithLoadScreenEnumerator(sceneId, true));
        }

        private IEnumerator RequestSceneLoadWithLoadScreenEnumerator(string sceneId, bool withUserInput)
        {
            InTransition = true;
			yield return SceneService.Instance.StartCoroutine(SceneService.Instance.LoadLevelLoadScreen(sceneId, withUserInput));
            InTransition = false;
        }

        /// <summary>
        /// Core transition method, use this to transition from one screen to another with fade
        /// </summary>
        public void ChangeInterfaceScreenWithFade(InterfaceScreen screen)
        {
            if (InTransition)
                return;

            StartCoroutine(ChangeInterfaceScreenEnumerator(screen, true));
        }

        /// <summary>
        /// Core transition method, use this to transition from one screen to another
        /// </summary>
        public void ChangeInterfaceScreenWithoutFade(InterfaceScreen screen)
        {
            if (InTransition)
                return;
            StartCoroutine(ChangeInterfaceScreenEnumerator(screen, false));
        }

        /// <summary>
        /// Request application exit
        /// </summary>
        public void QuitApplicationRequest()
        {
            Application.Quit();
        }

        /// <summary>
        /// Internal request for scene transition
        /// </summary>
        private IEnumerator ChangeInterfaceScreenEnumerator(InterfaceScreen target, bool animate)
        {
            // Execute fade out animation
            if (animate)
            {
                SceneService.Instance.SetCanvasEnabled(true);
                yield return StartCoroutine(
                    SceneService.Instance.PlayFadeAnimation(0f, 1f, SceneService.Instance.BlackOverlay));
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
                yield return StartCoroutine(
                    SceneService.Instance.PlayFadeAnimation(1f, 0f, SceneService.Instance.BlackOverlay));
                SceneService.Instance.SetCanvasEnabled(false);
            }
        }
    }
}
