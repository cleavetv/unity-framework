using System;
using CleaveFramework.Commands;
using CleaveFramework.Core;
using UnityEngine;

namespace CleaveFramework.Scene
{

    public sealed class SceneManager
    {
        public static bool IsSceneInitialized { get; private set; }

        public SceneManager()
        {
            Command.Register(typeof(ChangeSceneCmd), OnChangeScene);
            Command.Register(typeof(SceneLoadedCmd), OnSceneLoaded);
        }

        static SceneManager()
        {
            IsSceneInitialized = false;
        }

        static void OnChangeScene(Command cmd)
        {
            var cCmd = (ChangeSceneCmd)cmd;

            GC.Collect();

            IsSceneInitialized = false;

            // enter transition scene
            UnityEngine.Application.LoadLevel(Framework.TransitionScene);
            // load the new scene
            if(UnityEngine.Application.HasProLicense())
                UnityEngine.Application.LoadLevelAsync(cCmd.SceneName);
            else
            {
                UnityEngine.Application.LoadLevel(cCmd.SceneName);
            }
        }

        static void OnSceneLoaded(Command cmd)
        {
            var cCmd = (SceneLoadedCmd) cmd;
            cCmd.View.Initialize();
            IsSceneInitialized = true;
            cCmd.View.SceneObjects.InitializeSceneObjects();
        }

        /// <summary>
        // load SceneView dynamically
        // expects <LevelName>SceneView object for ex:
        // a scene named "Game" would expect a SceneView component named "GameSceneView"
        /// </summary>
        public static void CreateSceneView()
        {
            if (GameObject.Find("SceneView")) return;

            var viewName = Application.loadedLevelName + "SceneView";
            var sceneViewObject = new GameObject("SceneView");
            var sceneViewComponent = sceneViewObject.AddComponent(viewName) as SceneView;

            Framework.PushCommand(new SceneLoadedCmd(sceneViewComponent));
        }

    }

}
