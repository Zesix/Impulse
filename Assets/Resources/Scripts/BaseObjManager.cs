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

public class BaseObjManager : MonoBehaviour {
	public bool didInit;
	
	public BaseStatsManager DataManager;
	
	// note that we initialize on Awake in this class so that it is ready for other classes to
	// access our details when they initialize on Start.
	public virtual void Awake () {
		didInit = false;
		
		// rather than clutter up the start() func, we call Init to do any
		// startup specifics
		Init();
	}
	
	public virtual void Init () {
		// cache ref to our stats manager 
		DataManager = gameObject.GetComponent<BaseStatsManager>();
		
		// throw error if object is missing a BaseStatsManager
		if (DataManager == null)
			Debug.LogError ("GameObject is missing a BaseStatsManager: " + gameObject.name);
		
		// do play init things in this function
		didInit= true;
	}
	
}