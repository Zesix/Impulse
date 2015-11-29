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
using System.Collections.Generic;

/// <summary>
/// A simple class to hold a single field of any data type named info.
/// Used often with input events since a single field of data is all that is ever passed at a time.
/// </summary>
public class InfoEventArgs<T> : EventArgs
{
    public T info;

    /// <summary>
    /// An empty constructor that inits itself using the default keyword (handles both reference and value types).
    /// </summary>
    public InfoEventArgs()
    {
        info = default(T);
    }

    /// <summary>
    /// A constructor that allows the user to specify the initial value.
    /// </summary>
    /// <param name="info">Initial value.</param>
    public InfoEventArgs(T info)
    {
        this.info = info;
    }
}
