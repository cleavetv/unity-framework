# Interfaces Overview:

### IInitializable 

SceneObjects implementing this interface will have `Initialize()` invoked on them after your `SceneView.Initialize()` method has returned.

### IConfigureable 

SceneObjects implementing this interface will have `Configure()` invoked on them immediately after all SceneObjects have had `Initialize()` invoked on them.

### IUpdateable 

SceneObjects implementing this interface will have `Update(deltaTime)` invoked on them during the SceneView object's update cycle with `Time.deltaTime` as the parameter.

### IDestroyable

SceneObjects implementing this interface will have `Destroy()` invoked on them at the point in which the `OnDestroy()` method on your SceneView is being called by the UnityEngine.