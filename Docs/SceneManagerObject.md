# SceneManager

SceneManager is an object that mostly operates "behind the scenes" so to speak, no pun intended.  

## Static Properties

### SceneManager.IsSceneInitialized (bool)

IsSceneInitialized will return true once the current SceneView.Initialize() method has returned completely.  You can use this value to make sure a totally encapsulated component object away from the framework doesn't attempt to use an unresolved dependency inside of the scene before it's ready.

## Command Type Listeners

### ChangeSceneCmd

This command is bound to the `OnChangeScene()` method.  This function is responsible for calling UnityEngine.Application.LoadLevel() with the scene name passed in to the command.  If you have a Unity pro license it will call `LoadLevelAsync()` otherwise it will call `LoadLevel()`

### SceneLoadedCmd

This command is bound to the `OnSceneLoaded()` method.  This function is responsible for invoking your implemented SceneView's `Initialize()` method and then calling `InitializeSceneObjects()` on the SceneView's member.  This command holds a copy of the SceneView object itself in it's .View property.