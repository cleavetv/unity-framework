# CleaveFramework v0.2.5

A Unity3D C# application framework.

## Quick Feature Overview

 * Unity Scene Management
   - Transitional Loading Scenes
   - Can automatically create SceneView context object at runtime through common naming convention.
   - Supports pre-created SceneView context object placed in Scene Hierarchy via UnityEditor
 * POCO Object Management with modular Interface support
   - Global Objects
     * Resolve object instances across your entire application in any scene or class
   - Scene Objects
     * Resolve scene specific object instances in any class
   - Reusable object container
     * Add, remove, or resolve objects of any type at any time.
     * Automatically initialize, update, and destroy objects.
 * Object Creation Factory supports C# Objects and Unity Component Attachments
   - Attach Constructors to MonoBehaviour Components
 * Automated Dependency Injection Module
   - Supports Field or Property injection into POCOs or MonoBehaviours 
      * Bind interfaces to implementations
      * Binding of templated value or reference types to parameterized members
 * Global Application level Executable Command Queuing System
   - Reuse the Command Framework and Executable Command Queue Object in your own implementations with minimal code.
 * Private Command Subscription for Multi-Delegate Callbacks
 * Generic Data Binding Libraries
   - Bind and Resolve almost any two types together quickly and easily from one object.

Most of these features are optional modules you can use as much or as little of as your project requires.

## License
The code is offered under the [Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0) license which essentially gives you complete freedom for use and distribution.  I do humbly request that if you are using and extending the code that you consider also offering those extensions back.

## Documentation

### [About](https://github.com/cleavetv/unity-framework/blob/master/Docs/About.md)
### [Installation](https://github.com/cleavetv/unity-framework/blob/master/Docs/Installation.md)
### [Core Principals](https://github.com/cleavetv/unity-framework/blob/master/Docs/CorePrincipals.md)
### [Basic Example Usage](https://github.com/cleavetv/unity-framework/blob/master/Docs/BasicUsage.md)
### [Sample Project](https://github.com/cleavetv/unity-framework/blob/master/Docs/CleaveFrameworkSampleProject.md)
### [Framework Objects](https://github.com/cleavetv/unity-framework/blob/master/Docs/FrameworkObjectsTOC.md)
### [Interfaces](https://github.com/cleavetv/unity-framework/blob/master/Docs/Interfaces.md)
### [Quick Snippets](https://github.com/cleavetv/unity-framework/blob/master/Docs/QuickSnippets.md)
### [Contributions](https://github.com/cleavetv/unity-framework/blob/master/Docs/Contributions.md)

## Community

If you've got questions, want to request a feature, report a bug, or show off a project using the framework come talk about it on [reddit](http://www.reddit.com/r/CleaveFramework/)




