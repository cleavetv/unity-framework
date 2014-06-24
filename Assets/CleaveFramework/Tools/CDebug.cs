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
using System.Collections.Generic;
using System.Diagnostics;

namespace CleaveFramework.Tools
{
    static class CDebug
    {
        public enum ConsoleLogMethod
        {
            Silent, // no log calls recognized
            Selected, // recognize log calls made from types registered with LogThis(type)
            Verbose, // all log calls recognized
        }

        /// <summary>
        /// Public setter for setting logging verbosity
        /// </summary>
        public static ConsoleLogMethod DisplayMethod = ConsoleLogMethod.Verbose;

        private static readonly List<Type> SelectedTypes = new List<Type>();

        /// <summary>
        /// add a type into the selected logging types:
        /// CDebug.LogThis(typeof(ClassName))
        /// </summary>
        /// <param name="type">log from this type</param>
        public static void LogThis(Type type)
        {
            if (!SelectedTypes.Contains(type))
            {
                SelectedTypes.Add(type);
            }
        }

        #region UnityEngine.Debug.Log Wrappers
        public static void Log(string str)
        {
            if (CanLog())
            {
                UnityEngine.Debug.Log(str);
            }
        }
        public static void Log(string str, UnityEngine.Object obj)
        {
            if (CanLog())
            {
                UnityEngine.Debug.Log(str, obj);
            }
        }
        #endregion

        private static bool CanLog()
        {
            switch (DisplayMethod)
            {
                case ConsoleLogMethod.Selected:
                {
                    // call into the second level of StackFrame
                    // This -> CDebug -> Caller
                    var frame = new StackFrame(2);
                    var method = frame.GetMethod();
                    var type = method.DeclaringType;
                    return SelectedTypes.Contains(type);
                }
                case ConsoleLogMethod.Verbose:
                {
                    return true;
                }
                case ConsoleLogMethod.Silent:
                {
                    return false;
                }
            }

            return false;
        }
    }
}
