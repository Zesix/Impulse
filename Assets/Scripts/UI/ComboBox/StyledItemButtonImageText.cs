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