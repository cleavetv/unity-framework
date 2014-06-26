# CleaveFramework v0.1.0 

A Unity3D C# application framework.

## Why you use a framework
﻿
This framework is meant to faciliate the implementation of a better structure for Unity3D game project code.  It is by no means perfect, and I welcome any and all feedback and potential contributions in regards to improving its functionality and easing its usability.  Thanks!

If you've ever finished, or worse started and not finished, a project using the Unity3D engine you realize the importance of well structuring your code.  This framework was created to assist in maintaining that structure.  The framework does not limit or shoehorn you into developing into a particular pattern however I personally believe it is most useful when utilizing a MVC/DI object pattern.  You are however free from being constrained to one specific architecture pattern, be it DI, Service Location, or whatever magical Voodoo you personally have created.

## Feature overview

The core functionality of the framework is two-fold based around an executable command delegate callback system and a SceneObjectData container for objects, automated system updating, and global data.  Any amount of unique Objects are able to subscribe to commands and then implement callbacks upon execution of the command.  Commands can be utilized to pass data to another object, as event messaging, or a virtual Execute() method can be implemented to  directly manipulate the data within the command object itself before or after propogating to the delegates.  Commands are pushed into the Framework via static methods which can perform commands on that frame, or after a given delay of frames or seconds.  Commands can push other Commands during their execution giving the ability to create sequences of events.

## Usage:

Clone or pull repository.  Copy Assets/CleaveFramework into the location of your Unity project Assets.  Load Unity.


## Basic Implementation:

The Framework executes based around several simple principals:

 - A single object in your Unity scenes contains the `Framework` component attached to it.  This object must exist in every scene.
 - A component is implemented with the name `<YourScene>SceneView`.  For example: a `GameSceneView` component is expected when initializing a scene named `Game`.
 - Your SceneView component is derived from the `CleaveFramework.Scene.SceneView` object.
 - Objects are instantiated in your derived `SceneView.Initialize()` implementation through the exposed `SceneObjects` instance of `SceneObjectsData`.
 
## Interfaces:

### IInitializable 

SceneObjects implementing this interface will have `Initialize()` invoked on them at the point in which you call `SceneObjects.InitializeSceneObjects()` -- you should call this method at the end of your `SceneView.Initialize()` implementation

### IConfigureable 

SceneObjects implementing this interface will have `Configure()` invoked on them immediately after all SceneObjects have been completely initialized.  At this point you are now able to bind any references to any initialized object.  For example: in the case of a View object added to your scene hierarchy in UnityEditor mode and has it's instance resolved during Initialize.

### IUpdateable 

SceneObjects implementing this interface will have `Update(deltaTime)` invoked on them during the SceneView object's update cycle with `Time.deltaTime` as the parameter.


### IDestroyable

SceneObjects implementing this interface will have `Destroy()` invoked on them at the point in which the `OnDestroy()` method on your SceneView is being called by the UnityEngine.
 
## Objects:

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
 
## How To:

##### Change a scene:
```
Framework.PushCommand(new ChangeSceneCmd(<SceneName>));
```
##### Change an option value:
```
Framework.App.Options.FullScreen = false;
```
##### Apply options and write the configuration to disk:
```
Framework.PushCommand(new ApplyOptionsCmd());
```
##### Implement a custom command:
```
class MyCustomCommand<T> : Command
{
    public T Data;
    public MyCustomCommand(T data) {
       Data = data;
    }
}
```
##### Push your custom command:
```
Framework.PushCommand(new MyCustomCommand<int>(42));
```
##### Implement a custom command listener:
```
Command.Register(typeof(MyCustomCommand<int>), OnCustomCommand);
void OnCustomCommand(Command c)
{
   var cmd = c as MyCustomCommand<int>;
   // cmd.Data = 42 here and you can use it as you wish...
}
```
Note: Many objects can implement listeners for the same command so they can process the data appropriately.  For example: Some internal game system can listen to an incoming command and act on it appropriately while your hud system can also listen to the same command and update it's view appropriately without any coupling between the systems.

##### Unregister a command listener:
```
Command.Unregister(typeof(MyCustomCommand<int>), OnCustomCommand);
``` 
##### Pass a data model from one scene to another:
###### As singleton:
```
Framework.Globals.PushObjectAsSingleton(new CustomDataModel());
```
###### As transient:
```
Framework.Globals.PushObjectAsTransient("myCustomData", new CustomDataModel());
```
##### Resolve a data model from the globals:
###### As singleton:
```
var myCustomData = Framework.Globals.ResolveSingleton<CustomDataModel>() as CustomDataModel;
``` 
###### As transient:
```
var myCustomData = Framework.Globals.ResolveTransient<CustomDataModel>("myCustomData") as CustomDataModel;
```
##### Access to SceneView can be done in the standard way you access any Unity component:
```
var sceneView = GameObject.Find("SceneView").GetComponent<SceneView>() as SceneView;
```
 
## Tools:

### CDebug

Provides a wrapper for Unity's logger with added functionality:
 - Enable/Disable logging globally
 - Enable/Disable logging per type
  
## Dynamic Objects:
 
Objects instantiated after the SceneView::Initialize() process implementing IInitializeable and IConfigureable will have their Initialize() and Configure() methods invoked on them in sequence immediately after being pushed into the SceneObjects.
