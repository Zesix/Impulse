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

/// <summary>
/// XML read write.
/// IMPORTANT: FOR XML LOADING TO BE DONE
/// THIS SCRIPT NEEDS TO BE ON A GAMEOBJECT
/// </summary>

using UnityEngine;
using System.Collections;

public class XMLReadWrite : MonoBehaviour
{
	
	public static XMLReadWrite Instance { get; private set; }
	
	public void Start ()
	{
		Instance = this;
		
		DontDestroyOnLoad (this);
		
	}

	public XMLExample[] DataLoad (string folderPath, string fileName)
	{
		XMLExample[] newObjects = XMLData.LoadObjects (folderPath, fileName + ".xml");
		
		if (newObjects == null) {
			return null;
			
		}

		return newObjects;
		
	}
	
	public void SaveData (string folderPath, string fileName, XMLExample[] objToSave)
	{
		
		XMLData.SaveObjects (folderPath, fileName, objToSave);
		
	}
}
