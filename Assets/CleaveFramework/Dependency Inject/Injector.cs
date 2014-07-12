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
using System.Collections;
using System.Collections.Generic;
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
    /// 
    /// for ex:
    /// <code>
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
    static public class Injector
    {

        private enum InjectTypes
        {
            Singleton,
            Transient,
        }

        private static Binding<Type, InjectTypes> _injectionTypes = new Binding<Type, InjectTypes>();
        private static Binding<Type, object> _singletons = new Binding<Type, object>();
        private static Binding<Type, Type> _transients = new Binding<Type, Type>(); 

        private static Binding<Type, IEnumerable<FieldInfo>>  _fieldsCache = new Binding<Type, IEnumerable<FieldInfo>>();
        private static Binding<Type, IEnumerable<PropertyInfo>> _propertyCache = new Binding<Type, IEnumerable<PropertyInfo>>();
        private static Binding<Type, IEnumerable<MemberInfo>> _memberCache = new Binding<Type, IEnumerable<MemberInfo>>();

        /// <summary>
        /// Add a Transient type to the Injector and map it to a specific implementation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="impl"></param>
        public static void BindTransient<T>(Type impl)
        {
            _injectionTypes.Bind(typeof(T), InjectTypes.Transient);
            _transients.Bind(typeof(T), impl);
        }

        /// <summary>
        /// Add a Transient type to the Injector and map it to a specific implementation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="I"></typeparam>
        public static void BindTransient<T, I>()
        {
            _injectionTypes.Bind(typeof(T), InjectTypes.Transient);
            _transients.Bind(typeof(T), typeof(I));
        }

        /// <summary>
        /// Add a transient type to the Injector and use itself as the implementation type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void BindTransient<T>()
        {
            _injectionTypes.Bind(typeof(T), InjectTypes.Transient);
            _transients.Bind(typeof(T), typeof(T));
        }

        /// <summary>
        /// Add a singleton type to the injector and map it to the instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void BindSingleton<T>(object instance)
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
                var instance = Factory.Factory.Create(impl);

                return instance;

            }
            return null;
        }

        private static void InjectProperties<T>(T obj)
        {
            var injectingType = typeof (T);

            IEnumerable<PropertyInfo> props;
            if (_propertyCache.IsBound(injectingType))
            {
                props = _propertyCache.Resolve(injectingType);
            }
            else
            {
                props = injectingType.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Inject)));
                _propertyCache.Bind(injectingType, props);
            }
            CDebug.Assert(props == null, "Injector.InjectProperties()");

            foreach (var cProp in props)
            {
                if (!cProp.CanWrite) continue;
                CDebug.Log("Injector.InjectProperties<" + cProp.PropertyType + "> on " + obj);
                var instance = Resolve(cProp.PropertyType);
                cProp.SetValue(obj, instance, null);

                var value = cProp.GetValue(obj, null);
                PerformInjections(value);
            }
        }

        private static void InjectFields<T>(T obj)
        {

            var injectingType = typeof(T);

            IEnumerable<FieldInfo> fields;
            if (_fieldsCache.IsBound(injectingType))
            {
                fields = _fieldsCache.Resolve(injectingType);
            }
            else
            {
                fields = injectingType.GetFields().Where(prop => Attribute.IsDefined(prop, typeof(Inject)));
                _fieldsCache.Bind(injectingType, fields);
            }
            CDebug.Assert(fields == null, "Injector.InjectFields()");

            foreach (var cField in fields)
            {
                CDebug.Log("Injector.InjectFields<" + cField.FieldType + "> on " + obj);
                var instance = Resolve(cField.FieldType);
                cField.SetValue(obj, instance);

                var value = cField.GetValue(obj);
                PerformInjections(value);
            }
        }

        private static void InjectMonoBehaviour<T>(T obj)
        {
            var monoObj = obj as MonoBehaviour;

            var injectingType = monoObj.GetType();

            IEnumerable<MemberInfo> members;
            if (_memberCache.IsBound(injectingType))
            {
                members = _memberCache.Resolve(injectingType);
            }
            else
            {
                members = injectingType.GetMembers().Where(prop => Attribute.IsDefined(prop, typeof(Inject)));
                _memberCache.Bind(injectingType, members);
            }
            CDebug.Assert(members == null, "Injector.InjectMonoBehaviour()");

            // for some reason MonoBehaviour members need to be addressed as fields.
            foreach (FieldInfo cField in members)
            {
                CDebug.Log("Injector.InjectMonoBehaviour<" + cField.FieldType + "> on " + obj);
                var instance = Resolve(cField.FieldType);
                cField.SetValue(monoObj, instance);

                var value = cField.GetValue(monoObj);
                PerformInjections(value);
            }
        }

        /// <summary>
        /// Reflect on the Object and inject its fields and properties marked with [Inject] tag
        /// Note: although this is public it should generally only need to be called by Factory
        /// except in the case of nested transient injections
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T PerformInjections<T>(T obj)
        {
            UnityEngine.Debug.Log("PerformInjections() = " + obj.GetType());

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
