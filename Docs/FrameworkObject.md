# Framework Object
Framework is implemented as a View and attaches to a Unity GameObject when your game starts.  A framework GameObject is expected to be found in every scene, a prefab located in _Prefabs is provided to speed up the process.  It is a very simple object containing just one component.  You are free to use this object as a parent for other objects you wish to stay alive for your games lifetime as children of objects marked for `DontDestroyOnLoad` also stay resident.

## Activating Commands to the Framework

The Framework object holds the static interface for pushing Command Type objects into the main application execution queue.  The interface is quite simple, there are two types of methods each in groups of 3 for 6 total methods.

### Framework.Dispatch()

`Framework.Dispatch()` is the method group capable of pushing commands which implement [Inject] attributes.

### Framework.PushCommand()

`Framework.PushCommand()` is the method group capable of pushing commands not requiring injections.

### Command method overloads

Each method type also has 2 overloads available to it.  These provide the capability of putting commands in to the queue that will execute with a delay component attached to them.  The delays available are: frame based and time based.

#### Frame based delays:

Giving an integer as the second parameter to the command method will cause that command to be executed in that number of frames from current frame.  For example a 1 here will make the command execute on the next frame.  This can be useful if you need to have a blocking command activate but want to render one additional frame first like in the case of displaying a please wait message.

#### Time based delays:

Giving a float as the second parameter will cause that command to be executed in that number of seconds from the method call.  For example a 3.0f here will make the command execute in 3 seconds.  This can be useful if you'd like something to happen but want to give a non-frame limited leeway in between the request and the actual execution.

## Globals

Framework contains a static accessor for a [SceneObjectsData](SceneObjectsDataObject.md) capable of storing global data.  This data will persist throughout the execution lifetime of your game or until you have cleaned up all references to it and removed it from the Globals manually.  

```csharp
// access globals via static method like:
Framework.Globals
```

## [App](AppObject.md)

Framework contains a static accessor for the [App Object](AppObject.md) which is accessible:

```csharp
Framework.App
```