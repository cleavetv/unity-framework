# Command

In CleaveFramework a Command is an extremely simple but powerful concept.  Command objects work very closely with [CmdBinder](CmdBinderObject) so it is important you understand how they both work to get the big picture on the Command system.

## Command Object

The Command object itself is an abstract class meaning no type of it may be created but it can be used as a base class for implementing custom Command Types.
Command implements a single virtual function which you can override: `Execute()`

When you use the Framework.Dispatch() or Framework.PushCommand() methods your command object is placed into a queue and upon reaching the top of that queue its `Execute()` method is...wait for it... executed.  This does one of two things:
 - If you haven't overrode `Execute()` in your Command the base `Command.Execute()` will invoke now which in turn invokes all of the callback methods which are bound to that Command Type through the [CmdBinder](CmdBinderObject.md) with `this` as the parameter.
 - If you've overrode `Execute()` in your implementation of the Command it will be invoked now instead.  It is up to you to make sure you call `base.Execute()` in your method if (and importantly when) you want the callbacks to be invoked.
 
## Some sample Command implementations:
```csharp
// Here's a Command implementation which takes in two integers and multiplies them together before invoking it's callbacks
class IntManipulationCmd {
	public int ManipulatedInt { get; private set; }
	public IntManipulationCmd(int lhs, int rhs) {
		ManipulatedInt = lhs * rhs;
	}
}
// Here's a generic Command that takes a piece of data and stores it
class DataContainerCmd<T> {
	public T MyData {get;private set;}
	public DataContainer<T>(T data) {
		MyData = data;
	}
}
// Here's a more complicated command that takes two systems, performs some work, and stores the result for the callbacks to use.
class DoWorkCmd {
	public ISystemA SysA {get; private set;}
	public ISystemB SysB {get; private set;}
	public IWork WorkResult {get; private set;}
	public DoWorkCmd(ISystemA sysA, ISystemB sysB) {
		SysA = sysA;
		SysB = sysB;
	}
	public override void Execute() {
		WorkResult = SysA().DoWork();
		WorkResult = SysB().DoWorkWithResults(WorkResult);
		// now invoke callbacks who are waiting for the WorkResult
		base.Execute();
	}
}
```