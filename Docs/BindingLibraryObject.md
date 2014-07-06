# BindingLibrary

BindingLibrary is a collection of Binders pre-mapped to important types you can use immediately

## Make a BindingLibrary

```csharp
var myLibrary = new BindingLibrary();
```

## Bind and resolve some stuff

```csharp
// bind some basic stuff
myLibrary.Bind(11, "eleven");
CDebug.Log(myLibrary.Resolve(11)); // prints "eleven"
myLibrary.Bind("eleven", 11);
CDebug.Log(myLibrary.Resolve("eleven")); // prints 11
myLibrary.Bind(FruitsEnum.Orange, "Oranges taste delicious");
CDebug.Log(myLibrary.Resolve(FruitsEnum.Orange)); // prints "Oranges taste delicious"

// lets try something complicated
// define a delegate signature
public delegate void Functor();

// make one
Functor Funcs = null;
// give it some lambdas
Funcs += () => CDebug.Log("Funcs(1)");
Funcs += () => CDebug.Log("Funcs(2)");
// bind it
myLibrary.Bind("functor", Funcs);
// resolve it 
var f = myLibrary.Resolve("functor") as Functor;
// execute it
f(); // This prints "Funcs(1)" & "Funcs(2)"
// add a third lambda
f += () => CDebug.Log("Funcs(3)");
// rebind it
myLibrary.Bind("functor", f);
// resolve it again
var anotherF = myLibrary.Resolve("functor") as Functor;
// execute it 
anotherF(); // This prints "Funcs(1)" & "Funcs(2)" & "Funcs(3)"
```