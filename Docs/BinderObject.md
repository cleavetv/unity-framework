# Binder

Binder is a generic binding wrapper for C#'s Dictionary.

## Make a binder:

```csharp
// make a binder that binds ints to strings:
var intToString = new Binder<int, string>();
// make a binder that binds strings to ints:
var stringToInt = new Binder<string, int>();

## Add some bindings:

``csharp
intToString.Bind(11, "eleven");
intToString.Bind(31, "thirty-one");
stringToInt.Bind("eleven", 11);
stringToInt.Bind("thirty-one", 31);
```

## Check a binding:

```csharp
if(intToString.IsBound(11)) {
	// do something
}
```

## Resolve a binding:

```csharp
if(intToString.IsBound(11)) {
	CDebug.Log(intToString.Resolve(11)); // this prints "eleven"
}
if(stringToInt.IsBound("eleven")) {
	CDebug.Log(stringToInt.Resolve("eleven"); // this prints 11
}
```

