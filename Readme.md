# CleaveFramework v0.1.2

A Unity3D C# application framework.

## Why you use a framework
﻿
This framework is meant to facilitate the implementation of a better structure for Unity3D game project code.  It is by no means perfect, and I welcome any and all feedback and potential contributions in regards to improving its functionality and easing its usability.  Thanks!

If you've ever finished, or worse started and not finished, a project using the Unity3D engine you realize the importance of well structuring your code.  This framework was created to assist in maintaining that structure.  The framework does not limit or shoehorn you into developing into a particular pattern however I personally believe it is most useful when utilizing a MVC/DI object pattern.  You are however free from being constrained to one specific architecture pattern, be it DI, Service Location, or whatever magical Voodoo you personally have created.

## Feature overview

The core functionality of the framework is two-fold based around an executable command delegate callback system and a SceneObjectData container for objects, automated system updating, and global data.  Any amount of unique Objects are able to subscribe to commands and then implement callbacks upon execution of the command.  Commands can be utilized to pass data to another object, as event messaging, or a virtual Execute() method can be implemented to  directly manipulate the data within the command object itself before or after propagating to the delegates.  Commands are pushed into the Framework via static methods which can perform commands on that frame, or after a given delay of frames or seconds.  Commands can push other Commands during their execution giving the ability to create sequences of events.

## Usage:

Clone or pull repository.  Copy Assets/CleaveFramework into the location of your Unity project Assets.  Load Unity.


## Basic Implementation:

The Framework executes based around several simple principals:

 - A single object in your Unity scenes contains the `Framework` component attached to it.  This object must exist in every scene.
 - A component is implemented with the name `<YourScene>SceneView`.  For example: a `GameSceneView` component is expected when initializing a scene named `Game`.
 - Your SceneView component is derived from the `CleaveFramework.Scene.SceneView` object.
 - Objects are pushed into the framework in your derived `SceneView.Initialize()` implementation through the exposed `SceneObjects` instance of `SceneObjectsData`.

## Objects Overview:

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
 
## Interfaces Overview:

### IInitializable 

SceneObjects implementing this interface will have `Initialize()` invoked on them at the point in which you call `SceneObjects.InitializeSceneObjects()` -- you should call this method at the end of your `SceneView.Initialize()` implementation

### IConfigureable 

SceneObjects implementing this interface will have `Configure()` invoked on them immediately after all SceneObjects have been completely initialized.  At this point you are now able to bind any references to any initialized object.  For example: in the case of a View object added to your scene hierarchy in UnityEditor mode and has it's instance resolved during Initialize.

### IUpdateable 

SceneObjects implementing this interface will have `Update(deltaTime)` invoked on them during the SceneView object's update cycle with `Time.deltaTime` as the parameter.


### IDestroyable

SceneObjects implementing this interface will have `Destroy()` invoked on them at the point in which the `OnDestroy()` method on your SceneView is being called by the UnityEngine.
 


## Dependency Injector:

Dependency Injector is an optional implementation for you to use if you desire.  It is able to automatically provide objects with resolved dependencies before the post-instantiation Constructor is invoked.

This version of the Dependency Injector only supports Property and Field injection.  In the future we will also support Constructor injection but it is not available yet.

### [Inject] Attribute
[Inject] is a C# attribute which precedes the object you want the Injector to inject for you.
We can inject into a C# object like:
```csharp
// as a field
[Inject] public IFooSystem MyFooField;
// or as a property
[Inject] public IFooSystem MyFooProperty {get;set;}
```
Note: Injecting into MonoBehaviours is perfectly valid however you must Inject as a Field only.  Attempting to Inject a Property into a MonoBehaviour will compile but throw an assert at runtime.

### Singleton types:
Singleton types are very basic.  To use a singleton type you are required to first create an instance of an implementation of your type and then feed it into the Injector.  All objects which hold a reference to this type marked for injection will be mapped to this exact instance of your implementation.

##### Give the Injector a singleton instance:
```csharp
// first create an implementation of FooSystem that implements IFooSystem
var myFooSystem = Factory.Create<FooSystem>(ConstructFooSystem) as FooSystem;
// bind an implementation of a singleton to an interface:
Injector.AddSingleton<IFooSystem>(myFooSystem);
// or just bind it to a concrete implementation (same thing as above just less extensible and flexible)
Injector.AddSingleton<FooSystem>(myFooSystem);
```
##### Define an object that injects a FooSystem:
```csharp
// define our object
class ObjectA : IInitializable {
    // precede injectable types with the [Inject] attribute
    [Inject] public IFooSystem AFooSystem {get; set;} // Field or Property injection is valid here
    void Initialize() {
        AFooSystem.SomeMethod(); // AFooSystem is resolved here automagically assuming ObjectA was created by Factory
    }
}
```
##### Instantiate an ObjectA:
```csharp
// tell Factory to make you an ObjectA and have it run ConstructObjectA on it
var myA = Factory.Create<ObjectA>(ConstructObjectA);
// define constructor:
object ConstructObjectA(object obj)
{
    var objA = obj as ObjectA;
    objA.AFooSystem.SomeMethod(); // AFooSystem is resolved here already and we can use it if we need to
    return objA;
}
```
### Transient types:
Transient types are also quite basic.  The difference between a singleton and a transient is when you define a singleton you give it an implementation but when you define a transient you give it a type.  The injector will then create a brand new instance of that type when it injects.  Transient types can define default constructors through the Factory just like any other object which will be run before injection takes place.

