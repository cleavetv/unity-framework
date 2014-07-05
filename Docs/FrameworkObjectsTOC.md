# Framework Objects:

These pages will go in-depth discussing the interactivity between and functionality and extensibility of these objects.

## Namespaces

### CleaveFramework.Core
 - [Framework](FrameworkObject.md)
 - [EngineOptions](EngineOptionsObject.md) : A generic options structure containing settings for things like screen resolution, volumes, and rendering qualities.
 - [App](AppObject.md) : Currently functions as container object for the EngineOptions
 - [CommandQueue](CommandQueueObject.md) : Contains and processes Command objects pushed to the Framework
 - [View](ViewObject.md) : abstract object derived from MonoBehaviour

### CleaveFramework.Commands
 - [CmdBinder](CmdBinderObject.md) : 
 - [Command](CommandObject.md) : abstract object implements basic event listening callbacks

### CleaveFramework.Binding
 - [Binder](BinderObject.md) : Wrapper for a Generic Dictionary
 - [BindingLibrary](BindingLibraryObject.md) : Generic collection of Binders
 
### CleaveFramework.DependencyInjection
 - [Injector](InjectorObject.md) : Dependency Injector for object creation

### CleaveFramework.Factory
 - [Factory](FactoryObject.md) : Generic Factory for creating objects and performing post-instantiation construction

### CleaveFramework.Scene
 - [SceneManager](SceneManagerObject.md) : implements basic scene switching functionality
 - [SceneObjectData](SceneObjectDataObject.md) : implements generic containers for objects which live inside the Unity Scene
 - [SceneView](SceneViewObject.md) : abstract object derived from View which holds SceneObjectData

### CleaveFramework.SceneViews
 - [LoadingSceneView](LoadingSceneViewObject.md)

### CleaveFramework.Tools
 - [CDebug](CDebugObject.md) : basic debugging tool (see tools)



 