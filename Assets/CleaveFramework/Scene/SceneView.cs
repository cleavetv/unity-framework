using CleaveFramework.Core;
using UnityEngine;

namespace CleaveFramework.Scene
{
    abstract public class SceneView : View
    {
        public SceneObjectData SceneObjects { get; private set; }

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
            if(SceneObjects != null)
                SceneObjects.Update(Time.deltaTime);
        }

        void OnDestroy()
        {
            if(SceneObjects != null)
                SceneObjects.Destroy();
        }

    }
}
