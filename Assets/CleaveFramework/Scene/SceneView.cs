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
using CleaveFramework.Core;
using UnityEngine;

namespace CleaveFramework.Scene
{
    abstract public class SceneView : View
    {
        protected SceneObjectData SceneObjects { get; private set; }

        virtual public void Start()
        {
            SceneObjects = new SceneObjectData();
        }

        /// <summary>
        /// construct your scene here
        /// </summary>
        public abstract void Initialize();

        virtual public void Update()
        {
            SceneObjects.Update(Time.deltaTime);
        }

        void OnDestroy()
        {
            if(SceneObjects != null)
                SceneObjects.Destroy();
        }

    }
}
