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
 * [Random Object Spawner with Culling](https://www.youtube.com/watch?v=aonCtVL7HGo&list=PLLXw4Fw6qNw5WVLPn1hhJNEcwXjxt3b9j&index=8)
 * [Events and Notifications](https://www.youtube.com/watch?v=EfVEFU0xxys&list=PLLXw4Fw6qNw5WVLPn1hhJNEcwXjxt3b9j&index=10)

## Model-View-Controller (MVC) Pattern ##

Keep in mind the framework is not intended to force you into any particular programming paradigm. 
However, the model-view-controller pattern used by the framework is an easy way to organize a project and is recommended as the 'default' way to structure a project.

## Useful Scripts ##

These can be used for a variety of purposes and are easily customizable and extensible.

 * ExtendedMonoBehavior.cs - Includes commonly used variable declarations for caching.
 * ChangeScene.cs - Calls the scene manager to switch scenes.
 * AutoSpinObj.cs - Causes an object to spin automatically upon start.
 * ArtificialFriction.cs - Applies artificial friction to an object moved by physics forces.

## Input Handling ##

These extend BaseInputController but the entire input system can be easily replaced with a different solution.

 * KeyboardInputController.cs - Translates keyboard inputs into general direction bools such as up, down, left, right, etc.
 * MouseInputController.cs - Grabs mouse position and click types.
 * ThirdPersonCameraMouseInputController.cs - Adds panning and zoom-in controls for use with the third person camera.
