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

		private T currentArgs;
		
		public T PreviousArgs { get; private set; }
		
		public new event EventHandler<T> OnInvoked;

		public void Invoke(object sender, T args)
		{
			PreviousArgs = currentArgs;
			currentArgs = args;
		
			OnInvoked?.Invoke(sender, args);
			onInvokedWithArgs.Invoke(args);
		}

		public override void ResetValues()
		{
			base.ResetValues();

#if DEBUG
			EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);
#endif

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
	public class ScriptableEvent : RuntimeScriptableObject
	{
		[SerializeField]
		private UnityEvent onInvoked = new UnityEvent();

		public event EventHandler OnInvoked;

		public void Invoke(object sender)
		{
			OnInvoked?.Invoke(sender, EventArgs.Empty);
			onInvoked.Invoke();
		}

		public override void ResetValues()
		{
#if DEBUG
			EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);
#endif

			OnInvoked = null;
		}
	}
}