using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.EventSystems
{
    using VisualEvent = Data.Event;

    public sealed class EventSystem
    {
        private readonly Dictionary<string, IEvent> events = new Dictionary<string, IEvent>();

        public EventSystem Register(string key, IEvent @event, bool shouldOverride = false)
        {
            if (key == null)
            {
                Debug.LogWarning("Cannot register the event with a null key");
                return this;
            }

            if (!this.events.ContainsKey(key))
            {
                this.events.Add(key, @event);
                return this;
            }

            if (this.events[key] == null)
            {
                this.events[key] = @event;
                return this;
            }

            if (shouldOverride)
            {
                this.events[key] = @event;
                return this;
            }

            Debug.Log($"An event has been registered with key={key}");
            return this;
        }

        public EventSystem Register<T>(string key, bool shouldOverride = false) where T : IEvent, new()
            => Register(key, new T(), shouldOverride);

        public EventSystem Register<T>(bool shouldOverride = false) where T : IEvent, new()
            => Register(typeof(T).Name, new T(), shouldOverride);

        public EventSystem Remove(string key)
        {
            if (key == null)
            {
                Debug.LogWarning("Cannot remove any event by a null key");
                return this;
            }

            if (!this.events.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any event by key={key}");
                return this;
            }

            this.events.Remove(key);
            return this;
        }

        public EventSystem Remove<T>(string key)
        {
            if (key == null)
            {
                Debug.LogWarning("Cannot remove any event by a null key");
                return this;
            }

            if (!this.events.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any event by key={key}");
                return this;
            }

            if (this.events[key] == null)
            {
                this.events.Remove(key);
                return this;
            }

            if (this.events[key] is T)
            {
                this.events.Remove(key);
                return this;
            }

            Debug.LogWarning($"The event of key={key} is not an instance of {typeof(T)}");
            return this;
        }

        public EventSystem Remove<T>() where T : IEvent
            => Remove(typeof(T).Name);

        public bool Contains(string key)
            => this.events.ContainsKey(key);

        public bool Contains<T>(string key) where T : IEvent
        {
            if (!this.events.ContainsKey(key))
                return false;

            return this.events[key] is T;
        }

        public bool Contains<T>() where T : IEvent
            => this.events.ContainsKey(typeof(T).Name);

        public IEvent GetEvent(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (!this.events.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any event by key={key}");
                return null;
            }

            return this.events[key];
        }

        public bool TryGetEvent<T>(string key, out T value) where T : IEvent
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (!this.events.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any event by key={key}");
                value = default;
                return false;
            }

            var ev = this.events[key];

            if (ev is T t)
            {
                value = t;
                return true;
            }

            Debug.LogWarning($"Event with key={key} is expected to be a {typeof(T)}, but it actually is a {ev.GetType()}");
            value = default;
            return false;
        }

        public void Invoke(VisualEvent @event, int progress)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            if (CanSkip(@event, progress))
                return;

            Invoke(@event.Type, @event.ObjectParameters);
        }

        public void Invoke<T>(VisualEvent @event, int progress) where T : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            if (CanSkip(@event, progress))
                return;

            Invoke<T>(@event.Type, @event.ObjectParameters);
        }

        public void Invoke(in Segment<VisualEvent> events, int progress)
        {
            if (events.Count <= 0)
                return;

            foreach (var @event in events)
            {
                if (CanSkip(@event, progress))
                    continue;

                Invoke(@event.Type, @event.ObjectParameters);
            }
        }

        public void Invoke<T>(in Segment<VisualEvent> events, int progress) where T : IEvent
        {
            if (events.Count <= 0)
                return;

            foreach (var @event in events)
            {
                if (CanSkip(@event, progress))
                    continue;

                Invoke<T>(@event.Type, @event.ObjectParameters);
            }
        }

        public void Invoke(string key, params object[] parameters)
            => Invoke(key, parameters.AsSegment());

        public void Invoke(string key, in Segment<object> parameters)
        {
            if (!this.events.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any event by key={key}");
                return;
            }

            this.events[key].Invoke(parameters);
        }

        public void Invoke<T>(params object[] parameters) where T : IEvent
            => Invoke<T>(parameters.AsSegment());

        public void Invoke<T>(in Segment<object> parameters) where T : IEvent
            => Invoke(typeof(T).Name, parameters);

        public void Invoke<T>(string key, params object[] parameters) where T : IEvent
            => Invoke<T>(key, parameters.AsSegment());

        public void Invoke<T>(string key, in Segment<object> parameters) where T : IEvent
        {
            if (!this.events.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot find any event by key={key}");
                return;
            }

            var e = this.events[key];

            if (e is T)
                e.Invoke(parameters);
        }

        private static bool CanSkip(VisualEvent @event, int progress)
            => @event == null ||
               (@event.MaxConstraint >= 0 && progress > @event.MaxConstraint);
    }
}