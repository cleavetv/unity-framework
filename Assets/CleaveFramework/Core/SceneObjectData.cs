using System;
using System.Collections.Generic;
using CleaveFramework.Interfaces;
using UnityEngine;

namespace CleaveFramework.Core
{
    /// <summary>
    /// container for scene objects
    /// </summary>
    public sealed class SceneObjectData
    {
        /// <summary>
        /// singleton library
        /// </summary>
        private Dictionary<Type, object> _singletons;

        /// <summary>
        /// transient objects library
        /// </summary>
        private Dictionary<Type, Dictionary<string, object>> _transients;

        /// <summary>
        /// IUpdateable objects library
        /// </summary>
        private List<IUpdateable> _updateables;

        /// <summary>
        /// IInitializeable objects library
        /// </summary>
        private List<IInitializeable> _initializeables;

        private bool _wasSceneInitialized;

        public SceneObjectData()
        {
            _singletons = new Dictionary<Type, object>();
            _transients = new Dictionary<Type, Dictionary<string, object>>();
            _updateables = new List<IUpdateable>();
            _initializeables = new List<IInitializeable>();
            _wasSceneInitialized = false;
        }

        public void Update(float deltaTime)
        {
            // ensure IInitializeables before we update
            if (!_wasSceneInitialized) return;

            // process IUpdateables
            foreach (var obj in _updateables)
            {
                obj.Update(deltaTime);
            }
        }

        public void InitializeSceneObjects()
        {
            // ensure scene is only able to be initialized once
            if (_wasSceneInitialized) return;

            Debug.Log("SceneObjectData::InitializeSceneObjects()");

            foreach (var obj in _initializeables)
            {
                obj.Initialize();
            }

            _wasSceneInitialized = true;
        }

        /// <summary>
        /// reflect the object's interfaces and insert it into the appropriate library
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        private static void ReflectInterfaces<T>(SceneObjectData instance, T obj)
        {
            if (typeof(IUpdateable).IsAssignableFrom(typeof(T)))
            {
                instance._updateables.Add((IUpdateable)obj);
            }
            if (typeof(IInitializeable).IsAssignableFrom(typeof(T)))
            {
                instance._initializeables.Add((IInitializeable)obj);
                
                // if the scene was already initialized we need to call initialize on this object which has been added dynamically after construction
                if (instance._wasSceneInitialized)
                {
                    ((IInitializeable)obj).Initialize();
                }
            }
        }

        /// <summary>
        /// Injects an object instance into the framework singleton library
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="obj">object instance</param>
        public void PushObjectAsSingleton<T>(T obj)
        {
            // reflect on the implemented interfaces
            ReflectInterfaces(this, obj);

            if (!_singletons.ContainsKey(typeof(T)))
            {
                // insert object into the library
                _singletons.Add(typeof(T), obj);
            }
            else
            {
                // TODO: should we throw exception here?  I'm not sure...
                // overwrite existing object with new instance
                _singletons[typeof(T)] = obj;
            }
        }

        /// <summary>
        /// resolve a type into an instance
        /// </summary>
        /// <typeparam name="T">object type to retrieve</typeparam>
        /// <returns>object instance if injected, otherwise null</returns>
        public object ResolveSingleton<T>()
        {
            return _singletons.ContainsKey(typeof(T)) ? _singletons[typeof(T)] : null;
        }

        /// <summary>
        /// Inject an object instance into the framework transients library
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="name">object instance name, required for lookup and must be unique per type</param>
        /// <param name="obj">object instance</param>
        public void PushObjectAsTransient<T>(string name, T obj)
        {
            ReflectInterfaces(this, obj);

            if (!_transients.ContainsKey(typeof (T)))
            {
                _transients.Add(typeof(T), new Dictionary<string, object>());
            }

            if (_transients[typeof (T)].ContainsKey(name))
            {
                throw new Exception("Object instance type/name combination is not unique: " + typeof(T).ToString() + "/" + name);
            }

            _transients[typeof(T)].Add(name, obj);

        }

        /// <summary>
        /// Resolve a name and type into an object from the transients library
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="name">instance name</param>
        /// <returns>object instance</returns>
        public object ResolveTransient<T>(string name)
        {
            if (!_transients.ContainsKey(typeof (T)))
            {
                return null;
            }
            return !_transients[typeof (T)].ContainsKey(name) ? null : _transients[typeof (T)][name];
        }

    }
}
