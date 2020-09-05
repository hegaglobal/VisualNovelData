using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Commands
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

            if (command != null)
                Debug.LogWarning($"Command with key={key} is expected to be a {typeof(T)}, but it actually is a {command.GetType()}");
            else
                Debug.LogWarning($"Command with key={key} is expected to be a {typeof(T)}, but it actually is null");

            value = default;
            return false;
        }

        public void Invoke(Command command, int stage)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (CanSkip(command, stage))
                return;

            Invoke(command.Key, command.Metadata, command.ObjectParameters);
        }

        public void Invoke<T>(Command command, int stage) where T : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (CanSkip(command, stage))
                return;

            Invoke<T>(command.Key, command.Metadata, command.ObjectParameters);
        }

        public void Invoke(in Segment<Command> commands, int stage)
        {
            if (commands.Count <= 0)
                return;

            foreach (var command in commands)
            {
                if (CanSkip(command, stage))
                    continue;

                Invoke(command.Key, command.Metadata, command.ObjectParameters);
            }
        }

        public void Invoke<T>(in Segment<Command> commands, int stage) where T : ICommand
        {
            if (commands.Count <= 0)
                return;

            foreach (var command in commands)
            {
                if (CanSkip(command, stage))
                    continue;

                Invoke<T>(command.Key, command.Metadata, command.ObjectParameters);
            }
        }

        public void Invoke(string key, params object[] parameters)
            => Invoke(key, Metadata.None, parameters.AsSegment());

        public void Invoke(string key, in Metadata metadata, params object[] parameters)
            => Invoke(key, metadata, parameters.AsSegment());

        public void Invoke(string key, in Segment<object> parameters)
            => Invoke(key, Metadata.None, parameters);

        public void Invoke(string key, in Metadata metadata, in Segment<object> parameters)
        {
            if (!this.commands.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any command by key={key}");
                return;
            }

            this.commands[key].Invoke(metadata, parameters);
        }

        public void Invoke<T>(params object[] parameters) where T : ICommand
            => Invoke<T>(Metadata.None, parameters.AsSegment());

        public void Invoke<T>(in Metadata metadata, params object[] parameters) where T : ICommand
            => Invoke<T>(metadata, parameters.AsSegment());

        public void Invoke<T>(in Segment<object> parameters) where T : ICommand
            => Invoke(typeof(T).Name, Metadata.None, parameters);

        public void Invoke<T>(in Metadata metadata, in Segment<object> parameters) where T : ICommand
            => Invoke(typeof(T).Name, metadata, parameters);

        public void Invoke<T>(string key, params object[] parameters) where T : ICommand
            => Invoke<T>(key, Metadata.None, parameters.AsSegment());

        public void Invoke<T>(string key, in Metadata metadata, params object[] parameters) where T : ICommand
            => Invoke<T>(key, metadata, parameters.AsSegment());

        public void Invoke<T>(string key, in Segment<object> parameters) where T : ICommand
            => Invoke<T>(key, Metadata.None, parameters);

        public void Invoke<T>(string key, in Metadata metadata, in Segment<object> parameters) where T : ICommand
        {
            if (!this.commands.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any command by key={key}");
                return;
            }

            if (this.commands[key] is T command)
                command.Invoke(metadata, parameters);
        }

        private static bool CanSkip(Command command, int stage)
            => command == null || (command.MaxConstraint >= 0 && stage > command.MaxConstraint);
    }
}