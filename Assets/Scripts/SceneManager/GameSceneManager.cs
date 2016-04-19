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


using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 	Scene manager.
/// </summary>
/// <remarks>
/// 	This should be loaded as part of the MainSystem factory. 
/// 	Call this object to change scene.
/// 	If you want to display a splash screen, set the image in the ImageToFade child of the SplashFadeIn prefab.
/// </remarks>
public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;		// Singleton

    public string[] levelNames;                 // Scenes to progressively load, in descending array order.
    public int gameLevelNum = 0;
    public GameObject splashObj;           		// Put a canvas here (with image child object) for splash screen loading.
    public AnimationCurve Interpolation;		// Interpolates the splash screen with the curve.
    public float initialSplashDuration = 3f;    // The duration of the initial splash screen overlay.
    public float duration = 2f;					// The duration of the animation in seconds.
    private CanvasGroup splashCanvasGroup;		// Used to grab the alpha of the canvas group.
    public bool inTransition;                   // Are we in the middle of a fade transition?
    public float transitionPercent;             // How far are we in a transition?

    public CanvasGroup blackOverlay;			//Canvas for black overlay

    private GameObject canvasObj;				// The canvas game object.

#if UNITY_EDITOR
    bool SkipSplash;
    string FirstScene;
#endif

    void Start()
    {
        // If there is no instance of this class, set it.
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject); // Don't destroy this object
            Instance = this;
            if (splashObj != null)
            {
                canvasObj = (GameObject)Instantiate(splashObj);
                canvasObj.transform.SetParent(transform);
                canvasObj.transform.position = Vector3.zero;
                splashCanvasGroup = canvasObj.GetComponent<CanvasGroup>();
                canvasObj.GetComponent<Canvas>().sortingOrder = 999;
                DontDestroyOnLoad(splashObj);
            }
            blackOverlay.GetComponent<Canvas>().sortingOrder = 998;
            Instance.StartCoroutine(LoadNextLevelFadeIn());
        }
        else
        {
            Debug.LogError("There is already a GameSceneManager in the scene.");
            GameObject.Destroy(this);
        }
    }

    public IEnumerator PlayFadeAnimation(float start, float end, CanvasGroup target)
    {
        // Start a lerp value from zero
        float lerpValue = 0f;
        inTransition = true;
        while (lerpValue <= 1f)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                //Increment the value each frame
                lerpValue += Time.deltaTime / initialSplashDuration;
                transitionPercent = lerpValue / initialSplashDuration;
            }
            else
            {
                //Increment the value each frame
                lerpValue += Time.deltaTime / duration;
                transitionPercent = lerpValue / duration;
            }
            //Set the alpha by the interpolated lerp value
            target.alpha = Mathf.Lerp(start, end, Interpolation.Evaluate(lerpValue));
            yield return null;
        }
        inTransition = false;
    }

    private IEnumerator PlayFadeAnimation(bool forward, CanvasGroup target)
    {
        if (forward)
        {
            yield return Instance.StartCoroutine(PlayFadeAnimation(0f, 1f, target));
        }
        else
        {
            yield return Instance.StartCoroutine(PlayFadeAnimation(1f, 0f, target));
        }
    }

    public IEnumerator LoadNextLevelFadeIn()
    {
        yield return StartCoroutine(LoadLevelFadeIn(+1, true));
    }

    public void SetCanvasEnabled(bool enabled)
    {
        blackOverlay.gameObject.SetActive(enabled);
        if (splashCanvasGroup != null)
        {
            canvasObj.gameObject.SetActive(enabled);
            splashCanvasGroup.gameObject.SetActive(enabled);
        }
    }

    public void LoadLevelFadeInDelegate(int index, bool animate = true)
    {
        if (animate)
            Instance.StartCoroutine(LoadLevelFadeIn(index));
        else
            SceneManager.LoadScene(index);
    }

    public void LoadLevelFadeInDelegate(string name, bool animate = true)
    {
        if (animate)
            Instance.StartCoroutine(LoadLevelFadeIn(name));
        else
            SceneManager.LoadScene(name);
    }

    public IEnumerator LoadLevelFadeIn(int index, bool showSplash = false)
    {
        SetCanvasEnabled(true);
        yield return Instance.StartCoroutine(PlayFadeAnimation(true, blackOverlay));
        if (showSplash && splashCanvasGroup != null)
            yield return Instance.StartCoroutine(PlayFadeAnimation(true, splashCanvasGroup));
        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        yield return async; // Wait for the async operation and animation to complete.
        if (showSplash && splashCanvasGroup != null)
            yield return Instance.StartCoroutine(PlayFadeAnimation(false, splashCanvasGroup)); // remove overlay
        yield return Instance.StartCoroutine(PlayFadeAnimation(false, blackOverlay));
        SetCanvasEnabled(false);
    }

    public IEnumerator LoadLevelFadeIn(string sceneName)
    {
        SetCanvasEnabled(true);
        yield return Instance.StartCoroutine(PlayFadeAnimation(true, blackOverlay));
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        yield return async; // Wait for the async operation and animation to complete.
        yield return Instance.StartCoroutine(PlayFadeAnimation(false, blackOverlay));
        SetCanvasEnabled(false);
    }

    public void ResetGame()
    {
        // reset the level index counter
        gameLevelNum = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
