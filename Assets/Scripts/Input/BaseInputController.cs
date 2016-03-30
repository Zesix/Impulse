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
using System;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class BaseInputController : MonoBehaviour
{
    // firing buttons
    public bool Fire1;
    public bool Fire2;

    // Horizontal and vertical movement.
    [SerializeField]
    protected float vertical;
    public float Vertical
    {
        get { return vertical; }
        set { vertical = value; }
    }
    [SerializeField]
    protected float horizontal;
    public float Horizontal
    {
        get { return horizontal; }
        set { horizontal = value; }
    }

    Vector3 TEMPVec3;

    // A string array containing our firing buttons.
    string[] _fires = new string[]
        {"Fire1",
        "Fire2"};

    // Initialize Repeaters for each axis.
    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");


    // Whenever a Repeater reports movement input, we will share it as a static event.
    public static event EventHandler<InfoEventArgs<MovementDirs>> moveEvent;

    // Whenever a Repeater reports firing input, we will pass it along.
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    // Used for realtime input.
    public virtual void CheckInput()
    {
        horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");
    }

    public virtual float GetHorizontal()
    {
        return horizontal;
    }

    public virtual float GetVertical()
    {
        return vertical;
    }

    public virtual Vector3 GetMovementDirectionVector()
    {
        // temp vector for movement dir gets set to the value of an otherwise unused vector that always have the value of 0,0,0
        TEMPVec3 = Vector3.zero;

        TEMPVec3.x = horizontal;
        TEMPVec3.y = vertical;

        // return the movement vector
        return TEMPVec3;
    }

    // For nonrealtime games, handle inpute via events within the main loop for performance.
    void Update()
    {
        // Set axis variables to update according to the Repeater instead of general Update() loop.
        int x = _hor.Update();
        int y = _ver.Update();

        // If we have movement input, then execute the movement input event at the appropriate time.
        if (x != 0 || y != 0)
        {
            if (moveEvent != null)
            {
                if (x > 0)
                    moveEvent(this, new InfoEventArgs<MovementDirs>(MovementDirs.Left));
                if (x < 0)
                    moveEvent(this, new InfoEventArgs<MovementDirs>(MovementDirs.Right));
                if (y > 0)
                    moveEvent(this, new InfoEventArgs<MovementDirs>(MovementDirs.Up));
                if (y < 0)
                    moveEvent(this, new InfoEventArgs<MovementDirs>(MovementDirs.Down));
            }
        }

        // Loop through each of our firing inputs.
        for (int i = 0; i < _fires.Length; ++i)
        {
            // We specify button up since the firing button release is considered confirmation.
            if (CrossPlatformInputManager.GetButtonUp(_fires[i]))
            {
                if (fireEvent != null)
                    fireEvent(this, new InfoEventArgs<int>(i));
            }
        }
    }

}

/// <summary>
/// Used to check for 'repeat' functionality, such as holding down a movement button will cause it to repeat the movement again after a short time.
/// If using realtime input, then do not use the repeater and instead use CheckInput().
/// </summary>
class Repeater
{
    const float threshold = 0.5f;                   // The amount of pause to wait between an initial press of the button and the point at which it will begin repeating.
    const float rate = 0.25f;                       // The speed that the input will repeat.
    float _next;                                    // A point in time which must be passed before new events will be registered (used with timer).
    bool _hold;                                     // A boolean specifying whether or not the user has continued pressing the same button since the last event fired.
    string _axis;                                   // The axis to be monitored.

    public Repeater(string axisName)
    {
        _axis = axisName;
    }

    /// <summary>
    /// This update is called manually by the repeater to check if an event should be repeated.
    /// </summary>
    /// <returns>An axis value, such as -1, 0, or 1. 0 indicates the user is not pressing a button or that we are waiting for a repeat event.</returns>
    public int Update()
    {
        int retValue = 0;                                       // The return value.
        int axisValue = Mathf.RoundToInt(Input.GetAxisRaw(_axis));  // The value we are tracking; obtained from the Unity Input Manager.

        if (axisValue != 0)
        {
            if (Time.time > _next)                              // The initial button press will always be registered because _next is initially zero (in the else clause).
            {
                retValue = axisValue;
                _next = Time.time + (_hold ? rate : threshold); // If we are holding then update _next, otherwise set it to the threshold.
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }

        return retValue;
    }
}