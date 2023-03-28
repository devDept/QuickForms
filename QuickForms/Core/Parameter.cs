using System;

namespace QuickForms.Core
{
    /// <summary>
    /// Represents a property of a control.
    /// </summary>
    /// <typeparam name="T">Type of the parameter.</typeparam>
    public class Parameter<T>
    {
        /// <summary>
        /// Sets the property in the GUI control.
        /// </summary>
        private readonly Action<T> _setter;

        /// <summary>
        /// Gets the property in the GUI control.
        /// </summary>
        private readonly Func<T> _getter;

        public Parameter(Func<T> getter, Action<T> setter, Trigger<T>? click = null, Trigger<T>? change = null, Trigger<T>? gotFocus = null, Trigger<T>? lostFocus = null)
        {
            _getter = getter;
            _setter = setter;

            Trigger<T> emptyTrigger = new Trigger<T>();
            
            if (gotFocus != null) gotFocus.Parameter = this;
            if (lostFocus != null) lostFocus.Parameter = this;
            if (click != null) click.Parameter = this;
            if (change!= null) change.Parameter = this;

            _gotFocus = gotFocus ?? emptyTrigger;
            _lostFocus = lostFocus ?? emptyTrigger;
            _click = click ?? emptyTrigger;
            _change = change ?? emptyTrigger;
        }

        /// <summary>
        /// Gets or sets the current value. Setting the value will update the GUI control
        /// but it will not trigger the callback.
        /// </summary>
        public T Value
        {
            get => _getter();
            set
            {
                // this setter is used by the user via code,
                // we do not want to trigger the change event
                _change.Enable = false;
                _setter(value);
                _change.Enable = true;
            }
        }

        // event triggers
        private readonly Trigger<T> _gotFocus;
        private readonly Trigger<T> _lostFocus;
        private readonly Trigger<T> _click;
        private readonly Trigger<T> _change;
        
        /// <summary>
        /// Adds an event handler invoked on focus got.
        /// </summary>
        public Parameter<T> FocusGot(Action<T> action)
        {
            _gotFocus.Add(action);
            return this;
        }

        /// <summary>
        /// Adds an event handler invoked on focus lost.
        /// </summary>
        public Parameter<T> FocusLost(Action<T> action)
        {
            _lostFocus.Add(action);
            return this;
        }

        /// <summary>
        /// Adds an event handler invoked on click.
        /// </summary>
        public Parameter<T> Click(Action<T> action)
        {
            _click.Add(action);
            return this;
        }

        /// <summary>
        /// Adds an event handler invoked on value change.
        /// </summary>
        public Parameter<T> Change(Action<T> action)
        {
            _change.Add(action);
            return this;
        }
    }

    public class Trigger<T>
    {
        private event Action<T>? _event;
        
        public Parameter<T>? Parameter { get; set; }

        public bool Enable { get; set; } = true;

        public void Add(Action<T> callback)
        {
            _event += callback;
        }

        public void Notify()
        {
            if(Enable && Parameter != null)
                _event?.Invoke(Parameter.Value);
        }
    }
}
