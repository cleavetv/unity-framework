# CleaveFramework v0.2.0

A Unity3D C# application framework.

## Why you use a framework

A good application framework allows it's users to save time by using generic code that allows them to focus on more project-specific areas.  

A great application framework provides it's users with excellent, fast, well tested, and well maintained generic modules capable of providing their application with a structure they can depend on.

Every piece of software you've even worked with has utilized a framework in one way or another.  Whether it was home-rolled or an off the shelf solution the very structure of your code base makes up the framework it executes on.  If you've ever finished, or worse started and not finished, a project using the Unity3D engine you realize the importance of well structuring your code.  If find yourself re-implementing the same functionality from project to project time and time again then you are a great candidate for migrating your work-flow into a framework package.

## What CleaveFramework can do for you

If you've ever written code that looks like:

```csharp
TheGlobals.GameManager.Game.WorldManager.World.SpawnPlayer(TheGlobals.GameManager.Game.PlayerManager.MakeNewPlayer());
TheGlobals.GameManager.Game.HUDManager.ShowWelcomeMsg(TheGlobals.GameManager.Game.PlayerManager.Player.Name);
TheGlobals.GameManager.Game.Sounds.PlaySound(GameSounds.WelcomeSound);
// etc...
```

Or worse yet: you dream of one day writing code that looks that clean.  You're in dire need of a better structure and work-flow.

You could write this snippet utilizing framework features in one line:
```csharp
Framework.Dispatch(new SpawnNewPlayerCmd());
```
You'll have to read further to see how powerful the Command Dispatching system is, and hopefully that alone has got you interested.

The framework was created to assist in creating and maintaining a better structure for you by implementing several core features.  However most of the framework itself remains modular and you can pick and choose how much or little of it you want to use and how you want to use it.

The framework will not constrain you in to working within one particular design paradigm.  Bits and pieces of it's implementation can be constructed together to form a flow that makes the best sense for you and your project.

What the framework is not is perfect.  We need your help and feedback to make this a great usable piece of software that programmers of all skill levels can use, learn from, and make great games with.

## Documentation

### [Installation](../master/Docs/Installation.md)
### [Core Principals](../master/Docs/CorePrincipals.md)
### [Basic Usage](../master/Docs/BasicUsage.md)
### [Framework Objects](../master/Docs/FrameworkObjectsTOC.md)
### [Interfaces](../master/Docs/Interfaces.md)

## General How To:

##### Change a scene:
```csharp
Framework.PushCommand(new ChangeSceneCmd(<SceneName>));
```
##### Change an option value:
```csharp
Framework.App.Options.FullScreen = false;
```
##### Apply options and write the configuration to disk:
```csharp
Framework.PushCommand(new ApplyOptionsCmd());
```
##### Implement a custom command:
```csharp
class MyCustomCommand<T> : Command
{
    public T Data;
    public MyCustomCommand(T data) {
       Data = data;
    }
}
```
##### Push your custom command:
```csharp
// execute on next Framework's Process()
Framework.PushCommand(new MyCustomCommand<int>(42));
// delay execution for 3 frames
Framework.PushCommand(new MyCustomCommand<int>(42), 3);
// delay execution for 5 seconds
Framework.PushCommand(new MyCustomCommand<int>(42), 5.0f);
```
##### Implement a custom command listener:
```csharp
Command.Register(typeof(MyCustomCommand<int>), OnCustomCommand);
// or with generic method:
Command.Register<MyCustomCommand<int>>(OnCustomCommand);
```

```csharp
void OnCustomCommand(Command c)
{
   var cmd = c as MyCustomCommand<int>;
   // cmd.Data = 42 here and you can use it as you wish...
}
```
Note: Many objects can implement listeners for the same command so they can process the data appropriately.  For example: Some internal game system can listen to an incoming command and act on it appropriately while your hud system can also listen to the same command and update it's view appropriately without any coupling between the systems.

##### Unregister a command listener:
```csharp
Command.Unregister(typeof(MyCustomCommand<int>), OnCustomCommand);
// or with generic command
Command.Unregister<MyCustomCommand<int>>(OnCustomCommand);
``` 
##### Pass a data model from one scene to another:
###### As singleton:
```csharp
Framework.Globals.PushObjectAsSingleton(new CustomDataModel());
```
###### As transient:
```csharp
Framework.Globals.PushObjectAsTransient("myCustomData", new CustomDataModel());
```
##### Resolve a data model from the globals:
###### As singleton:
```csharp
var myCustomData = Framework.Globals.ResolveSingleton<CustomDataModel>() as CustomDataModel;
``` 
###### As transient:
```csharp
var myCustomData = Framework.Globals.ResolveTransient<CustomDataModel>("myCustomData") as CustomDataModel;
```
##### Access to SceneView can be done in the standard way you access any Unity component:
```csharp
var sceneView = GameObject.Find("SceneView").GetComponent<SceneView>() as SceneView;
```


## Dynamic Objects:

During gameplay obviously it is highly likely you will need to instantiate new objects, the framework fully supports it.

Here is the order of operations for an object instantiated through Factory at runtime:
 - Your object's default constructor is executed (ie: var obj = new MyObject();)
 - Your object is evaluated for [Inject] attributes and existing dependencies are resolved.
 - Your object's Factory defined default constructor is invoked.
 - Your object is added to SceneData if it is provided to the Factory's create method.
  - If your object is added to the Scene Data then: 
    - Your object's Initialize() method is invoked if you have implemented the interface.
    - Your object's Configure() method is invoked if you have implemented the interface.
 - Factory.Create returns your object now.
