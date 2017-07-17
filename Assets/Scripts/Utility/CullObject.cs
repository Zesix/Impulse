using UnityEngine;

/// <summary>
/// Shows or hides an object's mesh renderer based on whether the culling target is within the Sphere Collider or not.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class CullObject : MonoBehaviour
{
    // The object we are comparing distance to, for culling. 
    public Collider CullingTarget;

    // Our renderer.
    private Renderer _myRenderer;

    // Our Sphere Collider
    private SphereCollider _myCullingCollider;

    private void Start ()
    {
        _myCullingCollider = GetComponent<SphereCollider>();
        _myCullingCollider.isTrigger = true;
        _myRenderer = transform.parent.GetComponent<Renderer>();
        _myRenderer.enabled = false;

        if (CullingTarget == null)
            Debug.LogWarning("No culling target specified for " + gameObject + "!");
    }

    // When the culling target is within the myCollider, we render the object.
    private void OnTriggerEnter(Collider myCollider)
    {
        if (myCollider == CullingTarget)
        {
            _myRenderer.enabled = true;
        }
    }

    // When the culling target leaves the myCollider, we hide the object.
    private void OnTriggerExit(Collider myCollider)
    {
        if (myCollider == CullingTarget)
        {
            _myRenderer.enabled = false;
        }
    }
}
