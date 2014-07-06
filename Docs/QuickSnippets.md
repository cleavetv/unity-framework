# Quick Snippets
Some code fragments showing how to do common actions.

##### Change a scene:
```csharp
Framework.PushCommand(new ChangeSceneCmd(<SceneName>));
```
##### Change an option value:
```csharp
Framework.App.Options.FullScreen = false;
```
##### Apply options and write the configuration to disk:
```csharp
Framework.PushCommand(new ApplyOptionsCmd());
```
##### Implement a custom command:
```csharp
class MyCustomCommand<T> : Command
{
    public T Data;
    public MyCustomCommand(T data) {
       Data = data;
    }
}
```
##### Push your custom command:
```csharp
// execute on next Framework's Process()
// With injections
Framework.Dispatch(new MyCustomCommand<int>(42));
// Without injections
Framework.PushCommand(new MyCustomCommand<int>(42));

// delay execution for 3 frames
// With injections
Framework.Dispatch(new MyCustomCommand<int>(42), 3);
// Without injections
Framework.PushCommand(new MyCustomCommand<int>(42), 3);

// delay execution for 5 seconds
// With injections 
Framework.Dispatch(new MyCustomCommand<int>(42), 5.0f);
// Without injections
Framework.PushCommand(new MyCustomCommand<int>(42), 5.0f);
```
##### Implement a custom command listener:
```csharp
// First bind it
CmdBinder.AddBinding<MyCustomCommand<int>>(OnCustomCommand);
// now implement a listener callback
void OnCustomCommand(Command c)
{
   var cmd = c as MyCustomCommand<int>;
   // cmd.Data = 42 here and you can use it as you wish...
}
```
##### Unregister a command listener:
```csharp
CmdBinder.RemoveBinding<MyCustomCommand<int>>(OnCustomCommand);
``` 
##### Pass a data model from one scene to another:
###### As singleton:
```csharp
Framework.Globals.PushObjectAsSingleton(new CustomDataModel());
```
###### As transient:
```csharp
Framework.Globals.PushObjectAsTransient("myCustomData", new CustomDataModel());
```
##### Resolve a data model from the globals:
###### As singleton:
```csharp
var myCustomData = Framework.Globals.ResolveSingleton<CustomDataModel>() as CustomDataModel;
``` 
###### As transient:
```csharp
var myCustomData = Framework.Globals.ResolveTransient<CustomDataModel>("myCustomData") as CustomDataModel;
```
##### Access to SceneView can be done in the standard way you access any Unity component:
```csharp
var sceneView = GameObject.Find("SceneView").GetComponent<SceneView>() as SceneView;
```

