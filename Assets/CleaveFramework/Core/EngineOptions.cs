/* Copyright 2014 Glen/CleaveTV

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. */
using System.IO;
using CleaveFramework.Commands;
using SimpleJSON;

namespace CleaveFramework.Core
{
    /// <summary>
    /// wrapper for Unity engine configuration options
    /// </summary>
    public class EngineOptions
    {
        private const string ConfigFile = "engine.ini";

        // Window options
        public bool FullScreen { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // Audio options
        public bool PlayMusic { get; set; }
        public float MusicVolume { get; set; }
        public bool PlaySfx { get; set; }
        public float SfxVolume { get; set; }

        // Rendering options
        public enum Quality
        {
            Ultra,
            High,
            Medium,
            Low,
            Disabled
        }
        public Quality Antialias { get; set; }
        public Quality SSAO { get; set; }
        public Quality MotionBlur { get; set; }
        public Quality Shadow { get; set; }
        public Quality Vignette { get; set; }

        public EngineOptions()
        {
            // check for configuration file
            if (!LoadFromConfig())
            {
                CreateDefaultOptions();
            }

            Command.Register(typeof(ApplyOptionsCmd), OnApplyOptions);
            Framework.PushCommand(new ApplyOptionsCmd());
        }

        /// <summary>
        /// Create the options properties to run an engine from the iniFile
        /// </summary>
        private bool LoadFromConfig()
        {
            if (!File.Exists(ConfigFile)) return false;
            var options = JSONNode.LoadFromFile(ConfigFile);
            if (options == null) return false;
            Load(ref options);
            return true;
        }

        private void CreateDefaultOptions()
        {
            FullScreen = false;
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