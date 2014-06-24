##### CleaveFramework v0.0.3
﻿
The Code in this repository gets merged back from the cleavetv/Architect repository so that repository will almost always 
contain the most up to date version of the Framework code.  It is recommended to check there first although I do try to keep
this as up to date as I can.

A Unity C# game framework.

###### Basic Implementation:

The Framework executes based around several simple principals:

 - A single object in your Unity scenes contains the "Framework" component attached to it.  This object must exist in
 every scene.
 - A component is implemented with the name <YourScene>SceneView.  For example:  a GameSceneView component
 is expected when initializing a scene named Game.
 - Your SceneView component is derived from the CleaveFramework.Scene.SceneView object.
 - Objects are instantiated in your derived SceneView::Initialize() implementation through the exposed SceneObjects
 instance
 
###### Interfaces:

 - IInitializable : SceneObjects implementing this interface will have Initialize() invoked on them at the point in which
 you call SceneObjects.InitializeSceneObjects() -- you should call this method at the end of your SceneView::Initialize()
 implementation
 - IConfigureable : SceneObjects implementing this interface will have Configure() invoked on them immediately after all
 SceneObjects have been completely initialized.  At this point you are now able to bind any references to any initialized
 object.  For example: in the case of a View object added to your scene hierarchy in UnityEditor mode and has it's instance
 resolved during Initialize.
 - IUpdateable : SceneObjects implementing this interface will have Update(deltaTime) invoked on them during the SceneView
 object's update cycle with Time.deltaTime as the parameter.
 - IDestroyable : SceneObjects implementing this interface will have Destroy() invoked on them at the point in which the
 OnDestroy() method on your SceneView is being called by the UnityEngine.
 
###### Objects:

 - Framework : The Framework object itself
 - Command : abstract object implements basic event listening callbacks
 - EngineOptions : A generic options structure containing settings for things like screen resolution, volumes, and
 rendering qualities.
 - App : Currently functions a container object for the EngineOptions
 - CommandQueue : Contains and processes Command objects pushed to the Framework
 - View : abstract object derived from MonoBehaviour
 - SceneManager : implements basic scene switching functionality
 - SceneObjectData : implements generic containers for objects which live inside the Unity Scene
 - SceneView : abstract object derived from View which holds SceneObjectData
 - CDebug : basic debugging tool (see tools)
 
###### How To:

 - Change a scene:
 Framework.PushCommand(new ChangeSceneCmd(<SceneName>));
 - Apply options and write the configuration to disk:
 Framework.PushCommand(new ApplyOptionsCmd());
 - Implement a custom command:
 class MyCustomCommand<T> : Command
 {
    public T Data;
    public MyCustomCommand(T data) {
       Data = data;
    }
 }
 - Push your custom command:
 Framework.PushCommand(new MyCustomCommand<int>(42));
 - Implement a custom command listener:
 Command.Register(typeof(MyCustomCommand<int>), OnCustomCommand);
 void OnCustomCommand(Command c)
 {
    var cmd = c as MyCustomCommand<int>;
    // cmd.Data = 42 here and you can use it as you wish...
 }
 
###### Tools:

 - CDebug object provides a wrapper for Unity's logger with added functionality. 
  * Enable/Disable logging globally
  * Enable/Disable logging per type
