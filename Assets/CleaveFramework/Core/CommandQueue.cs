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
using CleaveFramework.Commands;
using UnityEngine;

namespace CleaveFramework.Core
{
    /// <summary>
    /// CommandQueue contains logic capable of processing the commands pushed into the application
    /// </summary>
    class CommandQueue
    {
        /// <summary>
        /// Container for an executable command
        /// </summary>
        class CommandContainer
        {
            public readonly Command Cmd;
            public readonly int FrameCalled;
            public readonly int ExecuteDelayFrame;
            public readonly float TimeCalled;
            public readonly float ExecuteDelayTime;

            public enum DelayTypes
            {
                Frame,
                Time,
            }

            public readonly DelayTypes DelayType;

            public CommandContainer(Command cmd, int executeDelay)
            {
                FrameCalled = Time.frameCount;
                ExecuteDelayFrame = executeDelay;
                DelayType = DelayTypes.Frame;
                Cmd = cmd;
            }
            public CommandContainer(Command cmd, float executeDelay)
            {
                TimeCalled = Time.timeSinceLevelLoad;
                ExecuteDelayTime = executeDelay;
                DelayType = DelayTypes.Time;
                Cmd = cmd;
            }
        }

        private readonly Queue<CommandContainer> _q; 
        public CommandQueue()
        {
            _q = new Queue<CommandContainer>();
        }

        /// <summary>
        /// process every command in the queue
        /// </summary>
        public void Process()
        {
            while (ProcessCommand()) { }
        }

        /// <summary>
        /// process up to command # in the queue and then return
        /// will return early if not enough commands were waiting for process
        /// </summary>
        /// <param name="commands">how many commands to execute</param>
        public void Process(int commands)
        {
            for (int index = 0; index < commands; index++)
            {
                if (!ProcessCommand()) return;
            }
        }

        /// <summary>
        /// Accessor function for putting commands into the game queue
        /// </summary>
        /// <param name="cmd"></param>
        public void Push(Command cmd)
        {
            _q.Enqueue(new CommandContainer(cmd, 0));
        }

        /// <summary>
        /// Enqueue a command with a delayed execution in frames
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="frameDelay"></param>
        public void Push(Command cmd, int frameDelay)
        {
            _q.Enqueue(new CommandContainer(cmd, frameDelay));
        }

        /// <summary>
        /// Enqueue a command with a delayed execution in seconds
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="timeDelay"></param>
        public void Push(Command cmd, float timeDelay)
        {
            _q.Enqueue(new CommandContainer(cmd, timeDelay));
        }

        private int _delayedCommands;
        /// <summary>
        /// Pops the top Command from the queue and executes it
        /// </summary>
        /// <returns></returns>
        bool ProcessCommand()
        {
            if (_q.Count <= 0)
            {
                _delayedCommands = 0;
                return _q.Count > 0;
            }
            var cmd = _q.Dequeue();

            bool processCmd = false;
            switch (cmd.DelayType)
            {
                case CommandContainer.DelayTypes.Frame:
                    processCmd = Time.frameCount - cmd.FrameCalled >= cmd.ExecuteDelayFrame;
                    break;

                case CommandContainer.DelayTypes.Time:
                    processCmd = Time.timeSinceLevelLoad - cmd.TimeCalled >= cmd.ExecuteDelayTime;
                    break;
            }
            if (!processCmd)
            {
                _delayedCommands++;
                _q.Enqueue(cmd);
                return _q.Count > _delayedCommands;
            }
            else
            {
//                 Debug.Log("Executing: " + cmd.Cmd.ToString());
                cmd.Cmd.Execute();
                return _q.Count > 0;
            }

        }

    }
}
