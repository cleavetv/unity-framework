# CDebug
CDebug is a static object that provides some basic debugging assistance.

### Assert
CDebug provides basic Assert on boolean values.  Assert is a great way for objects to validate values before proceeding, inform the programmer when values are not what they are expected to be at runtime, and immediately stop the editor and let you jump right to the point in code where the assert took place.  Asserts are only compiled in to code that executes from the UnityEditor.

##### Assert a value
```csharp
CDebug.Assert(!myName.Equals("CleaveTV")); // throws an exception if (!myName.Equals("CleaveTV"))
// or add a msg to the assert:
CDebug.Assert(!myName.Equals("CleaveTV"), "myName != CleaveTV");
```

### Logging
CDebug wraps UnityEngine.Debug.Log() functionality behind a verbosity filter.

#### Verbosity settings
```csharp
public enum ConsoleLogMethod
{
	Silent, // no log calls recognized
	Selected, // recognize log calls made from types registered with LogThis(type)
	Verbose, // all log calls recognized
}
```
#### Set verbosity
```csharp
CDebug.DisplayMethod = ConsoleLogMethod.Verbose;
```
#### Log
```csharp
CDebug.Log(string);
CDebugLog(string, GameObject);
```
#### Selected type logging
When DisplayMethod is set to Selected the Log method will ignore all calls to it made from types not provided to the CDebugger.  This can be handy for quickly enabling or disabling lots of calls to the Logger from types that you don't care about at once.
##### To Add a Type:
```csharp
CDebug.LogThis<ClassName>();
```