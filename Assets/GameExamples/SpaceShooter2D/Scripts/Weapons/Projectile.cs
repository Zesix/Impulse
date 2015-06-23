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

namespace SpaceShooter2D
{

    [RequireComponent(typeof(AudioSource))]
    public class Projectile : MonoBehaviour
    {

        public float Damage;
        public Faction.Factions Faction;
        public AudioClip shootFX;
        [Range(0, 1)]
        public float shootFXVolume = 0.5f;
        protected AudioSource myAudio;

        void OnEnable()
        {
            myAudio = GetComponent<AudioSource>();
        }

        public void PlayShotFX()
        {
            myAudio.PlayOneShot(shootFX, shootFXVolume);
        }

        /// <summary>
        /// Deacivates this gameobject
        /// </summary>
        public void Deactivate()
        {
            // This will be replaced with gameObject.SetActive(False) when the pooling system is implemented
            Destroy(gameObject);
        }
    }
}
