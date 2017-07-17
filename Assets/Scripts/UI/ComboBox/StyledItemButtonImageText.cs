using UnityEngine;
using UnityEngine.UI;

public class StyledItemButtonImageText : StyledItem
{
	public RawImage 	RawImageCtrl;
	public Text   		TextCtrl;
	public Button		ButtonCtrl;
	
	public class Data
	{
		public Data(string t, Texture2D tex) { Text = t; Image = tex; }
		public string Text;
		public Texture2D Image;
	}

	public override Button GetButton () { return ButtonCtrl; }
	public override Text GetText () { return TextCtrl; }
	public override RawImage GetRawImage () { return RawImageCtrl; }

	// we accept a string, a texture2d, or a data object if we want both.
	public override void Populate(object o)
	{
		var tex = o as Texture2D;
		if (tex != null)
		{
			if (RawImageCtrl != null)
			{
				RawImageCtrl.texture = tex;
			}
			return;
		}

		var d = o as Data;
		if (d == null)
		{
			if (TextCtrl != null)
				TextCtrl.text = o.ToString();	// string..
			return;
		}
		
		if (RawImageCtrl != null)
		{
			RawImageCtrl.texture = d.Image;
		}
		if (TextCtrl != null)
		{
			TextCtrl.text = d.Text;
		}
	}
}