using System;
using CleaveFramework.Commands;
using CleaveFramework.Core;
using UnityEngine;

namespace CleaveFramework.Scene
{

    public sealed class SceneManager
    {
        /// <summary>
        /// IsSceneInitialized will return true once the current SceneView.Initialize() method has returned completely.
        /// You can use this value to make sure a totally encapsulated component object away from the framework doesn't
        /// attempt to use an unresolved dependency inside of the scene before it's ready.
        /// </summary>
        public static bool IsSceneInitialized { get; private set; }

        public SceneManager()
        {
            CmdBinder.AddBinding<ChangeSceneCmd>(OnChangeScene);
            CmdBinder.AddBinding<SceneLoadedCmd>(OnSceneLoaded);
        }

        static SceneManager()
        {
            IsSceneInitialized = false;
        }

        static void OnChangeScene(Command c)
        {
            var cmd = c as ChangeSceneCmd;

            GC.Collect();

            IsSceneInitialized = false;

            // enter transition scene
            UnityEngine.Application.LoadLevel(Framework.TransitionScene);
            // load the new scene
            if(UnityEngine.Application.HasProLicense())
                UnityEngine.Application.LoadLevelAsync(cmd.SceneName);
            else
            {
                UnityEngine.Application.LoadLevel(cmd.SceneName);
            }
        }

        static void OnSceneLoaded(Command c)
        {
            var cmd = c as SceneLoadedCmd;
            cmd.View.Initialize();
            IsSceneInitialized = true;
            cmd.View.SceneObjects.InitializeSceneObjects();
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
