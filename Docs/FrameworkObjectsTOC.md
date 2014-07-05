# Framework Objects:

These pages will go in-depth discussing the interactivity between and functionality and extensibility of these objects.

## Namespaces

### CleaveFramework.Core
 - [Framework](../master/Docs/FrameworkObject.md)
 - [EngineOptions](..master/Docs/EngineOptionsObject.md) : A generic options structure containing settings for things like screen resolution, volumes, and rendering qualities.
 - [App](..master/Docs/AppObject.md) : Currently functions as container object for the EngineOptions
 - [CommandQueue](..master/Docs/CommandQueueObject.md) : Contains and processes Command objects pushed to the Framework
 - [View](..master/Docs/ViewObject.md) : abstract object derived from MonoBehaviour

### CleaveFramework.Commands
 - [CmdBinder](..master/Docs/CmdBinderObject.md) : 
 - [Command](..master/Docs/CommandObject.md) : abstract object implements basic event listening callbacks

### CleaveFramework.Binding
 - [Binder](..master/Docs/BinderObject.md) : Wrapper for a Generic Dictionary
 - [BindingLibrary](..master/Docs/BindingLibraryObject.md) : Generic collection of Binders
 
### CleaveFramework.DependencyInjection
 - [Injector](..master/Docs/InjectorObject.md) : Dependency Injector for object creation

### CleaveFramework.Factory
 - [Factory](..master/Docs/FactoryObject.md) : Generic Factory for creating objects and performing post-instantiation construction

### CleaveFramework.Scene
 - [SceneManager](..master/Docs/SceneManagerObject.md) : implements basic scene switching functionality
 - [SceneObjectData](..master/Docs/SceneObjectDataObject.md) : implements generic containers for objects which live inside the Unity Scene
 - [SceneView](..master/Docs/SceneViewObject.md) : abstract object derived from View which holds SceneObjectData

### CleaveFramework.SceneViews
 - [LoadingSceneView](..master/Docs/LoadingSceneViewObject.md)

### CleaveFramework.Tools
 - [CDebug](..master/Docs/CDebugObject.md) : basic debugging tool (see tools)



 