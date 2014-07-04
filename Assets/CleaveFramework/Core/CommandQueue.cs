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

        private readonly Queue<CommandContainer> _q = new Queue<CommandContainer>();

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

            var processCmd = false;
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

            cmd.Cmd.Execute();
            return _q.Count > 0;

        }

    }
}
