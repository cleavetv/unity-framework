# EngineOptions

EngineOptions is a basic container for common settings you might apply to a game.  For example:  Width and Height of the screen, Fullscreen, Antialiasing quality, and so forth.  Feel free to customize your own implementation of EngineOptions to use whatever is relevant to your game.  Or alternatively you can completely ignore it and implement your own system for engine settings.  

## Command Type listeners

### ApplyOptionsCmd

When this Command Type is pushed the EngineOptions object reacts by invoking it's OnApplyOptions() method which writes the current options configuration to disk.