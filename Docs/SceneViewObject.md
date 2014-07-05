# SceneView

The SceneView is the abstract class of which you will derive your SceneView implementations from.

SceneView contains an accessible instance of [SceneObjectData](SceneObjectDataObject.md) named `SceneObjects` to use for populationg with your Scene space's related objects.  

SceneView in combination with the [SceneManager](SceneManagerObject.md) takes care of handling the tasks related to implementing the various available [Interfaces](Interfaces.md) in the framework.

When `SceneView.Update()` is called by the UnityEngine or by your overriden implementation of `Update()` it will call `Update()` for you on the [SceneObjects](SceneObjectDataObject.md).

When `SceneView.OnDestroy` is called by the UnityEngine or by your overriden implementation of `OnDestroy()` it will call `Destroy()` for you on the [SceneObjects](SceneObjectDataObject.md)

## SceneView.Initialize()

`SceneView.Initialize()` is an abstract method required by your implementations of each `SceneView`.  It is automatically invoked by the [SceneManager](SceneManagerObject.md) upon receiving the `SceneLoadedCmd` Command Type.  

###### `SceneLoadedCmd` is the framework event which occurs after UnityEngine is done building the hierarchy for your scene as serialized by the UnityEditor.  
