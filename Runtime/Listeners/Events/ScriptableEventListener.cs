#nullable enable

using System;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     Listens to a <see cref="ScriptableEvent" /> without arguments.
    /// </summary>
#if UNITY_EDITOR
    [AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Event Listener", 1100)]
#endif
    public class ScriptableEventListener : ScriptableListenerBase
    {
        [SerializeField]
        [EditorTooltip("The event to listen to.")]
        private ScriptableEvent? targetEvent = null;
        [SerializeField]
        [EditorTooltip("The event to invoke when the target event is raised.")]
        private UnityEvent onInvoked = new UnityEvent();

        /// <summary>
        ///     The event to listen to.
        /// </summary>
        public ScriptableEvent? TargetEvent
        {
            get { return targetEvent; }
            set { SetTargetEvent(value); }
        }

        /// <summary>
        ///     The event to invoke when the target event is raised.
        /// </summary>
        public UnityEvent OnInvoked
        {
            get { return onInvoked; }
        }

        /// <inheritdoc />
        protected sealed override void SetListening(bool listen)
        {
            base.SetListening(listen);

            if (targetEvent == null)
            {
                return;
            }

            SetListeningToObject(targetEvent, listen);
        }

        /// <summary>
        ///     Sets the target event.
        /// </summary>
        /// <param name="newEvent">The new event.</param>
        private void SetTargetEvent(ScriptableEvent? newEvent)
        {
            // If the target event is the same, do nothing.
            if (newEvent == targetEvent)
            {
                return;
            }

            // If we are currently listening to the old event, stop listening.
            if (targetEvent != null && IsListening)
            {
                SetListeningToObject(targetEvent, false);
            }

            targetEvent = newEvent;

            // If we should start listening to the new event, start listening.
            if (targetEvent != null && IsListening)
            {
                SetListeningToObject(targetEvent, true);
            }
        }

        private void SetListeningToObject(ScriptableEvent target, bool listen)
        {
            Guard.IsNotNull(target, nameof(target));

            if (listen)
            {
                target.OnInvoked += OnEventInvoked;
            }
            else
            {
                target.OnInvoked -= OnEventInvoked;
            }
        }

        /// <summary>
        ///     Called when the target event is invoked.
        /// </summary>
        private void OnEventInvoked(object sender, EventArgs eventArgs)
        {
            if (!OnBeforeEventInvoked(sender))
            {
                return;
            }

            onInvoked.Invoke();

            OnAfterEventInvoked(sender);
        }

        /// <summary>
        ///     Called before the event is invoked.
        /// </summary>
        /// <param name="sender">The object that sent the event.</param>
        /// <returns><c>true</c> if the event should be invoked; otherwise, <c>false</c>.</returns>
        protected virtual bool OnBeforeEventInvoked(object sender)
        {
            return true;
        }

        /// <summary>
        ///     Called after the event is invoked.
        /// </summary>
        /// <param name="sender">The object that sent the event.</param>
        protected virtual void OnAfterEventInvoked(object sender) { }
    }
}