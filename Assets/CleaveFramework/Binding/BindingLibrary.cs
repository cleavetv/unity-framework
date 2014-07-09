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

namespace CleaveFramework.Binding
{
    /// <summary>
    /// A generic BindingLibrary that can filter bindings into an appropriate library
    /// Since everything gets stored as an object you need to be sure to cast your expected 
    /// return value appropriately
    /// 
    /// for ex:
    /// var myLibrary = new BindingLibrary();
    /// myLibrary.Bind(11, "Eleven");
    /// var value = (string) myLibrary.Resolve(11); // value = "Eleven";
    /// 
    /// myLibrary.Bind("sky", "is up");
    /// var value = (string) myLibrary.Resolve("sky"); // value = "is up";
    /// 
    /// myLibrary.Bind(ESomeEnum.EnumEntry.ToString(), typeof(MyClass));
    /// var value = (Type) myLibrary.Resolve(ESomeEnum.EnumEntry.ToString()); // value = typeof(MyClass);
    /// 
    /// myLibrary.Bind(typeof(MyClass), new MyClass());
    /// var value = (MyClass) myLibrary.Resolve(typeof(MyClass)); // value = MyClass instance previously bound
    /// </summary>
    public class BindingLibrary
    {
        private Binding<object, object> _library = new Binding<object, object>();
        private Binding<object, object> _instances = new Binding<object, object>(); 

        public BindingLibrary()
        {
            _library.Bind(typeof(string), new Binding<string, object>());
            _library.Bind(typeof(int), new Binding<int, object>());
            _library.Bind(typeof(float), new Binding<float, object>());
            _library.Bind(typeof(Type), new Binding<Type, object>());
        }

        /// <summary>
        /// Explicitly get the Strings library
        /// </summary>
        public Binding<string, object> Strings
        {
            get { return (Binding<string, object>)_library[typeof (string)]; }
        }

        /// <summary>
        /// Explicitly get the Ints library
        /// </summary>
        public Binding<int, object> Ints
        {
            get { return (Binding<int, object>)_library[typeof(int)]; }
        }

        /// <summary>
        /// Explicitly get the Floats library
        /// </summary>
        public Binding<float, object> Floats
        {
            get { return (Binding<float, object>)_library[typeof(float)]; }
        }

        /// <summary>
        /// Explicitly get the Types library
        /// </summary>
        public Binding<Type, object> Types
        {
            get { return (Binding<Type, object>)_library[typeof(Type)]; }
        }

        /// <summary>
        /// Explicitly get the Objects library
        /// </summary>
        public Binding<object, object> Objects
        {
            get { return _instances; }
        }


        /// <summary>
        /// Add a binding implicitly by its type into the appropriately library
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Bind<T>(T key, object value)
        {
            var bindings = GetBinder<T>();
            if (bindings == null)
            {
                _instances.Bind(key, value);
            }
            else
            {
                bindings.Bind(key, value);
            }
        }

        /// <summary>
        /// Implicitly resolve a binding from the library
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>will return null if no key of that type exists</returns>
        public object Resolve<T>(T key)
        {
            var bindings = GetBinder<T>();
            return bindings == null ? _instances.Resolve(key) : bindings.Resolve(key);
        }

        private Binding<T, object> GetBinder<T>()
        {
            return _library.Resolve(typeof(T)) as Binding<T, object>;
        }

    }
}
