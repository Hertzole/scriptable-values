using System;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
#if UNITY_EDITOR
	[AddComponentMenu("Scriptable Values/Listeners/Events/Scriptable Event Listener", 1100)]
#endif
	public class ScriptableEventListener : MonoBehaviour
	{
		[SerializeField]
		private ScriptableEvent targetEvent = default;
		[SerializeField]
		private StartListenEvents startListening = StartListenEvents.Awake;
		[SerializeField]
		private StopListenEvents stopListening = StopListenEvents.OnDestroy;
		[SerializeField]
		private UnityEvent onInvoked = new UnityEvent();

		public bool IsListening { get; private set; } = false;

		public ScriptableEvent TargetEvent { get { return targetEvent; } set { SetTargetEvent(value); } }
		public StartListenEvents StartListening { get { return startListening; } set { startListening = value; } }
		public StopListenEvents StopListening { get { return stopListening; } set { stopListening = value; } }

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

		protected virtual void SetTargetEvent(ScriptableEvent newEvent)
		{
			if (newEvent == targetEvent)
			{
				return;
			}
			
			if (targetEvent != null && IsListening)
			{
				targetEvent.OnInvoked -= OnEventInvoked;
			}
			
			targetEvent = newEvent;
			
			if (targetEvent != null && IsListening)
			{
				targetEvent.OnInvoked += OnEventInvoked;
			}
		}

		private void OnEventInvoked(object sender, EventArgs e)
		{
			onInvoked.Invoke();
		}
	}
}