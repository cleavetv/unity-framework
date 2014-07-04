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
using CleaveFramework.Tools;

namespace CleaveFramework.Commands
{
    /// <summary>
    /// callback delegate
    /// </summary>
    /// <param name="cmd">the command that was executed</param>
    public delegate void CommandCallback(Command cmd);

    /// <summary>
    /// CmdBinder handles the binding and unbinding of CommandCallback actions to Command Types.
    /// </summary>
    static public class CmdBinder
    {
        static private Binding<Type, CommandCallback> _callbackBindings = new Binding<Type, CommandCallback>();

        /// <summary>
        /// Binds a command type to a CommandCallback
        /// </summary>
        /// <typeparam name="T">Command Type</typeparam>
        /// <param name="func">CommandCallback function</param>
        static public void AddBinding<T>(CommandCallback func) where T : Command
        {
            CDebug.Assert(func == null);

            var cmdType = typeof(T);

            if (_callbackBindings.IsBound(cmdType))
            {
                var callback = _callbackBindings.Resolve(cmdType);
                callback += func;
                _callbackBindings.Bind(cmdType, callback);
            }
            else
            {
                _callbackBindings.Bind(cmdType, func);
            }
        }

        /// <summary>
        /// Unbinds a command type from a CommandCallback
        /// 
        /// Note it's important to Unbind commands you've previously bound else you can 
        /// potentially leave unwanted remnants of objects in memory when switching scenes.
        /// It is best to do this in the Destroy() method of the registering objects.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        static public void RemoveBinding<T>(CommandCallback func) where T : Command
        {
            CDebug.Assert(func == null);

            var cmdType = typeof (T);
            if (!_callbackBindings.IsBound(cmdType)) return;

            var callback = _callbackBindings.Resolve(cmdType);
            callback -= func;
            _callbackBindings.Bind(cmdType, callback);
        }

        static private CommandCallback ResolveBinding(Type t)
        {
            CDebug.Assert(t.IsAssignableFrom(typeof(Command)));
            return !_callbackBindings.IsBound(t) ? null : _callbackBindings.Resolve(t);
        }

        /// <summary>
        /// Invokes the type's specific command bindings using cmd as the parameter
        /// </summary>
        /// <param name="t"></param>
        /// <param name="cmd"></param>
        static public void DispatchBindings(Type t, Command cmd)
        {
            CDebug.Assert(cmd == null);

            var callback = ResolveBinding(t);
            if (callback == null) return;
            callback.Invoke(cmd);
        }

    }

}
