using System;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using System.Diagnostics;
#endif

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptableEvent<T> : ScriptableEvent
	{
		[SerializeField]
		private UnityEvent<T> onInvokedWithArgs = new UnityEvent<T>();

		private T currentArgs;
		
		public T PreviousArgs { get; private set; }
		
		public new event EventHandler<T> OnInvoked;

		public void Invoke(object sender, T args)
		{
#if UNITY_EDITOR
			// Skip a frame to avoid the Invoke method itself being included in the stack trace.
			AddStackTrace(new StackTrace(1, true));
#endif
			
			PreviousArgs = currentArgs;
			currentArgs = args;
		
			OnInvoked?.Invoke(sender, args);
			onInvokedWithArgs.Invoke(args);
		}

		public override void ResetValues()
		{
			base.ResetValues();
			
			EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);

			PreviousArgs = default;
			OnInvoked = null;
		}

		private void Reset()
		{
			OnInvoked = null;
		}
	}

#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Runtime Event", menuName = "Aurora Punks/Scriptable Values/Events/Runtime Event", order = 1100)]
#endif
	public partial class ScriptableEvent : RuntimeScriptableObject
	{
		[SerializeField]
		private UnityEvent onInvoked = new UnityEvent();

		public event EventHandler OnInvoked;
		
		public void Invoke(object sender)
		{
#if UNITY_EDITOR
			// Skip a frame to avoid the Invoke method itself being included in the stack trace.
			AddStackTrace(new StackTrace(1, true));
#endif

			OnInvoked?.Invoke(sender, EventArgs.Empty);
			onInvoked.Invoke();
		}

		public override void ResetValues()
		{
#if UNITY_EDITOR
			ResetStackTraces();
#endif
			
			EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);

			OnInvoked = null;
		}
	}
}