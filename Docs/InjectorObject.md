# Dependency Injector:

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
// first create an instance of a FooSystem that implements IFooSystem
var myFooSystem = Factory.Create<FooSystem>(ConstructFooSystem) as FooSystem;
// bind an instance of a singleton to an interface:
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
Transient types are also quite basic.  The difference between a singleton and a transient is when you define a singleton you give it an instance of an object but when you define a transient you give it a type of an object.  The injector will then create a brand new instance of that type when it injects.  Transient types can define default constructors through the Factory just like any other object which will be run before injection takes place.

##### Give the injector a transient type:
```csharp
// if we don't implement any interface we can just give it the type
Injector.AddTransient<FooSystem>();
// or we can bind an implementation to an interface:
Injector.AddTransient<IFooSystem>(typeof(FooSystemImpl));
```
That's all we need to know about the difference between Singleton and Transient types.
Object definition and instantiation is identical to the definition and instantiation of ObjectA above.  The object doesn't care and shouldn't know whether it is receiving a new instance or a previously instantiated instance of FooSystem.  