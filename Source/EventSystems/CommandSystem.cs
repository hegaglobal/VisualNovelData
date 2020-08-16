using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.CommandSystems
{
    using Data;

    public sealed class CommandSystem
    {
        private readonly Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        public CommandSystem Register(string key, ICommand command, bool shouldOverride = false)
        {
            if (key == null)
            {
                Debug.LogWarning("Cannot register the command with a null key");
                return this;
            }

            if (!this.commands.ContainsKey(key))
            {
                this.commands.Add(key, command);
                return this;
            }

            if (this.commands[key] == null)
            {
                this.commands[key] = command;
                return this;
            }

            if (shouldOverride)
            {
                this.commands[key] = command;
                return this;
            }

            Debug.Log($"An command has been registered with key={key}");
            return this;
        }

        public CommandSystem Register<T>(string key, bool shouldOverride = false) where T : ICommand, new()
            => Register(key, new T(), shouldOverride);

        public CommandSystem Register<T>(bool shouldOverride = false) where T : ICommand, new()
            => Register(typeof(T).Name, new T(), shouldOverride);

        public CommandSystem Remove(string key)
        {
            if (key == null)
            {
                Debug.LogWarning("Cannot remove any command by a null key");
                return this;
            }

            if (!this.commands.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any command by key={key}");
                return this;
            }

            this.commands.Remove(key);
            return this;
        }

        public CommandSystem Remove<T>(string key)
        {
            if (key == null)
            {
                Debug.LogWarning("Cannot remove any command by a null key");
                return this;
            }

            if (!this.commands.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any command by key={key}");
                return this;
            }

            if (this.commands[key] == null)
            {
                this.commands.Remove(key);
                return this;
            }

            if (this.commands[key] is T)
            {
                this.commands.Remove(key);
                return this;
            }

            Debug.LogWarning($"The command of key={key} is not an instance of {typeof(T)}");
            return this;
        }

        public CommandSystem Remove<T>() where T : ICommand
            => Remove(typeof(T).Name);

        public bool Contains(string key)
            => this.commands.ContainsKey(key);

        public bool Contains<T>(string key) where T : ICommand
        {
            if (!this.commands.ContainsKey(key))
                return false;

            return this.commands[key] is T;
        }

        public bool Contains<T>() where T : ICommand
            => this.commands.ContainsKey(typeof(T).Name);

        public ICommand GetCommand(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (!this.commands.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any command by key={key}");
                return null;
            }

            return this.commands[key];
        }

        public bool TryGetCommand<T>(string key, out T value) where T : ICommand
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (!this.commands.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any command by key={key}");
                value = default;
                return false;
            }

            var command = this.commands[key];

            if (command is T t)
            {
                value = t;
                return true;
            }

            Debug.LogWarning($"Command with key={key} is expected to be a {typeof(T)}, but it actually is a {command.GetType()}");
            value = default;
            return false;
        }

        public void Invoke(Command command, int progress)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (CanSkip(command, progress))
                return;

            Invoke(command.Type, command.ObjectParameters);
        }

        public void Invoke<T>(Command command, int progress) where T : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (CanSkip(command, progress))
                return;

            Invoke<T>(command.Type, command.ObjectParameters);
        }

        public void Invoke(in Segment<Command> commands, int progress)
        {
            if (commands.Count <= 0)
                return;

            foreach (var command in commands)
            {
                if (CanSkip(command, progress))
                    continue;

                Invoke(command.Type, command.ObjectParameters);
            }
        }

        public void Invoke<T>(in Segment<Command> commands, int progress) where T : ICommand
        {
            if (commands.Count <= 0)
                return;

            foreach (var command in commands)
            {
                if (CanSkip(command, progress))
                    continue;

                Invoke<T>(command.Type, command.ObjectParameters);
            }
        }

        public void Invoke(string key, params object[] parameters)
            => Invoke(key, parameters.AsSegment());

        public void Invoke(string key, in Segment<object> parameters)
        {
            if (!this.commands.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any command by key={key}");
                return;
            }

            this.commands[key].Invoke(parameters);
        }

        public void Invoke<T>(params object[] parameters) where T : ICommand
            => Invoke<T>(parameters.AsSegment());

        public void Invoke<T>(in Segment<object> parameters) where T : ICommand
            => Invoke(typeof(T).Name, parameters);

        public void Invoke<T>(string key, params object[] parameters) where T : ICommand
            => Invoke<T>(key, parameters.AsSegment());

        public void Invoke<T>(string key, in Segment<object> parameters) where T : ICommand
        {
            if (!this.commands.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any command by key={key}");
                return;
            }

            var e = this.commands[key];

            if (e is T)
                e.Invoke(parameters);
        }

        private static bool CanSkip(Command command, int progress)
            => command == null ||
               (command.MaxConstraint >= 0 && progress > command.MaxConstraint);
    }
}