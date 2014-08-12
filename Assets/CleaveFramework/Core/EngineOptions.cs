using System;
using System.IO;
using CleaveFramework.Commands;
using CleaveFramework.DependencyInjection;
using CleaveFramework.Interfaces;
using SimpleJSON;

namespace CleaveFramework.Core
{
    /// <summary>
    /// wrapper for Unity engine configuration options
    /// </summary>
    [Serializable]
    public class EngineOptions : IInitializeable
    {
        private const string ConfigFile = "engine.ini";

        // Rendering options
        public enum Quality
        {
            Ultra,
            High,
            Medium,
            Low,
            Disabled
        }

        // Window options
        public bool FullScreen;
        public bool Vsync;
        public int Width;
        public int Height;

        // Audio options
        public bool PlayMusic;
        public float MusicVolume;
        public bool PlaySfx;
        public float SfxVolume;

        public Quality Antialias;
        public Quality SSAO;
        public Quality MotionBlur;
        public Quality Shadow;
        public Quality Vignette;

        [Inject("Framework_UseDiskAccess")] public bool UseDiskAccess;

        public EngineOptions()
        {
            
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="copy"></param>
        public EngineOptions(EngineOptions copy)
        {
            this.FullScreen = copy.FullScreen;
            this.Vsync = copy.Vsync;
            this.Width = copy.Width;
            this.Height = copy.Height;
            this.PlayMusic = copy.PlayMusic;
            this.MusicVolume = copy.MusicVolume;
            this.PlaySfx = copy.PlaySfx;
            this.Antialias = copy.Antialias;
            this.SSAO = copy.SSAO;
            this.MotionBlur = copy.MotionBlur;
            this.Shadow = copy.Shadow;
            this.Vignette = copy.Vignette;
            this.UseDiskAccess = copy.UseDiskAccess;
        }

        public void Initialize()
        {
            // check for configuration file
            if (!LoadFromConfig())
            {
                CreateDefaultOptions();
            }

            CmdBinder.AddBinding<ApplyOptionsCmd>(OnApplyOptions);
            Framework.PushCommand(new ApplyOptionsCmd());
        }

        /// <summary>
        /// Create the options properties to run an engine from the iniFile
        /// </summary>
        private bool LoadFromConfig()
        {
            if (!UseDiskAccess) return false;

            if (!File.Exists(ConfigFile)) return false;
            var options = JSONNode.LoadFromFile(ConfigFile);
            if (options == null) return false;
            Load(ref options);
            return true;
        }

        private void CreateDefaultOptions()
        {
            FullScreen = false;
            Vsync = false;
            Width = 1280;
            Height = 720;
            PlayMusic = true;
            MusicVolume = 1f;
            PlaySfx = true;
            SfxVolume = 1f;
            Antialias = Quality.Medium;
            SSAO = Quality.Medium;
            MotionBlur = Quality.Medium;
            Shadow = Quality.Medium;
            Vignette = Quality.Medium;
        }

        private void WriteConfig()
        {
            if (!UseDiskAccess) return;

            var options = Save();
            options.SaveToFile(ConfigFile);
        }

        void OnApplyOptions(Command cmd)
        {
            WriteConfig();
        }

        JSONNode Save()
        {
            var options = new JSONClass();
            options["FullScreen"].AsBool = FullScreen;
            options["Vsync"].AsBool = Vsync;
            options["Width"].AsInt = Width;
            options["Height"].AsInt = Height;
            options["PlayMusic"].AsBool = PlayMusic;
            options["MusicVolume"].AsFloat = MusicVolume;
            options["PlaySfx"].AsBool = PlaySfx;
            options["SfxVolume"].AsFloat = SfxVolume;
            options["Antialias"].AsInt = (int)Antialias;
            options["SSAO"].AsInt = (int)SSAO;
            options["MotionBlur"].AsInt = (int)MotionBlur;
            options["Shadow"].AsInt = (int)Shadow;
            options["Vignette"].AsInt = (int)Vignette;
            return options;
        }

        void Load(ref JSONNode node)
        {
            FullScreen = node["FullScreen"].AsBool;
            Vsync = node["Vsync"].AsBool;
            Width = node["Width"].AsInt;
            Height = node["Height"].AsInt;
            PlayMusic = node["PlayMusic"].AsBool;
            MusicVolume = node["MusicVolume"].AsFloat;
            PlaySfx = node["PlaySfx"].AsBool;
            SfxVolume = node["SfxVolume"].AsFloat;
            Antialias = (Quality)node["Antialias"].AsInt;
            SSAO = (Quality)node["SSAO"].AsInt;
            MotionBlur = (Quality)node["MotionBlur"].AsInt;
            Shadow = (Quality)node["Shadow"].AsInt;
            Vignette = (Quality)node["Vignette"].AsInt;
        }
    }

}