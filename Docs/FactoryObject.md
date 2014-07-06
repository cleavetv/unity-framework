# Factory

Factory is a generic factory object which is optional for you to use if you desire.  It is able to provide the object or MonoBehaviour component with a post-instantiation Construction step via delegate.

## Factory Usage:

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

## Object creation order of operations
### A object instantiated through Factory at runtime goes through the following steps in order:
 - Your object's default constructor is executed (ie: var obj = new MyObject();)
 - Your object is evaluated for [Inject] attributes and existing dependencies are resolved.
 - Your object's Factory defined default constructor is invoked.
 - Your object is added to SceneData if it is provided to the Factory's create method.
  - If your object is added to the Scene Data and the SceneObjectData has already been initialized then: 
    - Your object's Initialize() method is invoked if you have implemented the interface.
    - Your object's Configure() method is invoked if you have implemented the interface.
 - Factory.Create returns your object now.
