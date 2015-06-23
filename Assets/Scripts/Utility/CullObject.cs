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

/// <summary>
/// Shows or hides an object's mesh renderer based on whether the culling target is within the Sphere Collider or not.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class CullObject : MonoBehaviour
{
    // The object we are comparing distance to, for culling. 
    public Collider CullingTarget;

    // Our renderer.
    Renderer myRenderer;

    // Our Sphere Collider
    SphereCollider myCullingCollider;

    void Start ()
    {
        myCullingCollider = GetComponent<SphereCollider>();
        myCullingCollider.isTrigger = true;
        myRenderer = transform.parent.GetComponent<Renderer>();
        myRenderer.enabled = false;

        if (CullingTarget == null)
            Debug.LogWarning("No culling target specified for " + gameObject + "!");
    }

    // When the culling target is within the collider, we render the object.
    void OnTriggerEnter(Collider collider)
    {
        if (collider = CullingTarget)
        {
            myRenderer.enabled = true;
        }
    }

    // When the culling target leaves the collider, we hide the object.
    void OnTriggerExit(Collider collider)
    {
        if (collider = CullingTarget)
        {
            myRenderer.enabled = false;
        }
    }
}
