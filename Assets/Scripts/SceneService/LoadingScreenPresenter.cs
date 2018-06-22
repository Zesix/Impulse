using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenPresenter : MonoBehaviour {
    // Presentation Loading Screen Presenter
    [SerializeField]
    private Image _progressFillImage;
    [SerializeField]
    private Text _progressText;

    [SerializeField]
    private GameObject _pressAnyKeyObj;

    // Interactivity parameters
    public bool RequiresUserInput = false;
    public float TimeAfterCompletionDelay = 0.5f;

    public void Initialize()
    {
        if (_pressAnyKeyObj != null)
        {
            _pressAnyKeyObj.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Update the loading screen progress
    /// </summary>
    /// <param name="progress"></param>
    public void RefreshLoadingProgress(float progress)
    {
        if (_progressText != null)
            _progressText.text = (Mathf.RoundToInt(progress * 100))+"%";

        if (_progressFillImage != null)
            _progressFillImage.fillAmount = progress;
    }

    public IEnumerator WaitForCompletion()
    {
        // Wait for delay
        if (!RequiresUserInput)
        {
            yield return new WaitForSeconds(TimeAfterCompletionDelay);
        }
        // Wait for user input
        else
        {
            if (_pressAnyKeyObj != null)
            {
                _pressAnyKeyObj.gameObject.SetActive(true);
            }

            bool done = false;
            while (!done) // essentially a "while true", but with a bool to break out naturally
            {
                if (Input.anyKey)
                {
                    done = true; // breaks the loop
                }
                yield return null; // wait until next frame, then continue execution from here (loop continues)
            }
        }
    }
}
