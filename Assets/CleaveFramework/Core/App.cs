using CleaveFramework.Commands;
using UnityEngine;

namespace CleaveFramework.Core
{
    /// <summary>
    /// App is the Model/Controller for the Application data
    /// </summary>
    public class App
    {
        public EngineOptions Options { get; private set; }

        public App()
        {
            // initialize UnityEngine from Options
            Options = new EngineOptions();
            Command.Register(typeof(ApplyOptionsCmd), OnApplyOptions);
        }

        protected virtual void OnApplyOptions(Command cmd)
        {
            Screen.SetResolution(Options.Width, Options.Height, Options.FullScreen);
            // TODO: apply options as necessary
        }
    }
}
