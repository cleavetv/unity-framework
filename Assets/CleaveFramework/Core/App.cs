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
