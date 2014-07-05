# CmdBinder

CmdBinder is a static object that gives you an interface to bind your Command Types to CommandCallbacks.  CmdBinder is useless without the [Commands](CommandObject.md) that utilize it so understanding Command is very important to understanding how the CmdBinder functions.

For the purposes of this page lets assume that a few Commands exists defined like so:
```csharp
// this basic cmd takes a value an int holds it
class BasicCmd {
	public int BasicValue { get; private set; }
	public BasicCmd(int value) {
		BasicValue = value;
	}
}
// this basic cmd takes an int, multiplies it by 100 and then invokes its callbacks
class ValueSharedCmd {
	public int ModifiedValue { get; private set; }
	public ValueSharedCmd(int value) {
		ModifiedValue = value;
	}
	public override void Execute() {
		ModifiedValue *= 100;
		base.Execute();
	}
}
// this basic cmd takes an initial value, invokes its callbacks which have the capability of modifying the value, then pushes a new BasicCmd with the resulting value
class ValueReturnedCmd {
	public int ModifiedValue { get; set; }
	public ValueReturnedCmd(int value) {
		ModifiedValue = value;
	}
	public override void Execute() {
		base.Execute();
		Framework.PushCommand(new BasicCmd(ModifiedValue));
	}
}
```

## Defining a CommandCallback

`CommandCallback` is defined as:
```csharp
public delegate void CommandCallback(Command cmd);
```

An object can then define a callback to a specific Command with the following syntax:

```csharp
private void OnBasic(Command c) {
	var cmd = c as BasicCmd;
	CDebug.Log(cmd.BasicValue.ToString()); // whatever you want to do
}
private void OnValueShared(Command c) {
	var cmd = c as ValueSharedCmd;
	CDebug.Log(cmd.ModifiedValue.ToString());
}
private void OnValueReturned(Command c) {
	var cmd = c as ValueReturnedCmd;
	cmd.ModifiedValue *= 100;
}
```

## Binding and Unbinding Callbacks to and from types

The main purpose of the CmdBinder is to bind the callbacks to these types, here's how:

class SomeListeningObject : IDestroyable {
	// add bindings in constructor
	public SomeListeningObject() {
		CmdBinder.AddBinding<BasicCmd>(OnBasic);
		CmdBinder.AddBinding<ValueSharedCmd>(OnValueShared);
		CmdBinder.AddBinding<ValueReturnedCmd>(OnValueReturned);
	}
	
	// implements IDestroyable
	public void Destroy() {
		CmdBinder.RemoveBinding<BasicCmd>(OnBasic);
		CmdBinder.RemoveBinding<ValueSharedCmd>(OnValueShared);
		CmdBinder.RemoveBinding<ValueReturnedCmd>(OnValueReturned);
	}
		
