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
using UnityStandardAssets.CrossPlatformInput;

public class Keyboard_Input : BaseInputController {
	
	public override void CheckInput () {    
		// get input data from vertical and horizontal axis and store them internally in vert and horz so we don't
		// have to access them every time we need to relay input data.
        vert = CrossPlatformInputManager.GetAxis("Vertical");
        horz = CrossPlatformInputManager.GetAxis("Horizontal");
		
		// set up some boolean values for up, down, left and right
		Up      = (vert > 0);
		Down    = (vert < 0);
		Left    = (horz < 0);
		Right   = (horz > 0);    
		
		// get fire / action keys
        Fire1 = CrossPlatformInputManager.GetButton("Fire1");
        shouldRespawn = CrossPlatformInputManager.GetButton("Fire3");
	}
	
	public void LateUpdate () {
		// check inputs each LateUpdate() ready for the next tick
		CheckInput ();
	}
}