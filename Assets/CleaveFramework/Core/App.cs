using CleaveFramework.Commands;
using UnityEngine;

namespace CleaveFramework.Core
{
    /// <summary>
    /// App is the Model/Controller for the Application data
    /// </summary>
    public sealed class App
    {
        public EngineOptions Options { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diskAccess">If diskAccess is false EngineOptions will initialize to it's default values
        /// and never write or read itself from the disk (for web platform)</param>
        public App(bool diskAccess)
        {
            // initialize UnityEngine from Options
            Options = new EngineOptions(diskAccess);
            CmdBinder.AddBinding<ApplyOptionsCmd>(OnApplyOptions);
        }

        void OnApplyOptions(Command cmd)
        {
            Screen.SetResolution(Options.Width, Options.Height, Options.FullScreen);
            // TODO: apply options as necessary
        }
    }
}
