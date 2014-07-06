# CleaveFramework v0.2.0

A Unity3D C# application framework.

## Why you use a framework

A good application framework allows it's users to save time by using generic code that allows them to focus on more project-specific areas.  

A great application framework provides it's users with excellent, fast, well tested, and well maintained generic modules capable of providing their application with a structure they can depend on.

Every piece of software you've even worked with has utilized a framework in one way or another.  Whether it was home-rolled or an off the shelf solution the very structure of your code base makes up the framework it executes on.  If you've ever finished, or worse started and not finished, a project using the Unity3D engine you realize the importance of well structuring your code.  If find yourself re-implementing the same functionality from project to project time and time again then you are a great candidate for migrating your work-flow into a framework package.

## What CleaveFramework can do for you

If you've ever written code that looks like:

```csharp
TheGlobals.GameManager.Game.WorldManager.World.SpawnPlayer(TheGlobals.GameManager.Game.PlayerManager.MakeNewPlayer());
TheGlobals.GameManager.Game.HUDManager.ShowWelcomeMsg(TheGlobals.GameManager.Game.PlayerManager.Player.Name);
TheGlobals.GameManager.Game.Sounds.PlaySound(GameSounds.WelcomeSound);
// etc...
```

Or worse yet: you dream of one day writing code that looks that clean.  You're in dire need of a better structure and work-flow.

You could write this snippet utilizing framework features in one line:
```csharp
Framework.Dispatch(new SpawnNewPlayerCmd());
```
You'll have to read further to see how powerful the Command Dispatching system is, and hopefully that alone has got you interested.

The framework was created to assist in creating and maintaining a better structure for you by implementing several core features.  However most of the framework itself remains modular and you can pick and choose how much or little of it you want to use and how you want to use it.

The framework will not constrain you in to working within one particular design paradigm.  Bits and pieces of it's implementation can be constructed together to form a flow that makes the best sense for you and your project.

What the framework is not is perfect.  We need your help and feedback to make this a great usable piece of software that programmers of all skill levels can use, learn from, and make great games with.

## Documentation

### [About](https://github.com/cleavetv/unity-framework/blob/master/Docs/About.md)
### [Installation](https://github.com/cleavetv/unity-framework/blob/master/Docs/Installation.md)
### [Core Principals](https://github.com/cleavetv/unity-framework/blob/master/Docs/CorePrincipals.md)
### [Basic Example Usage](https://github.com/cleavetv/unity-framework/blob/master/Docs/BasicUsage.md)
### [Sample Project](https://github.com/cleavetv/unity-framework/blob/master/Docs/CleaveFrameworkSampleProject.md)
### [Framework Objects](https://github.com/cleavetv/unity-framework/blob/master/Docs/FrameworkObjectsTOC.md)
### [Interfaces](https://github.com/cleavetv/unity-framework/blob/master/Docs/Interfaces.md)
### [Quick Snippets](https://github.com/cleavetv/unity-framework/blob/master/Docs/QuickSnippets.md)
