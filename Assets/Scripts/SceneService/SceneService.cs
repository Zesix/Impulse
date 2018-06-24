using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 	Scene manager.
/// </summary>
/// <remarks>
/// 	This should be loaded as part of the MainSystem factory. 
/// 	Call this object to change scene.
/// 	If you want to display a splash screen, set the image in the ImageToFade child of the SplashFadeIn prefab.
/// </remarks>
public class SceneService : MonoBehaviour
{
    public static SceneService Instance;		// Singleton

    public string[] LevelNames;                 // Scenes to progressively load, in descending array order.
    public int GameLevelNum;
    public GameObject SplashObj;           		// Put a canvas here (with image child object) for splash screen loading.
    public AnimationCurve Interpolation;		// Interpolates the splash screen with the curve.
    public float InitialSplashDuration = 3f;    // The duration of the initial splash screen overlay.
    public float Duration = 2f;					// The duration of the animation in seconds.
    private CanvasGroup _splashCanvasGroup;		// Used to grab the alpha of the canvas group.
    public bool InTransition;                   // Are we in the middle of a fade transition?
    public float TransitionPercent;             // How far are we in a transition?


    public CanvasGroup BlackOverlay;			//Canvas for black overlay

    private GameObject _canvasObj;              // The canvas game object.


    [SerializeField]
    private GameObject _loadScreenParent;

    [SerializeField]
    private LoadingSceneConfig _loadScreenConfig;
    private LoadingScreenPresenter _instancedLoadScreen;
    
#if UNITY_EDITOR
    private bool _skipSplash;
    private string _firstScene;
#endif

