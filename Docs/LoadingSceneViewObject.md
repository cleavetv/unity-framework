# LoadingSceneView

The Loading.Unity scene and companion LoadingSceneView is a default implementation of how the Framework utilizes a transitional scene to load your next scene.  You can make a transitional scene as simple or complex as you want while your next scene is being constructed.

## Changing transitional scene

The Framework object holds a reference to the name of a scene the Framework will use as a transitional scene.  By default the scene is set to `Loading` and is included with the CleaveFramework.  Feel free to modify this scene or even create custom loading scenes tailored specifically to the target scene you are loading in to.

```csharp
// Access and modify transition scene name like:
Framework.TransitionScene = "DifferentLoader";
```

TransitionScene is also exposed as a public property editable on the Framework Object inside of the UnityEditor.
