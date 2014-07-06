using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CleaveFramework.Tools
{
    static class CDebug
    {
        public static bool BreakOnAsserts = false;

        #region Assert
        [Conditional("UNITY_EDITOR")]
        public static void Assert(bool condition)
        {
            if (condition)
            {
                if(BreakOnAsserts) UnityEngine.Debug.Break();
                throw new Exception();
            }
        }
        [Conditional("UNITY_EDITOR")]
        public static void Assert(bool condition, string msg)
        {
            if (condition)
            {
                if (BreakOnAsserts) UnityEngine.Debug.Break();
                throw new Exception(msg);
            }
        }
        #endregion

        #region Console Logger

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
        /// CDebug.LogThis(typeof(ClassName));
        /// or
        /// CDebug.LogThis<ClassName>();
        /// </summary>
        /// <param name="type">log from this type</param>
        public static void LogThis(Type type)
        {
            if (!SelectedTypes.Contains(type))
            {
                SelectedTypes.Add(type);
            }
        }
        public static void LogThis<T>()
        {
            LogThis(typeof(T));
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
        #endregion
    }
}
