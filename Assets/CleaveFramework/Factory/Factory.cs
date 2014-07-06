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
using System;
using CleaveFramework.Binding;
using CleaveFramework.DependencyInjection;
using CleaveFramework.Scene;
using CleaveFramework.Tools;
using UnityEngine;

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
        static Binding<Type, Constructor> _constructors = new Binding<Type,Constructor>();

        /// <summary>
        /// Sets a post instantiation constructor to an object type
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="constructor">delegate to act as constructor</param>
        static public void SetConstructor<T>(Constructor constructor)
        {
            _constructors[typeof (T)] = constructor;
        }

        /// <summary>
        /// attach a new component of type T to the GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(string componentName, GameObject go)
        {
            if (go == null)
            {
                throw new Exception("AddComponent: GameObject was null");
            }
            var component = go.AddComponent(componentName) as MonoBehaviour;
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour)InvokeDefaultConstructor<T>(component);
            return component;
        }

        /// <summary>
        /// attach a new component of type T to the GameObject of given name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goName"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(string goName)
        {
            var go = ResolveGameObject(goName);
            return AddComponent<T>(go);
        }

        /// <summary>
        /// attach a new component of type T to the GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(GameObject go)
        {
            if (go == null)
            {
                throw new Exception("AddComponent: GameObject was null");
            }
            var component = go.AddComponent(typeof(T).Name) as MonoBehaviour;
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour)InvokeDefaultConstructor<T>(component);
            return component;
        }

        /// <summary>
        /// attach a new component of type T to the GameObject of given name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goName"></param>
        /// <param name="constructor"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(string goName, Constructor constructor)
        {
            var go = ResolveGameObject(goName);
            return AddComponent<T>(go, constructor);
        }

        /// <summary>
        /// attach a new component of type T to the GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="constructor"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(GameObject go, Constructor constructor)
        {
            if (go == null)
            {
                throw new Exception("AddComponent: GameObject was null");
            }
            var component = go.AddComponent(typeof(T).Name) as MonoBehaviour;
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour)InvokeConstructor<T>(component, constructor);
            return component;
        }

        /// <summary>
        /// attach a new component of type T to the GameObject of given name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(string goName, SceneObjectData data)
        {
            var go = ResolveGameObject(goName);
            return AddComponent<T>(go, data);
        }

        /// <summary>
        /// attach a new component of type T to the GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(GameObject go, SceneObjectData data)
        {
            CDebug.Assert(go == null, "AddComponent: GameObject was null.");

            var component = go.AddComponent(typeof(T).Name) as MonoBehaviour;
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour)InvokeDefaultConstructor<T>(component);
            PushSingleton<T>(data, component);
            return component;
        }

        /// <summary>
        /// attach a new component of type T to the GameObject of given name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goName"></param>
        /// <param name="constructor"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(string goName, Constructor constructor, SceneObjectData data)
        {
            var go = ResolveGameObject(goName);
            return AddComponent<T>(go, constructor, data);
        }

        /// <summary>
        /// attach a new component of type T to the GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="constructor"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(GameObject go, Constructor constructor, SceneObjectData data)
        {
            CDebug.Assert(go == null, "AddComponent: GameObject was null.");

            var component = go.AddComponent(typeof(T).Name) as MonoBehaviour;
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour)InvokeConstructor<T>(component, constructor);
            PushSingleton<T>(data, component);
            return component;
        }

        /// <summary>
        /// attach a new component of type T to the GameObject of given name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goName"></param>
        /// <param name="constructor"></param>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(string goName, Constructor constructor, SceneObjectData data, string name)
        {
            var go = ResolveGameObject(goName);
            return AddComponent<T>(go, constructor, data, name);
        }

        /// <summary>
        /// attach a new component of type T to the GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="constructor"></param>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(GameObject go, Constructor constructor, SceneObjectData data, string name)
        {
            CDebug.Assert(go == null, "AddComponent: GameObject was null.");

            var component = go.AddComponent(typeof(T).Name) as MonoBehaviour;
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour)InvokeConstructor<T>(component, constructor);
            PushTransient<T>(data, name, component);
            return component;
        }

        /// <summary>
        /// attach a new component of type T to the GameObject of given name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="goName"></param>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(string goName, SceneObjectData data, string name)
        {
            var go = ResolveGameObject(goName);
            return AddComponent<T>(go, data, name);
        }

        /// <summary>
        /// attach a new component of type T to the GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public MonoBehaviour AddComponent<T>(GameObject go, SceneObjectData data, string name)
        {
            CDebug.Assert(go == null, "AddComponent: GameObject was null.");

            var component = go.AddComponent(typeof(T).Name) as MonoBehaviour;
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour)InvokeDefaultConstructor<T>(component);
            PushTransient<T>(data, name, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <returns></returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName)
        {
            var component = ResolveComponent<T>(goName);
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour) InvokeDefaultConstructor<T>(component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject in a UnityScene
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">GameObject holding the component</param>
        /// <returns></returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go)
        {
            var component = ResolveComponent<T>(go);
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour)InvokeDefaultConstructor<T>(component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName, Constructor constructor)
        {
            var component = ResolveComponent<T>(goName);
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour) InvokeConstructor<T>(component, constructor);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go, Constructor constructor)
        {
            var component = ResolveComponent<T>(go);
            component = Injector.PerformInjections(component);
            component = (MonoBehaviour)InvokeConstructor<T>(component, constructor);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a singleton
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName, Constructor constructor, SceneObjectData data)
        {
            var component = ConstructMonoBehaviour<T>(goName, constructor);
            PushSingleton<T>(data, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a singleton
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go, Constructor constructor, SceneObjectData data)
        {
            var component = ConstructMonoBehaviour<T>(go, constructor);
            PushSingleton<T>(data, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a singleton
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName, SceneObjectData data)
        {
            var component = ConstructMonoBehaviour<T>(goName);
            PushSingleton<T>(data, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a singleton
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go, SceneObjectData data)
        {
            var component = ConstructMonoBehaviour<T>(go);
            PushSingleton<T>(data, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a transient
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <param name="name">name of the object</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName, SceneObjectData data, string name)
        {
            var component = ConstructMonoBehaviour<T>(goName);
            PushTransient<T>(data, name, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a transient
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">Name of GameObject holding the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <param name="name">name of the object</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go, SceneObjectData data, string name)
        {
            var component = ConstructMonoBehaviour<T>(go);
            PushTransient<T>(data, name, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a transient
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="goName">Name of GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <param name="name">name of the object</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(string goName, Constructor constructor, SceneObjectData data, string name)
        {
            var component = ConstructMonoBehaviour<T>(goName, constructor);
            PushTransient<T>(data, name, component);
            return component;
        }

        /// <summary>
        /// Construct a previously instantiated Component attached to a GameObject present and active in a UnityScene 
        /// and add it to the SceneObjects as a transient
        /// </summary>
        /// <typeparam name="T">Type of Component to get</typeparam>
        /// <param name="go">Name of GameObject holding the component</param>
        /// <param name="constructor">constructor to run on the component</param>
        /// <param name="data">SceneObjectsData instance to add component to</param>
        /// <param name="name">name of the object</param>
        /// <returns>constructed component</returns>
        static public MonoBehaviour ConstructMonoBehaviour<T>(GameObject go, Constructor constructor, SceneObjectData data, string name)
        {
            var component = ConstructMonoBehaviour<T>(go, constructor);
            PushTransient<T>(data, name, component);
            return component;
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
            obj = Injector.PerformInjections(obj);
            obj = (T)InvokeDefaultConstructor<T>(obj);
            return obj;
        }

        /// <summary>
        /// Create an object of type T and run it's default constructor if one exists
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        static public object Create(Type t)
        {
            var obj = Activator.CreateInstance(t);
            obj = Injector.PerformInjections(obj);
            if (_constructors.IsBound(t))
            {
                _constructors[t].Invoke(obj);
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
            obj = Injector.PerformInjections(obj);
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
            CDebug.Assert(data == null, "SceneObjectsData passed to Create() was null.");
            data.PushObjectAsSingleton((T)obj);
        }

        static private void PushTransient<T>(SceneObjectData data, string name, object obj)
        {

            CDebug.Assert(data == null, "SceneObjectsData passed to Create() was null.");
            CDebug.Assert(string.IsNullOrEmpty(name), "Name passed to Create() was empty or null");

            data.PushObjectAsTransient(name, (T)obj);
        }

        static private object InvokeConstructor<T>(object obj, Constructor constructor)
        {
            if (constructor != null)
            {
                obj = constructor.Invoke(obj);
            }

            return obj;
        }

        static private object InvokeDefaultConstructor<T>(object obj)
        {
            var objType = typeof (T);
            if (!_constructors.IsBound(objType)) return obj;
            return (T)_constructors[objType].Invoke(obj);
        }

        static private GameObject ResolveGameObject(string goName)
        {
            var go = GameObject.Find(goName);
            CDebug.Assert(go == null, "ResolveGameObject: GameObject was null.");
            return go;
        }
        static private MonoBehaviour ResolveComponent<T>(string goName)
        {
            var go = ResolveGameObject(goName);
            return ResolveComponent<T>(go);
        }
        static private MonoBehaviour ResolveComponent<T>(GameObject go)
        {
            var component = go.GetComponent(typeof(T).Name);
            CDebug.Assert(component == null, "ResolveComponent: Component was null.");

            return (MonoBehaviour)component;
        }
    }
}
