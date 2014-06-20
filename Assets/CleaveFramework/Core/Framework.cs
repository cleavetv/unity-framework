using CleaveFramework.Scene;
using CleaveFramework.Tools;
using UnityEngine;
using System.Collections;

namespace CleaveFramework.Core
{
    /// <summary>
    /// AppManager is the main script that will register with a Unity GameObject
    /// the AppManager view object MUST be in every scene that will execute the framework
    /// </summary>
    public sealed class Framework : MonoBehaviour
    {
        /// <summary>
        /// name of the scene to use while transitioning into the next scene
        /// </summary>
        [SerializeField] private string _transitionScene = "Loading";
        public static string TransitionScene
        {
            get { return Instance._transitionScene; }
        }

        /// <summary>
        /// singleton instance
        /// </summary>
        public static Framework Instance { get; private set; }

        private static CommandQueue _commands;

        public static SceneObjectData FrameworkObjects { get; private set; }

        void Awake()
        {
            CDebug.DisplayMethod = CDebug.ConsoleLogMethod.Selected;

            if (Instance != this && Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                _commands = new CommandQueue();

                // create framework manager objects
                FrameworkObjects = new SceneObjectData();
                FrameworkObjects.PushObjectAsSingleton(new SceneManager());
                FrameworkObjects.PushObjectAsSingleton(new App());

                StartCoroutine(ProcessCommands());
                //var thread = new System.Threading.Thread(ProcessCommands);

                DontDestroyOnLoad(gameObject);
            }

            // load SceneView dynamically
            // expects <LevelName>SceneView object for ex:
            // a scene named "Game" would expect a SceneView component named "GameSceneView"
            SceneManager.CreateSceneView(Application.loadedLevelName + "SceneView");

        }

        void Update()
        {
            FrameworkObjects.Update(Time.deltaTime);
        }

        static IEnumerator ProcessCommands()
        {
            while (true)
            {
                _commands.Process();
                yield return null;
            }
        }

        /// <summary>
        /// push a command with no delay into the command queue
        /// </summary>
        /// <param name="cmd">the command</param>
        public static void PushCommand(Command cmd)
        {
            _commands.Push(cmd);
        }

        /// <summary>
        /// push a command with a frame count execution delay
        /// </summary>
        /// <param name="cmd">the command</param>
        /// <param name="frameDelay"># of frames to wait until command will be executed</param>
        public static void PushCommand(Command cmd, int frameDelay)
        {
            _commands.Push(cmd, frameDelay);
        }

        /// <summary>
        /// push a command with a timed execution delay
        /// </summary>
        /// <param name="cmd">the command</param>
        /// <param name="timeDelay"># of seconds to wait until command will be executed</param>
        public static void PushCommand(Command cmd, float timeDelay)
        {
            _commands.Push(cmd, timeDelay);
        }
    }
}