using System;
using CleaveFramework.Commands;
using CleaveFramework.Core;
using CleaveFramework.Tools;
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

        private static SceneView _cachedSceneView = null;

        public SceneManager()
        {
            IsSceneInitialized = false;
            CmdBinder.AddBinding<ChangeSceneCmd>(OnChangeScene);
            CmdBinder.AddBinding<SceneLoadedCmd>(OnSceneLoaded);
            CmdBinder.AddBinding<LoadNextSceneCmd>(OnLoadNextScene);
            Factory.Factory.SetConstructor<SceneView>(ConstructSceneView);
        }

        static void OnLoadNextScene(Command c)
        {
            var cmd = c as LoadNextSceneCmd;

            // load the new scene
            if (UnityEngine.Application.HasProLicense())
                UnityEngine.Application.LoadLevelAsync(cmd.SceneName);
            else
            {
                UnityEngine.Application.LoadLevel(cmd.SceneName);
            }
        }

        static void OnChangeScene(Command c)
        {
            var cmd = c as ChangeSceneCmd;

            GC.Collect();

            // enter transition scene
            Framework.PushCommand(new LoadNextSceneCmd(cmd.SceneName), 1);
            UnityEngine.Application.LoadLevel(Framework.TransitionScene);
        }

        static void OnSceneLoaded(Command c)
        {
            var cmd = c as SceneLoadedCmd;
            _cachedSceneView = cmd.View;
            cmd.View.StartCoroutine("ValidateSceneObjects");
            cmd.View.Initialize();
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

            IsSceneInitialized = false;

            var viewName = Application.loadedLevelName + "SceneView";
            var sceneViewObject = new GameObject("SceneView");
            var sceneViewComponent = Factory.Factory.AddComponent<SceneView>(viewName, sceneViewObject) as SceneView;
            Framework.PushCommand(new SceneLoadedCmd(sceneViewComponent));
        }

        private static object ConstructSceneView(object obj)
        {
            var sceneView = obj as SceneView;
            sceneView.SceneObjects = new SceneObjectData();
            return sceneView;
        }

        public static void ValidateSceneObjects()
        {
            CDebug.Assert(_cachedSceneView == null, "SceneManager.ValidateSceneObjects()");

            IsSceneInitialized = true;
            _cachedSceneView.StopAllCoroutines();
        }
    }

}
