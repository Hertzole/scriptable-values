using System;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     Listens to a <see cref="ScriptableEvent" /> without arguments.
	/// </summary>
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Event Listener", 1100)]
#endif
	public class ScriptableEventListener : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("The event to listen to.")]
		private ScriptableEvent targetEvent = default;
		[SerializeField]
		[Tooltip("When the listener should start listening.")]
		private StartListenEvents startListening = StartListenEvents.Awake;
		[SerializeField]
		[Tooltip("When the listener should stop listening.")]
		private StopListenEvents stopListening = StopListenEvents.OnDestroy;
		[SerializeField]
		[Tooltip("The event to invoke when the target event is raised.")]
		private UnityEvent onInvoked = new UnityEvent();

		/// <summary>
		///     Is the listener currently listening to the target value?
		/// </summary>
		public bool IsListening { get; private set; } = false;

		/// <summary>
		///     The event to listen to.
		/// </summary>
		public ScriptableEvent TargetEvent { get { return targetEvent; } set { SetTargetEvent(value); } }
		/// <summary>
		///     When the listener should start listening.
		/// </summary>
		public StartListenEvents StartListening { get { return startListening; } set { startListening = value; } }
		/// <summary>
		///     When the listener should stop listening.
		/// </summary>
		public StopListenEvents StopListening { get { return stopListening; } set { stopListening = value; } }

		/// <summary>
		///     The event to invoke when the target event is raised.
		/// </summary>
		public UnityEvent OnInvoked { get { return onInvoked; } }

		protected virtual void Awake()
		{
			IsListening = false;

			if (!IsListening && startListening == StartListenEvents.Awake)
			{
				ToggleListening(true);
			}
		}

		protected void Start()
		{
			if (!IsListening && startListening == StartListenEvents.Start)
			{
				ToggleListening(true);
			}
		}

		protected virtual void OnEnable()
		{
			if (!IsListening && startListening == StartListenEvents.OnEnable)
			{
				ToggleListening(true);
			}
		}

		protected virtual void OnDisable()
		{
			if (IsListening && stopListening == StopListenEvents.OnDisable)
			{
				ToggleListening(false);
			}
		}

		protected virtual void OnDestroy()
		{
			if (IsListening && stopListening == StopListenEvents.OnDestroy)
			{
				ToggleListening(false);
			}
		}

		protected virtual void ToggleListening(bool listen)
		{
			IsListening = listen;

			if (targetEvent == null)
			{
				return;
			}

			if (listen)
			{
				targetEvent.OnInvoked += OnEventInvoked;
			}
			else
			{
				targetEvent.OnInvoked -= OnEventInvoked;
			}
		}

		/// <summary>
		///     Sets the target event.
		/// </summary>
		/// <param name="newEvent">The new event.</param>
		protected virtual void SetTargetEvent(ScriptableEvent newEvent)
		{
			// If the target event is the same, do nothing.
			if (newEvent == targetEvent)
			{
				return;
			}

			// If we are currently listening to the old event, stop listening.
			if (targetEvent != null && IsListening)
			{
				targetEvent.OnInvoked -= OnEventInvoked;
			}

			targetEvent = newEvent;

			// If we should start listening to the new event, start listening.
			if (targetEvent != null && IsListening)
			{
				targetEvent.OnInvoked += OnEventInvoked;
			}
		}

		/// <summary>
		///     Called when the target event is invoked.
		/// </summary>
		/// <param name="sender">The object that invoked the event.</param>
		/// <param name="e">The arguments.</param>
		private void OnEventInvoked(object sender, EventArgs e)
		{
			onInvoked.Invoke();
		}
	}
}