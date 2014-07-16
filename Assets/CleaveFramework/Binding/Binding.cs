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
using System.Collections.Generic;
using System.Linq;

namespace CleaveFramework.Binding
{
    /// <summary>
    /// A generic binding wrapper for Dictionary
    /// 
    /// Usage ex:
    /// 
    /// Make a library:
    /// var myLibrary = new Binding<string, string>();
    /// 
    /// Add some books:
    /// myLibrary.Bind("The Shining", "Steven King");
    /// myLibrary.Bind("Misery", "Steven King");
    /// 
    /// Find an author:
    /// var author = myLibrary.Resolve("Misery"); 
    /// 
    /// Find if a book is in the library:
    /// if(myLibrary.IsBound("War and Peace")) // false
    /// 
    /// Find a collection matching values(note this is VERY EXPENSIVE)
    /// var books = myLibrary.FindKeyMatches("Steven King"); 
    /// 
    /// </summary>
    /// <typeparam name="T">Key</typeparam>
    /// <typeparam name="V">Value</typeparam>
    public class Binding<T, V>
    {
        public Dictionary<T, V> Bindings = new Dictionary<T, V>();

        /// <summary>
        /// Bind one value to another, LHS is considered the type, RHS is considered the value
        /// 
        /// for ex 
        /// var binds = new Binding<int, string>();
        /// binds.Bind(39, "thirty nine");
        /// 
        /// If the binding already exists it is overwritten
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        public void Bind(T type, V val)
        {
            if (Bindings.ContainsKey(type))
            {
                Bindings[type] = val;
            }
            else
            {
                Bindings.Add(type, val);
            }
        }

        /// <summary>
        /// Resolve a previous binding, from LHS to RHS
        /// 
        /// for ex
        /// var alphaNumber = binds.Resolve(39);
        /// CDebug.Log(alphaNumber.ToString()); // prints "thirty nine" to console
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public V Resolve(T type)
        {
            return Bindings.ContainsKey(type) ? Bindings[type] : default(V);
        }

        /// <summary>
        /// Resolve a list of LHS keys from a RHS value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IEnumerable<T> FindKeyMatches(V value)
        {
            return Bindings.Where(pair => pair.Value.Equals(value))
                              .Select(pair => pair.Key);
        }

        /// <summary>
        /// Check if a LHS key is bound
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsBound(T type)
        {
            return Bindings.ContainsKey(type);
        }

        /// <summary>
        /// removes all bindings from the library
        /// </summary>
        public void Clear()
        {
            Bindings.Clear();
        }

        /// <summary>
        /// removes binding of type from the library
        /// </summary>
        /// <param name="type"></param>
        public V Clear(T type)
        {
            var obj = Bindings[type];
            Bindings.Remove(type);
            return obj;
        }

        /// <summary>
        /// return the bindings library as an associative array
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<T, V>[] ToArray()
        {
            return Bindings.ToArray();
        }

        /// <summary>
        /// return the number of total bindings in the library
        /// </summary>
        public int Count
        {
            get { return Bindings.Count; }
        }

        /// <summary>
        /// [] operator overload
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public V this[T type]
        {
            get { return Resolve(type); }
            set { Bind(type, value);  }
        }
    }

}
