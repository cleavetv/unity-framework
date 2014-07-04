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
using System.Linq;
using System.Reflection;
using CleaveFramework.Binding;
using CleaveFramework.Tools;
using UnityEngine;


namespace CleaveFramework.DependencyInjection
{
    /// <summary>
    /// Injector handles mapping types and interfaces to implementations
    /// 
    /// for ex:
    /// // Map IMySystem interface to an implementation of MySystem in the Injector
    /// // Create MySystem using ConstructMySystem as its secondary constructor
    /// Injector.AddSingleton<IMySystem>(Factory.Create<MySystem>(ConstructMySystem));
    /// // Or have the Injector create a new instance for you instead:
    /// Injector.AddTransient<IMySystem>(typeof(MySystem))
    /// 
    /// // Inject MySystem into an object that requires an IMySystem interface:
    /// class SomeObject : IInitializable {
    ///     [Inject] public IMySystem MySystemService {get; set;}
    ///     public Initialize() {
    ///         // MySystemService is valid here and instance is either the singleton given or a new instance
    ///         // depending on what was given to the Injector
    ///         MySystemService.Method(); 
    ///     }
    /// }
    /// 
    /// </code>
    /// </summary>
    static class Injector
    {

        private enum InjectTypes
        {
            Singleton,
            Transient,
            MonoBehaviour,
        }

        private static Binding<Type, InjectTypes> _injectionTypes = new Binding<Type, InjectTypes>();
        private static Binding<Type, object> _singletons = new Binding<Type, object>();

        private static Binding<Type, Type> _transients = new Binding<Type, Type>(); 

        /// <summary>
        /// Add a Transient type to the Injector and map it to a specific implementation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="impl"></param>
        public static void AddTransient<T>(Type impl)
        {
            _injectionTypes.Bind(typeof(T), InjectTypes.Transient);
            _transients.Bind(typeof(T), impl);
        }

        /// <summary>
        /// Add a Transient type to the Injector and map it to a specific implementation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="I"></typeparam>
        public static void AddTransient<T, I>()
        {
            _injectionTypes.Bind(typeof(T), InjectTypes.Transient);
            _transients.Bind(typeof(T), typeof(I));
        }

        /// <summary>
        /// Add a transient type to the Injector and use itself as the implementation type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void AddTransient<T>()
        {
            _injectionTypes.Bind(typeof(T), InjectTypes.Transient);
            _transients.Bind(typeof(T), typeof(T));
        }

        /// <summary>
        /// Add a singleton type to the injector and map it to the instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void AddSingleton<T>(object instance)
        {
            _injectionTypes.Bind(typeof(T), InjectTypes.Singleton);
            _singletons.Bind(typeof (T), instance);
        }

        private static object Resolve(Type t)
        {
            if(_singletons.IsBound(t))
            {
                return _singletons.Resolve(t);
            }
            if(_injectionTypes.Resolve(t) == InjectTypes.Transient)
            {
                var impl = _transients.Resolve(t);
                return Factory.Factory.Create(impl);
            }
            return null;
        }

        private static void InjectProperties<T>(T obj)
        {
            var props = typeof(T).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Inject)));
            foreach (var cProp in props)
            {
                if (!cProp.CanWrite) continue;

                CDebug.Log("Injector.InjectProperties<" + cProp.PropertyType + "> on " + obj);
                var instance = Resolve(cProp.PropertyType);
                cProp.SetValue(obj, instance, null);
            }
        }

        private static void InjectFields<T>(T obj)
        {
            var fields = typeof(T).GetFields().Where(prop => Attribute.IsDefined(prop, typeof(Inject)));
            foreach (var cField in fields)
            {
                CDebug.Log("Injector.InjectFields<" + cField.FieldType + "> on " + obj);
                var instance = Resolve(cField.FieldType);
                cField.SetValue(obj, instance);
            }
        }

        private static void InjectMonoBehaviour<T>(T obj)
        {
            var monoObj = obj as MonoBehaviour;
            var monoFields = monoObj.GetType().GetMembers().Where(prop => Attribute.IsDefined(prop, typeof(Inject)));
            foreach (FieldInfo monoField in monoFields)
            {
                CDebug.Log("Injector.InjectMonoBehaviour<" + monoField.FieldType + "> on " + obj);
                var instance = Resolve(monoField.FieldType);
                monoField.SetValue(monoObj, instance);
            }
        }

        /// <summary>
        /// Reflect on the Object and inject its fields and properties marked with [Inject] tag
        /// Note: although this is public it should generally only need to be called by Factory
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T PerformInjections<T>(T obj)
        {

            if (typeof (MonoBehaviour).IsAssignableFrom(typeof (T)))
            {
                InjectMonoBehaviour(obj);
            }
            else
            {
                InjectProperties(obj);
                InjectFields(obj);
            }

            return obj;
        }
    }

}
