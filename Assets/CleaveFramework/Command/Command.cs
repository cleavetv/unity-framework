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

namespace CleaveFramework.Commands
{
    /// <summary>
    /// base executable command
    /// </summary>
    abstract public class Command
    {
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
        [Obsolete("Migrate to CmdBinder, this function will self destruct on July 11th, 2014", false)]
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
            //Debug.Log("Registering: " + type.ToString());
        }
		
        /// <summary>
        /// generic registration of a command type to a callback
        /// </summary>
        /// <example>  
        /// This sample shows how to call the generic <see cref="Register{T}"/> method.
        /// <code> 
        /// class CustomDataModel  
        /// { 
        ///     public void Initialize()
        ///     { 
        ///         Commands.Register&lt;MyCustomCommand&gt;(MyCallback);
        ///     } 
        /// } 
        /// </code> 
        /// </example>         
        /// <typeparam name="T">command type</typeparam>
        /// <param name="execute">delegate for callback</param>
        [Obsolete("Migrate to CmdBinder, this function will self destruct on July 11th, 2014", false)]
        public static void Register<T>(CommandCallback execute) where T : Command
        {
            Register(typeof(T), execute);
        }	
		
        /// <summary>
        /// remove callback registration from a command type
        /// </summary>
        /// <param name="type">command type</param>
        /// <param name="execute">delegate to remove from callback</param>
        [Obsolete("Migrate to CmdBinder, this function will self destruct on July 11th, 2014", false)]
        public static void Unregister(Type type, CommandCallback execute)
        {
            if (execute != null)
            {
                _executeCallbacks[type] -= execute;
            }
        }
		
        /// <summary>
        /// generic remove callback registration from a command type
        /// </summary>
        /// <example>  
        /// This sample shows how to call the generic <see cref="Unregister{T}"/> method.
        /// <code> 
        /// class CustomDataModel  
        /// { 
        ///     public void Destroy()
        ///     { 
        ///         Commands.Unregister&lt;MyCustomCommand&gt;(MyCallback);
        ///     } 
        /// } 
        /// </code> 
        /// </example>         
        /// <typeparam name="T">command type</typeparam>
        /// <param name="execute">delegate to remove from callback</param>
        [Obsolete("Migrate to CmdBinder, this function will self destruct on July 11th, 2014", false)]
        public static void Unregister<T>(CommandCallback execute) where T : Command
        {
            Unregister(typeof(T), execute);
        }	
		
        /// <summary>
        /// base execution method, can override to provide more functionality
        /// </summary>
        virtual public void Execute()
        {
            CmdBinder.DispatchBindings(GetType(), this);

            // holding this code for backwards API compatibility until its removal
            if (!_executeCallbacks.ContainsKey(this.GetType())) return;
            if (_executeCallbacks[this.GetType()] != null)
                _executeCallbacks[this.GetType()].Invoke(this);
        }
    }
}
