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
    /// 
    /// for ex:
    /// var myLibrary = new BindingLibrary();
    /// myLibrary.Bind(11, "Eleven");
    /// var value = myLibrary.Resolve(11); // value = "Eleven";
    /// 
    /// myLibrary.Bind("sky", "is up");
    /// var value = myLibrary.Resolve("sky"); // value = "is up";
    /// 
    /// myLibrary.Bind(ESomeEnum.EnumEntry, typeof(MyClass));
    /// var value = myLibrary.Resolve(ESomeEnum.EnumEntry); // value = typeof(MyClass);
    /// 
    /// myLibrary.Bind(typeof(MyClass), new MyClass());
    /// var value = myLibrary.Resolve(typeof(MyClass)); // value = MyClass instance previously bound
    /// </summary>
    class BindingLibrary
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
        /// Throw a binding into the library of LHS = RHS
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
        /// Resolve a binding from the library of LHS
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
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
