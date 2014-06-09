using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleaveFramework.Core
{
    #region Base
    /// <summary>
    /// base executable command
    /// </summary>
    abstract public class Command
    {
        /// <summary>
        /// callback delegate
        /// </summary>
        /// <param name="cmd">the command that was executed</param>
        public delegate void CommandCallback(Command cmd);
        /// <summary>
        /// dictionary for the command callbacks
        /// </summary>
        static private Dictionary<Type, CommandCallback> _executeCallbacks = new Dictionary<Type, CommandCallback>();
        /// <summary>
        /// internal registration for the given command type
        /// </summary>
        /// <param name="type">typeof(Command)</param>
        private static void SelfRegister(Type type)
        {
            _executeCallbacks.Add(type, null);
        }
        /// <summary>
        /// registration of a command type to a callback
        /// </summary>
        /// <param name="type">command type</param>
        /// <param name="execute">delegate for callback</param>
        public static void Register(Type type, CommandCallback execute)
        {
            if (!_executeCallbacks.ContainsKey(type))
            {
                SelfRegister(type);
            }
            if (execute != null)
            {
                _executeCallbacks[type] += execute;
            }
            Debug.Log("Registering: " + type.ToString());
        }
        /// <summary>
        /// remove callback registration from a command type
        /// </summary>
        /// <param name="type">command type</param>
        /// <param name="execute">delegate to remove from callback</param>
        public static void Unregister(Type type, CommandCallback execute)
        {
            if (execute != null)
            {
                _executeCallbacks[type] -= execute;
            }
        }
        /// <summary>
        /// base execution method, can override to provide more functionality
        /// </summary>
        virtual public void Execute()
        {
            if (!_executeCallbacks.ContainsKey(this.GetType())) return;
            if (_executeCallbacks[this.GetType()] != null)
                _executeCallbacks[this.GetType()].Invoke(this);
        }
    }
#endregion

    public class ApplyOptionsCmd : Command { }

    public class ChangeSceneCmd : Command
    {
        public string SceneName { get; private set; }
        public ChangeSceneCmd(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
