# Factory:

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

Note: In the SceneObjects data the difference between a singleton and a transient object is subtle but important:

A singleton object can only ever have one instance of it's type in the library.  Attempting to place a second instance of a singleton into the library will OVERWRITE the previous instance.  This can have extremely undesirable effects or could be exactly what you were looking for, it all depends on your situation.

A transient object can have unlimited amounts of instances of it's type in the library.  Transients are differentiated between each other by their "name" property.  Type/Name combinations however must be UNIQUE and the SceneObjects will assert if you attempt to place a second object of the same type and name into it.