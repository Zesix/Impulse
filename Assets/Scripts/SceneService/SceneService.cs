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
    private GameObject _loadProgressCanvas;
    [SerializeField]
    private Image _loadProgress;
    [SerializeField]
    private float _afterLoadscreenDelay = 0.5f;

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

                _loadProgressCanvas.gameObject.SetActive(false);
                DontDestroyOnLoad(SplashObj);
            }
            Instance.StartCoroutine(LoadNextLevelFadeIn());
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

    public IEnumerator LoadNextLevelFadeIn()
    {
        yield return StartCoroutine(LoadLevelFadeIn(SceneManager.GetActiveScene().buildIndex + 1, true));
    }

    public IEnumerator LoadNextLevelWithLoadingScreen()
    {
        yield return StartCoroutine(LoadLevelLoadScreen(SceneManager.GetActiveScene().buildIndex + 1));
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

    public void LoadLevelFadeInDelegate(int index, bool animate = true)
    {
        if (animate)
            Instance.StartCoroutine(LoadLevelFadeIn(index));
        else
            Instance.StartCoroutine(LoadLevelLoadScreen(index));
    }

    public void LoadLevelFadeInDelegate(string sceneName, bool animate = true)
    {
        if (animate)
            Instance.StartCoroutine(LoadLevelFadeIn(sceneName));
        else
            Instance.StartCoroutine(LoadLevelLoadScreen(sceneName));
    }

    public IEnumerator LoadLevelFadeIn(int index, bool showSplash = false)
    {
        SetCanvasEnabled(true);
        yield return Instance.StartCoroutine(PlayFadeAnimation(true, BlackOverlay));
        if (showSplash && _splashCanvasGroup != null)
            yield return Instance.StartCoroutine(PlayFadeAnimation(true, _splashCanvasGroup));
        var async = SceneManager.LoadSceneAsync(index);
        yield return async; // Wait for the async operation and animation to complete.
        if (showSplash && _splashCanvasGroup != null)
            yield return Instance.StartCoroutine(PlayFadeAnimation(false, _splashCanvasGroup)); // remove overlay
        yield return Instance.StartCoroutine(PlayFadeAnimation(false, BlackOverlay));
        SetCanvasEnabled(false);
    }

    public IEnumerator LoadLevelFadeIn(string sceneName)
    {
        SetCanvasEnabled(true);
        yield return Instance.StartCoroutine(PlayFadeAnimation(true, BlackOverlay));
        var async = SceneManager.LoadSceneAsync(sceneName);
        yield return async; // Wait for the async operation and animation to complete.
        yield return Instance.StartCoroutine(PlayFadeAnimation(false, BlackOverlay));
        SetCanvasEnabled(false);
    }

    public IEnumerator LoadLevelLoadScreen(int index)
    {
        _loadProgressCanvas.gameObject.SetActive(true);
        _loadProgress.fillAmount = 0;

        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(index);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            _loadProgress.fillAmount = ao.progress;

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }

        _loadProgress.fillAmount = 1;

        yield return new WaitForSeconds(_afterLoadscreenDelay);

        ao.allowSceneActivation = true;
        _loadProgressCanvas.gameObject.SetActive(false);
    }

    public IEnumerator LoadLevelLoadScreen(string sceneName)
    {
        _loadProgressCanvas.gameObject.SetActive(true);
        _loadProgress.fillAmount = 0;

        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            _loadProgress.fillAmount = ao.progress;

            if (ao.progress >= 1.0f)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }

        _loadProgress.fillAmount = 1;

        yield return new WaitForSeconds(_afterLoadscreenDelay);

        ao.allowSceneActivation = true;
        _loadProgressCanvas.gameObject.SetActive(false);
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
}
