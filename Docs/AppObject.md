# App

App is a container object for the [EngineOptions](EngineOptionsObject.md).  You can access the EngineOptions via:

`Framework.App.EngineOptions`

## Command Type listeners

### ApplyOptionsCmd

When this Command Type is pushed the App object reacts by invoking it's OnApplyOptions where you can call directly to the UnityEngine and make modifications to it's settings.  In the stock version of this object it shows how you can change the Screen Height and Width to the appropriate values.  You should use your own custom implementation of this method which applies options appropriately however your game needs to do so.