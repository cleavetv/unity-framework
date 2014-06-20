using UnityEngine;

namespace CleaveFramework.Scene
{
    abstract public class SceneView : MonoBehaviour
    {
        protected SceneObjectData SceneObjects { get; private set; }

        virtual public void Start()
        {
            SceneObjects = new SceneObjectData();
            gameObject.name = "SceneView";
        }

        /// <summary>
        /// construct your scene here
        /// </summary>
        public abstract void Initialize();

        virtual public void Update()
        {
            SceneObjects.Update(Time.deltaTime);
        }

    }
}
