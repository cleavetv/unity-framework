using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleaveFramework.Interfaces;
using UnityEngine;

namespace CleaveFramework.Core
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
