using UnityEngine;

/// <summary>
/// A basic model for gameObjects that comes with basic movement functionality.
/// </summary>
public class BaseObjectModel : MonoBehaviour
{

    // Our movement destination.
    [SerializeField] private Vector3 _destinationPosition;

    // The spped our object moves.
    [SerializeField]
    float _moveSpeed = 1.5f;

    protected virtual void Awake()
    {
        // Set destination position to current position on awake.
        _destinationPosition = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_destinationPosition != transform.position)
            MoveObject();
    }

    /// <summary>
    /// Moves the object to the destination position (acquired from the input controller) and also rotates them to look at the destination position.
    /// When the object reaches the destination position, stop movement.
    /// </summary>
    protected virtual void MoveObject()
    {
        transform.LookAt(_destinationPosition);
        transform.position = Vector3.MoveTowards(transform.position, _destinationPosition, _moveSpeed * Time.deltaTime);

        // If we are at the desired position, then stop moving.

        // Draw a debug line to show where we are moving.
        Debug.DrawLine(transform.position, _destinationPosition, Color.red);
    }

    // Sets our destination. Used by our input controller.
    public virtual void SetDestinationInput(Vector3 input)
    {
        _destinationPosition = input;
    }

    // Sets whether we are firing or not. Used by our input controller.
    public virtual void SetFireInput(bool input)
    {
    }
}
