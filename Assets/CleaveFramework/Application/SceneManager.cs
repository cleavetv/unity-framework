using UnityEngine;

namespace CleaveFramework.Application
{
    public class SceneManager
    {
        public SceneManager()
        {
            Command.Register(typeof(ChangeSceneCmd), OnChangeScene);
        }

        public void OnChangeScene(Command cmd)
        {
            var cCmd = (ChangeSceneCmd)cmd;
            // enter transition scene
            UnityEngine.Application.LoadLevel(AppManager.TransitionScene);
            // load the new scene
            UnityEngine.Application.LoadLevelAsync(cCmd.SceneName);
        }
    }
}
