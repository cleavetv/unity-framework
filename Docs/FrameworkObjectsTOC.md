# Framework Objects:

 - Framework : The Framework object itself
 - Command : abstract object implements basic event listening callbacks
 - EngineOptions : A generic options structure containing settings for things like screen resolution, volumes, and rendering qualities.
 - App : Currently functions a container object for the EngineOptions
 - CommandQueue : Contains and processes Command objects pushed to the Framework
 - View : abstract object derived from MonoBehaviour
 - SceneManager : implements basic scene switching functionality
 - SceneObjectData : implements generic containers for objects which live inside the Unity Scene
 - SceneView : abstract object derived from View which holds SceneObjectData
 - CDebug : basic debugging tool (see tools)
 - Factory : Generic Factory for creating objects and performing post-instantiation construction
 - Binder : Wrapper for a Generic Dictionary
 - BindingLibrary : Generic collection of Binders
 - Injector : Dependency Injector for object creation (relies on Factory) 