##### Give the injector a transient type:
```csharp
// if we don't implement any interface we can just give it the type
Injector.AddTransient<FooSystem>();
// or we can bind an implementation to an interface:
Injector.AddTransient<IFooSystem>(typeof(FooSystemImpl));
```
That's all we need to know about the difference between Singleton and Transient types.
Object definition and instantiation is identical to the definition and instantiation of ObjectA above.  The object doesn't care and shouldn't know whether it is receiving a new instance or a previously instantiated instance of FooSystem.  

## Factory:

Factory is a generic factory object which is optional for you to use if you desire.  It is able to provide the object or MonoBehaviour component with a post-instantiation Construction step via delegate.

### Factory Usage:

##### Define a constructor for an object type:
```csharp
private object ConstructDefaultFoo(object obj) {
    var foo = obj as Foo;
    // use some dependency that has already been injected by the Injector:
    foo.InjectedDependency.SomeMethod(); 
    // if you don't want to use the Injector then you can resolve dependencies here however you want for ex (assuming SceneObjects):
    foo.Dependency = SceneObjects.ResolveSingleton<SystemA>(); 
    foo.SomeMethod(); // call an internal method on foo if you need to
    var system = SceneObjects.ResolveSingleton<SystemB>() as SystemB;
    system.SomeMethod(foo); // tell some object about foo
    Framework.PushCommand(new FooCommand(foo)); // tell anyone who wants to know all about foo
    return foo;
}
```
##### Set a default object constructor:
```csharp
Factory.SetConstructor<Foo>(ConstructDefaultFoo);
// remove default constructor if you want:
Factory.SetConstructor<Foo>(null);
// or set it to something else:
Factory.SetConstructor<Foo>(ConstructNonDefaultFoo);
```
##### Make a Foo using previous set constructor:
```csharp
var newFoo = Factory.Create<Foo>() as Foo;
// or resolve and construct a Foo component already attached to a GameObject in one step from a GameObject's name:
var fooComponent = Factory.ConstructMonoBehaviour<Foo>("FoosGameObject") as Foo;
// or pass in the container GameObject directly:
var fooComponent = Factory.ConstructMonoBehaviour<Foo>(FoosGameObject) as Foo;
// or if Foo is a non attached MonoBehaviour we can attach it as a component to a GameObject:
var fooComponent = Factory.AddComponent<Foo>(FoosGameObject) as Foo;
```
##### Make a Foo using an alternate non-default constructor:
```csharp
var newFoo = Factory.Create<Foo>(ConstructDifferentFoo) as Foo;
// or as a component:
var fooComponent = Factory.ConstructMonoBehaviour<Foo>("FoosGameObject", ConstructDifferentFoo) as Foo;
// attaching a component:
var fooComponent = Factory.AddComponent<Foo>(FoosGameObject, ConstructDifferentFoo) as Foo;
```
##### Make a singleton Foo and place it into the SceneObjects framework:
```csharp
var newFoo = Factory.Create<Foo>(SceneObjects) as Foo;
// note this is the same (just less typing and less error prone) as doing:
var newFoo = SceneObjects.PushObjectAsSingleton((Foo)Factory.Create<Foo>()) as Foo;
// or as a component:
var fooComponent = Factory.ConstructMonoBehaviour<Foo>("FoosGameObject", SceneObjects) as Foo;
// or while adding a component:
var fooComponent = Factory.AddComponent<Foo>(FoosGameObject, SceneObjects) as Foo;
```
##### Make a transient Foo and place it into the SceneObjects framework:
```csharp
var newFoo = Factory.Create<Foo>(SceneObjects, "fooName") as Foo;
// or as a component:
var fooComponent = Factory.ConstructMonoBehaviour<Foo>("FoosGameObject", SceneObjects, "fooName") as Foo;
```

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
 
## Tools:

### CDebug

##### Tell CDebug to log from a type:
```csharp
CDebug.LogThis(typeof(MyType));
```
##### Tell CDebug to log something:
```csharp
CDebug.Log("hello world");
// or attach a GameObject to the console:
CDebug.Log("hello world", someGameObject);
```
##### Assert a value
```csharp
CDebug.Assert(myName.Equals("CleaveTV")); // throws an exception if (!myName.Equals("CleaveTV"))
// or add a msg to the assert:
CDebug.Assert(myName.Equals("CleaveTV"), "myName != CleaveTV");
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
