# CmdBinder

CmdBinder is a static object that gives you an interface to bind your Command Types to CommandCallbacks.  CmdBinder is useless without the [Commands](CommandObject.md) that utilize it so understanding Command is very important to understanding how the CmdBinder functions.

For the purposes of this page lets assume that a few Commands exists defined like so:
```csharp
// this basic cmd takes a value an int holds it
class BasicCmd : Command {
	public int BasicValue { get; private set; }
	public BasicCmd(int value) {
		BasicValue = value;
	}
}
// this basic cmd takes an int, multiplies it by 100 and then invokes its callbacks
class ValueSharedCmd : Command {
	public int ModifiedValue { get; private set; }
	public ValueSharedCmd(int value) {
		ModifiedValue = value;
	}
	public override void Execute() {
		ModifiedValue *= 100;
		base.Execute();
	}
}
```

## CommandCallback

`CommandCallback` is defined as:
```csharp
public delegate void CommandCallback(Command cmd);
```

## An object that implements listeners:

The main purpose of the CmdBinder is to bind the callbacks to these types, here's how:

```csharp
class SomeListeningObject : IDestroyable {
	// add bindings in constructor
	public SomeListeningObject() {
		CmdBinder.AddBinding<BasicCmd>(OnBasic);
		CmdBinder.AddBinding<ValueSharedCmd>(OnValueShared);
	}
	
	// implements IDestroyable
	public void Destroy() {
		CmdBinder.RemoveBinding<BasicCmd>(OnBasic);
		CmdBinder.RemoveBinding<ValueSharedCmd>(OnValueShared);
	}
	
	private void OnBasic(Command c) {
		var cmd = c as BasicCmd;
		CDebug.Log(cmd.BasicValue.ToString()); // whatever you want to do
	}
	
	private void OnValueShared(Command c) {
		var cmd = c as ValueSharedCmd;
		CDebug.Log(cmd.ModifiedValue.ToString());
	}
}	
```
