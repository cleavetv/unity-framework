using System;
using System.Collections.Generic;
using CleaveFramework.Scene;

namespace CleaveFramework.Factory
{
    /// <summary>
    /// defines a post instantiation constructor for the object
    /// </summary>
    /// <param name="obj">object to construct</param>
    /// <returns>constructed object</returns>
    public delegate object Constructor(object obj);

    /// <summary>
    /// generic Factory implementation
    /// </summary>
    static class Factory
    {
        static Dictionary<Type, Constructor> _constructors = new Dictionary<Type, Constructor>();

        /// <summary>
        /// Sets a post instantiation constructor to an object type
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="constructor">delegate to act as constructor</param>
        static public void SetConstructor<T>(Constructor constructor)
        {
            // for now we are a single delegate
            if (_constructors.ContainsKey(typeof (T))) return;
            _constructors.Add(typeof(T), constructor);
        }

        /// <summary>
        /// Create object of type T, if a constructor exists in the Factory use it on the object
        /// before returning it
        /// </summary>
        /// <typeparam name="T">Type of object to construct</typeparam>
        /// <returns>constructed object</returns>
        static public object Create<T>()
        {
            var obj = Activator.CreateInstance<T>();

            if(_constructors.ContainsKey(typeof(T)))
            {
                if(_constructors[typeof(T)] != null)
                    obj = (T)_constructors[typeof (T)].Invoke(obj);
            }
            return obj;
        }

        /// <summary>
        /// Create an object of type T and run this custom constructor on it
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="constructor">constructor to run on object</param>
        /// <returns>constructed object</returns>
        static public object Create<T>(Constructor constructor)
        {
            var obj = Activator.CreateInstance<T>();

            if (constructor != null)
            {
                obj = (T) constructor.Invoke(obj);
            }

            return obj;
        }

        /// <summary>
        /// Create an object of type T and insert it as singleton into the scene objects data
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="data">Instance of SceneObjectsData to insert to</param>
        /// <returns>constructed object</returns>
        static public object Create<T>(SceneObjectData data)
        {
            var obj = Create<T>();
            PushSingleton<T>(data, obj);
            return obj;
        }

        /// <summary>
        /// Create an object of type T and insert it as transient into the objects data
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="data">Instance of scene data to insert to</param>
        /// <param name="name">Name of object to use in transients library</param>
        /// <returns>constructed object</returns>
        static public object Create<T>(SceneObjectData data, string name)
        {
            var obj = Create<T>();
            PushTransient<T>(data, name, obj);
            return obj;
        }

        /// <summary>
        /// Create an object of type T with a custom constructor and insert it into singleton library
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="constructor">custom constructor to use</param>
        /// <param name="data">Instance of scene data</param>
        /// <returns>constructed object</returns>
        static public object Create<T>(Constructor constructor, SceneObjectData data)
        {
            var obj = Create<T>(constructor);
            PushSingleton<T>(data, obj);
            return obj;
        }

        /// <summary>
        /// Create an object of type T with a custom constructor and insert it into transients library
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="constructor">custom constructor to use</param>
        /// <param name="data">Instance of scene data</param>
        /// <param name="name">Name to use in transients library</param>
        /// <returns>constructed object</returns>
        static public object Create<T>(Constructor constructor, SceneObjectData data, string name)
        {
            var obj = Create<T>(constructor);
            PushTransient<T>(data, name, obj);
            return obj;
        }

        static private void PushSingleton<T>(SceneObjectData data, object obj)
        {
            if (data == null)
            {
                throw new Exception("SceneObjectsData passed to Create() was null.");
            }
            data.PushObjectAsSingleton((T)obj);
        }

        static private void PushTransient<T>(SceneObjectData data, string name, object obj)
        {
            if (data == null)
            {
                throw new Exception("SceneObjectsData passed to Create() was null.");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name passed to Create() was empty or null");
            }
            data.PushObjectAsTransient(name, (T)obj);
        }
    }
}
