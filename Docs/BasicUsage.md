# Basic Usage

## Your first CleaveFramework Project

Here we will implement some very basic functionality utilizing the framework.  I will not be explaining very much of how or why this works as I will be going into much further depth discussions about the objects in their own individual pages.  This is meant to give you a basic overview of how your code might look using the framework's features.  This is by no means a completely comprehensive view into how you should structure an application, in fact in the interests of brevity I will mix in pseudo-code and take several short-cuts here I don't recommend.  To see an exact practice of implementing the Framework you can take a look [here](https://github.com/cleavetv/Architect).  A more comprehensive sample project is also in the plans.

### Set up some types and commands:
```csharp
// Here we'll define a basic type that stores information about how the Game scene will create the game-play.
class GameTypeConstructor
{
	public enum GameType {
		NewGame,
		LoadGame,
	};
	public GameType myType {get; private set;}
	public string GameName {get; private set;}
	public int NumPlayers {get; private set;}
	
	public GameTypeConstructor(GameType type, string name, int numPlayers)
	{
		myType = type;
		GameName = name;
		NumPlayers = numPlayers;
	}
}
// Here we define a Command type which will fire when the player starts a game from the menu
class StartGameCmd : Command 
{
	public GameTypeConstructor GameType {get; private set;}
	public StartGameCmd(GameTypeConstructor gameType)
	{
		GameType = gameType;
	}
}
```

### Define a Menu
```csharp
// ...assume menu code above here
// assume this code is bound to when the user has pushed a "Start New Game" button via whatever GUI methods you are implementing
void StartNewGameButtonPressed()
{
	// tell the framework to run the StartGameCmd and give it a game type constructor
	Framework.PushCommand(new StartGameCmd(new GameTypeConstructor(GameType.NewGame, "Awesome Game", 1)));
}

```
	
### An Example MainMenuView implementation:
This basic SceneView will attach when MainMenu.Unity is loaded.  All we're doing here is binding a command listener and waiting for it to happen.  Your menu implementation can happen however and where ever you want it to.
```csharp
class MainMenuSceneView : SceneView
{
	public override void Initialize()
	{
		// bind OnStartGame to StartGameCmd
		CmdBinder.AddBinding<StartGameCmd>(OnStartGame);
	}
	
	void OnDestroy()
	{
		CmdBinder.RemoveBinding<StartGameCmd>(OnStartGame);
	}
	
	void OnStartGame(Command c)
	{
		// save our game type constructor in the globals
		StartGameCmd cmd = c as StartGameCmd;
		Framework.Globals.PushSingleton<GameTypeConstructor>(cmd.GameType);
		// switch scenes
		Framework.PushCommand(new ChangeSceneCmd("Game"));
	}
}
```

### An Example GameSceneView implementation:
This SceneView will attach when Game.Unity is loaded.  It takes the previously stored game constructor out of the globals and shows how you could use it to create your game scene.
```csharp
class GameSceneView : SceneView
{
	public override void Initialize()
	{
		// get a previously saved singleton object out of the globals:
		var gameConstructModel = Framework.Globals.ResolveSingleton<GameTypeConstructor>() as GameTypeConstructor;
		// use it to construct your game type:
		switch(gameConstructModel.myType)
		{
			case GameTypeConstructor.GameType.NewGame:
				var numPlayers = gameConstructModel.NumPlayers;
				// etc 
			case GameTypeConstructor.GameType.LoadGame:
				// load a game...
		}
		// etc
	}
}
