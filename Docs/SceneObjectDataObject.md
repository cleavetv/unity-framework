# SceneObjectData

The SceneObjectData is a container for objects created at runtime.  The container is what provides the invocation of the framework [Interfaces](Interfaces.md) available so any object you want to have the interface methods automatically invoked on needs to be inside of a SceneObjectData container.  

##  `InitializeSceneObjects()`

Upon calling the `InitializeSceneObjects()` method it will iterate the objects in its containers reflecting on the [interfaces](Interfaces.md) implemented.  It will then set its internal state to initialized, further calls to `InitializeSceneObjects()` will be ignored.

## Adding objects after calling `InitializeSceneObjects()`

This is valid to do, if your object implements `IInitializeable` or `IConfigureable` it will be invoked in order after being added to the container.

## Updating SceneObjectData

Calling `Update(deltaTime)` on the SceneObjectData instance will invoke the `Update(deltaTime)` method on all contained objects implementing the `IUpdateable` interface.

## Destroying SceneObjectData

Calling `Destroy()` on the SceneObjectData will invoke the `Destroy()` method on all contained objects implementing the `IDestroyable` interface.  It will also clear all the containers and reset the initialization state of the object to false allowing you to re-use it with new object data if you want to.

## SceneDataObject contained object types

For the following examples assume we have access to a SceneObjectData defined like:
```csharp
public SceneObjectData SceneObjects;
```

### Singleton

A SceneObjects Singleton type is a 1-to-1 mapping of a System.Type to an instance of that type.  

##### A singleton object can only ever have one instance of it's type in the library.  Attempting to place a second instance of a singleton into the library will OVERWRITE the previous instance.  This can have extremely undesirable effects or could be exactly what you were looking for, it all depends on your situation.

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

##### A transient object can have unlimited amounts of instances of it's type in the library.  Transients are differentiated between each other by their "name" property.  Type/Name combinations however must be UNIQUE and the SceneObjects will assert if you attempt to place a second object of the same type and name into it.

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