    private void Start()
    {
        // If there is no instance of this class, set it.
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject); // Don't destroy this object
            Instance = this;
            if (SplashObj != null)
            {
                _canvasObj = Instantiate(SplashObj);
                _canvasObj.transform.SetParent(transform);
                _canvasObj.transform.position = Vector3.zero;
                _splashCanvasGroup = _canvasObj.GetComponentInChildren<CanvasGroup>();
                _canvasObj.GetComponent<Canvas>().sortingOrder = 999;

                DontDestroyOnLoad(SplashObj);
            }
            Instance.StartCoroutine(LoadNextLevelFadeIn(true,false));
        }
        else
        {
            Debug.LogError("There is already a SceneController in the scene.");
            Destroy(this);
        }
    }

    public IEnumerator PlayFadeAnimation(float start, float end, CanvasGroup target)
    {
        // Start a lerp value from zero
        var lerpValue = 0f;
        InTransition = true;
        while (lerpValue <= 1f)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                //Increment the value each frame
                lerpValue += Time.deltaTime / InitialSplashDuration;
                TransitionPercent = lerpValue / InitialSplashDuration;
            }
            else
            {
                //Increment the value each frame
                lerpValue += Time.deltaTime / Duration;
                TransitionPercent = lerpValue / Duration;
            }
            //Set the alpha by the interpolated lerp value
            target.alpha = Mathf.Lerp(start, end, lerpValue);
            yield return null;
        }
        InTransition = false;
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

    public IEnumerator LoadNextLevelFadeIn(bool useLoadingScreen = true, bool usePlayerInput = false)
    {
        yield return StartCoroutine(LoadLevelFadeIn(SceneManager.GetActiveScene().buildIndex + 1, true, useLoadingScreen, usePlayerInput));
    }

    public void SetCanvasEnabled(bool isCanvasActive)
    {
        BlackOverlay.gameObject.SetActive(isCanvasActive);
        if (_splashCanvasGroup != null)
        {
            _canvasObj.gameObject.SetActive(isCanvasActive);
            _splashCanvasGroup.gameObject.SetActive(isCanvasActive);
        }
    }

    public void LoadLevelFadeInDelegate(int index, bool animate = true,bool useLoadingScreen = true, bool usePlayerInput = false)
    {
        if (animate)
            Instance.StartCoroutine(LoadLevelFadeIn(index,false, useLoadingScreen, usePlayerInput));
        else
            Instance.StartCoroutine(LoadLevelLoadScreen(index,usePlayerInput));
    }

    public void LoadLevelFadeInDelegate(string sceneName, bool animate = true,bool useLoadingScreen = true, bool usePlayerInput = false)
    {
        if (animate)
            Instance.StartCoroutine(LoadLevelFadeIn(sceneName,false, useLoadingScreen,usePlayerInput));
        else
            Instance.StartCoroutine(LoadLevelLoadScreen(sceneName, usePlayerInput));
    }


    public IEnumerator LoadLevelLoadScreen(int index,bool usePlayerInput)
    {
        _instancedLoadScreen = RequestLoadingScreen(usePlayerInput);

        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(index);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            _instancedLoadScreen.RefreshLoadingProgress(ao.progress);

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }

        _instancedLoadScreen.RefreshLoadingProgress(1);

        yield return StartCoroutine(_instancedLoadScreen.WaitForCompletion());

        ao.allowSceneActivation = true;
        RemoveLoadingScreen();
    }

    public IEnumerator LoadLevelLoadScreen(string sceneName, bool usePlayerInput)
    {
        _instancedLoadScreen = RequestLoadingScreen(usePlayerInput);

        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            _instancedLoadScreen.RefreshLoadingProgress( ao.progress);

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }

        _instancedLoadScreen.RefreshLoadingProgress(1);

        yield return StartCoroutine(_instancedLoadScreen.WaitForCompletion());

        ao.allowSceneActivation = true;
        RemoveLoadingScreen();
    }

    public void ResetGame()
    {
        // reset the level index counter
        GameLevelNum = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region LoadingScreen Manangement
    
    private LoadingScreenPresenter RequestLoadingScreen(bool requirePlayerInput)
    {
        LoadingScreenPresenter loadingScreenPrefab = _loadScreenConfig.GetLoadScreen(requirePlayerInput);

        LoadingScreenPresenter instance = Instantiate(loadingScreenPrefab);
        instance.transform.parent = _loadScreenParent.transform;
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localScale = Vector3.one;

        instance.Initialize();

        return instance;
    }
    
    private void RefreshLoadingScreen(float progress)
    {
        _instancedLoadScreen.RefreshLoadingProgress(progress);
    }
    
    private void RemoveLoadingScreen()
    {
        Destroy(_instancedLoadScreen.gameObject);
    }
    #endregion

    #region Core Transition
    public IEnumerator LoadLevelFadeIn(int index, bool showSplash = false, bool useLoadingScreen = true, bool usePlayerInput = false)
    {
        SetCanvasEnabled(true);
        yield return Instance.StartCoroutine(PlayFadeAnimation(true, BlackOverlay));

        // Transition with splash screen
        if (showSplash && _splashCanvasGroup != null)
        {
            yield return Instance.StartCoroutine(PlayFadeAnimation(true, _splashCanvasGroup));
            // Transition without loading screen
            if (!useLoadingScreen)
            {
                var async = SceneManager.LoadSceneAsync(index);
                yield return async; // Wait for the async operation and animation to complete.

                yield return Instance.StartCoroutine(PlayFadeAnimation(false, _splashCanvasGroup)); // remove overlay
            }
            // Transition with loading screen
            else
            {
                yield return Instance.StartCoroutine(PlayFadeAnimation(false, _splashCanvasGroup)); // remove overlay

                yield return LoadLevelLoadScreen(index, usePlayerInput);
            }
        }
        // Transition without splash screen
        else
        {
            yield return LoadLevelLoadScreen(index, usePlayerInput);
        }

        yield return Instance.StartCoroutine(PlayFadeAnimation(false, BlackOverlay));
        SetCanvasEnabled(false);
    }

    public IEnumerator LoadLevelFadeIn(string sceneId, bool showSplash = false, bool useLoadingScreen = true, bool usePlayerInput = false)
    {
        SetCanvasEnabled(true);
        yield return Instance.StartCoroutine(PlayFadeAnimation(true, BlackOverlay));

        // Transition with splash screen
        if (showSplash && _splashCanvasGroup != null)
        {
            yield return Instance.StartCoroutine(PlayFadeAnimation(true, _splashCanvasGroup));
            // Transition without loading screen
            if (!useLoadingScreen)
            {
                var async = SceneManager.LoadSceneAsync(sceneId);
                yield return async; // Wait for the async operation and animation to complete.

                yield return Instance.StartCoroutine(PlayFadeAnimation(false, _splashCanvasGroup)); // remove overlay
            }
            // Transition with loading screen
            else
            {
                yield return Instance.StartCoroutine(PlayFadeAnimation(false, _splashCanvasGroup)); // remove overlay

                yield return LoadLevelLoadScreen(sceneId, usePlayerInput);
            }
        }
        // Transition without splash screen
        else
        {
            yield return LoadLevelLoadScreen(sceneId, usePlayerInput);
        }

        yield return Instance.StartCoroutine(PlayFadeAnimation(false, BlackOverlay));
        SetCanvasEnabled(false);
    }
    #endregion
}
