# Binder

Binder is a generic binding wrapper for C#'s Dictionary.

## Make a binder:

```csharp
// make a binder that binds ints to strings:
var intToString = new Binder<int, string>();
// make a binder that binds strings to ints:
var stringToInt = new Binder<string, int>();
```

## Add some bindings:

```csharp
// function style
intToString.Bind(11, "eleven");
intToString.Bind(31, "thirty-one");
stringToInt.Bind("eleven", 11);
stringToInt.Bind("thirty-one", 31);

// or with [] operator
intToString[11] = "eleven";
intToString[31] = "thirty-one";
stringToInt["eleven"] = 11;
stringToint["thirty-one"] = 31;
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

## Remove a binding:

```csharp
intToString.Clear(11);
```

## Remove all bindings

```csharp
intToString.Clear();
```

## Reverse resolve, find a list of keys matching a given value

```csharp
var Books = new Binding<string, string>();
var author = "Dr Suess";
Books["Cat In the Hat"] = author;
Books["Green Eggs and Ham"] = author;
Books["The Lorax"] = author;
Books["Hop On Pop"] = author;
author = "Shel Silverstein";
Books["The Giving Tree"] = author;
Books["Where The Sidewalk Ends"] = author;
IEnumerable<string> booksByAuthor = Books.FindKeyMatches("Dr Suess"); // returns 4 books matching Dr Suess

## Size of bindings

```csharp
int binds = Books.Count; // 6
```

