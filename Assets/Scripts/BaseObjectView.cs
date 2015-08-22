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
using System.Collections;

/// <summary>
/// A basic view for gameObjects that comes with basic movement functionality.
/// </summary>
public class BaseObjectView : MonoBehaviour {

    // Our input controller.
    Mouse_Input myController;

    // Our movement destination.
    [SerializeField]
    Vector3 destinationPosition;
    
    // The spped our object moves.
    [SerializeField]
    float moveSpeed;

    bool fireInput = false;                 // Are we firing?
    bool isMoving = false;                  // Are we moving?

	virtual protected void Awake () {
        // Get our controller.
        myController = GetComponent<Mouse_Input>();
        if (myController == null)
            Debug.LogError(gameObject.name + " is missing a Controller!");

        // Set destination position to current position.
        destinationPosition = transform.position;

        // TODO This is normally acquired from the model. We use a placeholder value here for testing.
        moveSpeed = 1.5f;
	}
	
	// Update is called once per frame
	virtual protected void Update () {
        if (destinationPosition != transform.position)
            MoveObject();
	}

    /// <summary>
    /// Moves the object to the destination position (acquired from the input controller) and also rotates them to look at the destination position.
    /// When the object reaches the destination position, stop movement.
    /// </summary>
    virtual protected void MoveObject ()
    {
        transform.LookAt(destinationPosition);
        transform.position = Vector3.MoveTowards(transform.position, destinationPosition, moveSpeed * Time.deltaTime);

        // If we are at the desired position, then stop moving.
        if (transform.position == destinationPosition)
            isMoving = false;

        // Draw a debug line to show where we are moving.
        Debug.DrawLine(transform.position, destinationPosition, Color.red);
    }

    // Sets our destination. Used by our input controller.
    virtual public void setDestinationInput(Vector3 input)
    {
        destinationPosition = input;
    }

    // Sets whether we are firing or not. Used by our input controller.
    virtual public void setFireInput(bool input)
    {
        fireInput = input;
    }
}
