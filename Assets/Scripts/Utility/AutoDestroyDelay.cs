using UnityEngine;

/// <summary>
/// Destroys a gameObject after a set delay.
/// </summary>
public class AutoDestroyDelay : MonoBehaviour
{
    [SerializeField] private float _delayTime = 1.5f;

    // Update is called once per frame
    private void Update()
    {
        Destroy(gameObject, _delayTime);
    }
}
