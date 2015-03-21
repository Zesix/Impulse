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
using UnityEngine.UI;
using System;

public class StyledItemButtonImageText : StyledItem
{
	public RawImage 	rawImageCtrl;
	public Text   		textCtrl;
	public Button		buttonCtrl;
	
	public class Data
	{
		public Data(string t, Texture2D tex) { text = t; image = tex; }
		public string text;
		public Texture2D image;
	}

	public override Button GetButton () { return buttonCtrl; }
	public override Text GetText () { return textCtrl; }
	public override RawImage GetRawImage () { return rawImageCtrl; }

	// we accept a string, a texture2d, or a data object if we want both.
	public override void Populate(object o)
	{
		Texture2D tex = o as Texture2D;
		if (tex != null)
		{
			if (rawImageCtrl != null)
			{
				rawImageCtrl.texture = tex;
			}
			return;
		}

		Data d = o as Data;
		if (d == null)
		{
			if (textCtrl != null)
				textCtrl.text = o.ToString();	// string..
			return;
		}
		
		if (rawImageCtrl != null)
		{
			rawImageCtrl.texture = d.image;
		}
		if (textCtrl != null)
		{
			textCtrl.text = d.text;
		}
	}
}