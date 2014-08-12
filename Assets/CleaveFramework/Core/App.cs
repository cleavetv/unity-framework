using CleaveFramework.Commands;
using CleaveFramework.DependencyInjection;
using UnityEngine;

namespace CleaveFramework.Core
{
    /// <summary>
    /// App is the Model/Controller for the Application data
    /// </summary>
    public sealed class App
    {
        //public EngineOptions Options { get; private set; }

        [Inject] public EngineOptions Options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diskAccess">If diskAccess is false EngineOptions will initialize to it's default values
        /// and never write or read itself from the disk (for web platform)</param>
        public App()
        {
            CmdBinder.AddBinding<ApplyOptionsCmd>(OnApplyOptions);
        }

        void OnApplyOptions(Command cmd)
        {
            Screen.SetResolution(Options.Width, Options.Height, Options.FullScreen);
            QualitySettings.vSyncCount = Options.Vsync ? 1 : 0;
            // TODO: apply options as necessary
        }
    }
}
