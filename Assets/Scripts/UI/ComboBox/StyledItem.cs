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

