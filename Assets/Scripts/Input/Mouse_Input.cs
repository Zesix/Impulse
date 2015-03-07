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

public class Mouse_Input : BaseInputController {
	private Vector2 prevMousePos;
	private Vector2 mouseDelta;
	
	private float speedX = 0.05f;
	private float speedY = 0.1f;
	
	public void Start () {
		prevMousePos = Input.mousePosition;
	}
	
	public override void CheckInput () {
		// get input data from vertical and horizontal axis and store them internally in vert and horz so we don't
		// have to access them every time we need to relay input data out
		
		// calculate a percentage amount to use per pixel
		float scalerX = 100f / Screen.width;
		float scalerY = 100f / Screen.height;
		
		// calculate and use deltas
		float mouseDeltaY =  Input.mousePosition.y - prevMousePos.y;
		float mouseDeltaX =  Input.mousePosition.x - prevMousePos.x;
		
		// scale based on screen size
		vert += ( mouseDeltaY * speedY ) * scalerY;
		horz += ( mouseDeltaX * speedX ) * scalerX;
		
		// store this mouse position for the next time we're here
		prevMousePos = Input.mousePosition;
		
		// set up some boolean values for up, down, left and right
		Up    	= ( vert>0 );
		Down    = ( vert<0 );
		Left    = ( horz<0 );
		Right   = ( horz>0 );    
		
		// REMEMBER: To make this work, you need to change some input settings. In Input settings, remove mouse 0 from Fire1 and change mouse 1 in Fire2 to mouse 0
		
		// get fire / action buttons
		Fire1 = Input.GetButton( "Fire2" );
	}
	
	public void LateUpdate() {
		// check inputs each LateUpdate() ready for the next tick
		CheckInput();
	}
}