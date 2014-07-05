## Core Principals:

 - A single component called `Framework` is attached to a permanent GameObject inside every scene in your project.
   * A Prefab containing the Framework Object is available in the _Prefabs folder for you to use if you wish.
 - When a Unity scene is loading the Framework will search for a component in your project named `<YourSceneName>SceneView`.  For example if your scene is named Game.Unity the framework will require a component named `GameSceneView`.  This component will be automatically attached to a new GameObject which will be named `SceneView` in your hierarchy.
 - The Framework requires that your `GameSceneView` component is derived from a base class provided in the Framework called `SceneView`.
 - SceneView has an abstract method named `Initialize()` which you must implement in your derived `GameSceneView` object.  It is within this method that you can begin to set up and start your scene.
 - The Framework has two "namespaces" reserved for storing and manipulating objects in memory.
   * Global space 
     - `Framework` contains a static accessor available at `Framework.Globals` which is the Global space object container.   
   * Scene space.  
     - `SceneView`'s base class contains a property called `SceneObjects` which is the Scene space object container.
 - A Global object will remain alive for the entire duration of your applications lifetime from the moment you create it until the moment the game closes.
 - A Scene object will remain alive only up until the point where you tell the framework to change scenes. 
