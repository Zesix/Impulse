using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MouseInputController : BaseInputController
{
    // Reference to the model. Replace with your own specific model during implementation.
    BaseObjectModel _myModel;

    // Mouse position in world space, from screen to world point.
    Vector3 _mouse3DInput;

    [SerializeField]
    float _cameraDistance = 10.0f;

    // Where the mouse has clicked, in 3D coordinates.
    [SerializeField]
    Vector3 _clickPosition;

    void Awake()
    {
        _myModel = GetComponent<BaseObjectModel>();

        if (_myModel == null)
            Debug.LogError(gameObject.name + " is missing a View!");

        // By default, set our click position to our current position.
        _clickPosition = transform.position;
    }

    public override void CheckInput()
    {
        // Get mouse position
        _mouse3DInput = Camera.main.ScreenToWorldPoint(new Vector3(CrossPlatformInputManager.mousePosition.x, CrossPlatformInputManager.mousePosition.y,
                                                                        _cameraDistance));

        // get fire / action buttons
        Fire1 = CrossPlatformInputManager.GetButton("Fire1");

        // If we have clicked, get the world space position and send it to our view.
        // If you want to use something other than the fire button for movement, replace 'Fire1'.
        if (Fire1)
        {
            SetClickPosition();
            SendInput();
        }
    }

    void Update()
    {
        CheckInput();
    }

    /// <summary>
    /// Sets the click position.
    /// </summary>
    void SetClickPosition()
    {
        Plane clickPlane = new Plane(Vector3.up, transform.position);
        Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var rayPoint = 0f;

        if (clickPlane.Raycast(clickRay, out rayPoint))
            _clickPosition = clickRay.GetPoint(rayPoint);
    }

    protected virtual void SendInput()
    {
        _myModel.SetDestinationInput(_clickPosition);
        _myModel.SetFireInput(Fire1);
    }

}