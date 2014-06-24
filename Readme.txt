##### CleaveFramework v0.0.3
﻿
The Code in this repository gets merged back from the cleavetv/Architect repository so that repository will almost always 
contain the most up to date version of the Framework code.  It is recommended to check there first although I do try to keep
this as up to date as I can.

A Unity C# game framework.

Utilizes JSON for object serialization.

See generated Doxygen for source documentation.

###### Basic Implementation:

The Framework executes based around several simple principals:

 - A single object in your Unity scenes contains the "Framework" component attached to it.  This object must exist in
 every scene.
 - A component is implemented with the name <YourScene>SceneView.  For example:  a GameSceneView component
 is expected when initializing a scene named Game.
 - Your SceneView component is derived from the CleaveFramework.Scene.SceneView object.
 
###### Usage:

###### Tools:

 - CDebug object provides a wrapper for Unity's logger with added functionality. 
  - Enable/Disable logging globally
  - Enable/Disable logging per type
