# Impulse #

A barebones C# Unity framework for building scalable projects quickly and easily. 
It includes a combination of utility scripts such as timer class and artificial friction alongside
easily customizable and out-of-box systems like scene management and mobile-optimized menu.

## Video Overviews ##

These demonstrate the functionality of the included systems, all of which can be easily customized and extended.

 * [Splash Screen, Scene Manager, Menu System](https://www.youtube.com/watch?v=btNqHCoRwB8&index=1&list=PLLXw4Fw6qNw5WVLPn1hhJNEcwXjxt3b9j)
 * [Music Manager and Playlists](https://www.youtube.com/watch?v=jQGTqGalGVw&list=PLLXw4Fw6qNw5WVLPn1hhJNEcwXjxt3b9j&index=2)
 * [Top Down Follow Camera](https://www.youtube.com/watch?v=DLTyrbMxytA)
 * [Third Person Camera](https://www.youtube.com/watch?v=DDdnLPPZXLg)
 * [Viewcones](https://www.youtube.com/watch?v=Dzby6O7ds3A&list=PLLXw4Fw6qNw5WVLPn1hhJNEcwXjxt3b9j&index=5)
 * [UGUI Combobox/Dropdown Box](https://www.youtube.com/watch?v=Wetc5hFMShA&list=PLLXw4Fw6qNw5WVLPn1hhJNEcwXjxt3b9j&index=6)
 * [AI Detection System](https://www.youtube.com/watch?v=1ZLkDv9OUNc&list=PLLXw4Fw6qNw5WVLPn1hhJNEcwXjxt3b9j&index=7)

## Manager / Controller System ##

Keep in mind the framework is not intended to force you into any particular programming paradigm. 
However, the manager / controller system used by the framework is an easy way to organize a project and can 
be further adapted to MVC, MVVM, or any number of architectural patterns should the developer choose to do so.

**Managers** manipulate the game itself, such as creating and destroying game objects. 
**Controllers** manipulate game objects within the game and are commonly used for movement.

For example, in an FPS game there would a player controller and manager. When the controller detects a collision
with an enemy projectile, it passes this information to the manager, which determines the damage is dealt and
subtracts health accordingly. If the manager determines the player has zero health, it passes this information
to the global game manager that destroys or pools the player object and updates the number of players remaining.

## Useful Scripts ##

These can be used for a variety of purposes and are easily customizable and extensible.

 * ExtendedMonoBehavior.cs - Includes commonly used variable declarations for caching.
 * ChangeScene.cs - Calls the scene manager to switch scenes.
 * AutoSpinObj.cs - Causes an object to spin automatically upon start.
 * ArtificialFriction.cs - Applies artificial friction to an object moved by physics forces.

## Input Handling ##

These extend BaseInputController but the entire input system can be easily replaced with a different solution.

 * Keyboard_Input.cs - Translates keyboard inputs into general direction bools such as up, down, left, right, etc.
 * Mouse_Input.cs - Grabs mouse position and click types.
 * ThirdPersonCameraMouse_Input - Adds panning and zoom-in controls for use with the third person camera.
