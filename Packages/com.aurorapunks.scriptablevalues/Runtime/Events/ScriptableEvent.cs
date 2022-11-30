using System;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptableEvent<T> : ScriptableEvent
	{
		[SerializeField]
		private UnityEvent<T> onInvokedWithArgs = new UnityEvent<T>();

#if UNITY_EDITOR
#pragma warning disable CS0414 // The field is assigned but its value is never used
		[SerializeField]
		private T editorInvokeValue = default;
#pragma warning restore CS0414 // The field is assigned but its value is never used
#endif

		private T currentArgs;

		public T PreviousArgs { get; private set; }

		public new event EventHandler<T> OnInvoked;
		
#if UNITY_INCLUDE_TESTS
		internal new bool InvokedHasSubscribers { get { return OnInvoked != null; } }
#endif

		// We must override the base class' invoke method to ensure that the event is invoked with the correct arguments.
		public new void Invoke(object sender)
		{
			// Skip a frame to avoid the Invoke method itself being included in the stack trace.
			AddStackTrace(1);
			
			PreviousArgs = currentArgs;
			currentArgs = default;

			OnInvoked?.Invoke(sender, currentArgs);
			onInvokedWithArgs.Invoke(currentArgs);
		}
		
		public void Invoke(object sender, T args)
		{
			// Skip a frame to avoid the Invoke method itself being included in the stack trace.
			AddStackTrace(1);

			PreviousArgs = currentArgs;
			currentArgs = args;

			OnInvoked?.Invoke(sender, args);
			onInvokedWithArgs.Invoke(args);
		}

		public override void ResetValues()
		{
			base.ResetValues();

			OnInvoked = null;

			PreviousArgs = default;
		}

#if UNITY_EDITOR
		protected override void OnExitPlayMode()
		{
			base.OnExitPlayMode();
			EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);
		}
#endif
	}

#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Runtime Event", menuName = "Aurora Punks/Scriptable Values/Events/Runtime Event", order = 1100)]
#endif
	public class ScriptableEvent : RuntimeScriptableObject
	{
		[SerializeField]
		private UnityEvent onInvoked = new UnityEvent();

		public event EventHandler OnInvoked;
		
#if UNITY_INCLUDE_TESTS
		internal bool InvokedHasSubscribers { get { return OnInvoked != null; } }
#endif

		public void Invoke()
		{
			Invoke(this);
		}

		public void Invoke(object sender)
		{
			// Skip a frame to avoid the Invoke method itself being included in the stack trace.
			AddStackTrace(1);

			OnInvoked?.Invoke(sender, EventArgs.Empty);
			onInvoked.Invoke();
		}

		public override void ResetValues()
		{
			ResetStackTraces();

			OnInvoked = null;
		}
		
#if UNITY_EDITOR
		protected override void OnExitPlayMode()
		{
			base.OnExitPlayMode();
			EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);
		}
#endif
	}
}