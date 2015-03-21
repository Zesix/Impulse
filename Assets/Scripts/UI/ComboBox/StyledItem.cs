using UnityEngine;
using System.Collections;
using UnityEngine.UI;

 
public class StyledItem : MonoBehaviour
{
	public virtual void Populate(object o) {}
	
	// most things boil down to these basic elements, so fill out virtuals
	// hmm... I really wish there was a better way to abstract this via
	// generics or something, but there's not really a way I can think of..

	public virtual Selectable GetSelectable() { return null; }
	public virtual Button     GetButton() { return null; }
	public virtual Text       GetText() { return null; }
	public virtual RawImage   GetRawImage() { return null; }
	public virtual Image      GetImage() { return null; }
}

