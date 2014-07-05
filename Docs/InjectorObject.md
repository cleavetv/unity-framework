# Dependency Injector:

Dependency Injector is able to automatically provide objects with resolved dependencies before the post-instantiation Constructor is invoked.

The Injector currently only supports Property and Field injection.

## [Inject] Attribute
[Inject] is a C# attribute which precedes the object you want the Injector to inject for you.
We can inject into a C# object like:
```csharp
// as a field
[Inject] public IFooSystem MyFooField;
// or as a property
[Inject] public IFooSystem MyFooProperty {get;set;}
```
Note: Injecting into MonoBehaviours is perfectly valid however you must Inject as a Field only.  Attempting to Inject a Property into a MonoBehaviour will compile but throw an assert at runtime.

## Injection Types
Injection of two different variable types is supported:  Singleton and Transient.  

The difference between a singleton and a transient is when you define a singleton you give it an instance of an object but when you define a transient you give it a type of an object.  The injector will then create a brand new instance of that type when it injects.  Transient types can define default constructors through the Factory just like any other object which will be run before injection takes place.

### Singleton types:
To use a singleton type you are required to first create an instance of an implementation of your type and then feed it into the Injector.  All objects which hold a reference to this type marked for injection will be mapped to this exact instance of your implementation.

##### Give the Injector a singleton instance:
```csharp
// first create an instance of a FooSystem that implements IFooSystem
var myFooSystem = Factory.Create<FooSystem>(ConstructFooSystem) as FooSystem;
// bind an instance of a singleton to an interface:
Injector.AddSingleton<IFooSystem>(myFooSystem);
// or just bind it to a concrete implementation (same thing as above just less extensible and flexible)
Injector.AddSingleton<FooSystem>(myFooSystem);
```

### Transient types:
To use a transient type we use the Injector to map a type to an implementation.  A type can be a concrete implementation type or an interface.

##### Give the injector a transient type:
```csharp
// if we don't implement any interface we can just give it the type.
// Any object looking to inject FooSystem will receive a new FooSystem()
Injector.AddTransient<FooSystem>();
// or we can bind an implementation to an interface:
// Any object looking to inject an IFooSystem will receive a new FooSystemImpl()
Injector.AddTransient<IFooSystem, FooSystemImpl>();
```

##### Define an object that injects a FooSystem:
```csharp
// define our object
class ObjectA : IInitializable {
    // precede injectable types with the [Inject] attribute
    [Inject] public IFooSystem AFooSystem {get; set;} // Field or Property injection is valid here
    void Initialize() {
        AFooSystem.SomeMethod(); // AFooSystem is resolved here already assuming ObjectA was created by Factory
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

## Pattern notes:
Binding to interfaces is a very powerful technique which can and should be taken advantage of where ever possible.  This gives your code the flexibility to change between concrete implementation types at your discretion without modifying the requesting objects or their usage of the implementation.