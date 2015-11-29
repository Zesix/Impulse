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

    // directional buttons
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;

    // firing buttons
    public bool Fire1;
    public bool Fire2;

    // weapon slots
    public bool Slot1;
    public bool Slot2;
    public bool Slot3;
    public bool Slot4;
    public bool Slot5;
    public bool Slot6;
    public bool Slot7;
    public bool Slot8;
    public bool Slot9;

    public float vert;
    public float horz;
    public bool shouldRespawn;

    public Vector3 TEMPVec3;

    // A string array containing our firing buttons.
    string[] _fires = new string[]
        {"Fire1",
        "Fire2"};

    // Initialize Repeaters for each axis.
    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");

    // Whenever a Repeater reports movement input, we will share it as a static event.
    public static event EventHandler<InfoEventArgs<int>> moveEvent;

    // Whenever a Repeater reports firing input, we will pass it along.
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    public virtual void CheckInput()
    {
        // override with your own code to deal with input
        horz = CrossPlatformInputManager.GetAxis("Horizontal");
        vert = CrossPlatformInputManager.GetAxis("Vertical");
    }

    public virtual float GetHorizontal()
    {
        // returns our cached horizontal input axis value
        return horz;
    }

    public virtual float GetVertical()
    {
        // returns our cached vertical input axis value
        return vert;
    }

    public bool GetRespawn()
    {
        return shouldRespawn;
    }

    public virtual Vector3 GetMovementDirectionVector()
    {
        // temp vector for movement dir gets set to the value of an otherwise unused vector that always have the value of 0,0,0
        TEMPVec3 = Vector3.zero;

        TEMPVec3.x = horz;
        TEMPVec3.y = vert;

        // return the movement vector
        return TEMPVec3;
    }

    // Update is called once per frame
    void Update()
    {
        // Set axis variables to update according to the Repeater instead of general Update() loop.
        int x = _hor.Update();
        int y = _ver.Update();

        // If we have movement input, then execute the movement input event at the appropriate time.
        if (x != 0 || y != 0)
        {
            // if (moveEvent != null)
            //        Handle the movement event here.
        }

        // Loop through each of our firing inputs.
        for (int i = 0; i < _fires.Length; ++i)
        {
            // We specify button up since the firing button release is considered confirmation.
            if (Input.GetButtonUp(_fires[i]))
            {
                if (fireEvent != null)
                    fireEvent(this, new InfoEventArgs<int>(i));
            }
        }
    }

}

/// <summary>
/// Used to check for 'repeat' functionality, such as holding down a movement button will cause it to repeat the movement again after a short time.
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