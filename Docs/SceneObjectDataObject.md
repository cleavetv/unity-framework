# SceneObjectData

The SceneObjectData is a container for objects created at runtime.  Upon calling the `InitializeSceneObjects()` method it will iterate the objects in its containers reflecting on the [interfaces](Interfaces.md) implemented.

The container functionality is what provides the invocation of the framework [Interfaces](Interfaces.md) available so any object you want to have the interface methods automatically invoked on needs to be inside of a SceneObjectData container.  

## Adding objects after calling `InitializeSceneObjects()`

This is valid to do, if your object implements `IInitializeable` or `IConfigureable` it will be invoked in order after being added to the container.

## Updating SceneObjectData

Calling `Update(deltaTime)` on the SceneObjectData instance will invoke the `Update(deltaTime)` method on all contained objects implementing the `IUpdateable` interface.

## Destroying SceneObjectData

Calling `Destroy()` on the SceneObjectData will invoke the `Destroy()` method on all contained objects implementing the `IDestroyable` interface.

## SceneDataObject contained object types

SceneDataObject provides two different types of object containment, Singleton and Transient, which are described below.

For the following examples assume we have access to a SceneObjectData defined like:
```csharp
public SceneObjectData SceneObjects;
```

### Singleton

A SceneObjects Singleton type is a 1-to-1 mapping of a System.Type to an instance of that type.  

#### Adding a singleton
```csharp
// add one specific instance of a Foo
SceneObjects.PushObjectAsSingleton<Foo>(new Foo());
```

#### Resolving the singleton
This will return the instance of Foo type previously given to the SceneObjects.
```csharp
// retrieve it
var myFoo = SceneObjects.ResolveSingleton<Foo>() as Foo;
```

### Transient

A SceneObjects Transient type is a mapping of a System.Type to a Name/Type instance pair.

#### Adding a transient
```csharp
// add a few instances of Foo named Foo0 through Foo4
for(var i = 0; i < 5; i++) {
	SceneObjects.PushObjectAsTransient<Foo>("foo" + i, new Foo());
}
```

#### Resolve a transient
```csharp
// resolve instances of Foo named Foo0 through Foo4
List<Foo> fooList = new List<Foo>();
for(var i = 0; i < 5; i++) {
	fooList.Add((Foo)SceneObjects.ResolveTransient<Foo>("foo" + i));
}
```

