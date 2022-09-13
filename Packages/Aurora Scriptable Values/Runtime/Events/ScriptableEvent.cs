using System;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptableEvent<T> : RuntimeScriptableObject
	{
		[SerializeField] 
		private UnityEvent<T> onInvoked = new UnityEvent<T>();
		
		public event EventHandler<T> OnInvoked;

		public void Invoke(object sender, T args)
		{
			OnInvoked?.Invoke(sender, args);
			onInvoked.Invoke(args);
		}

		public override void ResetValues()
		{
#if DEBUG
			EventHelper.WarnIfLeftOverSubscribers(OnInvoked, nameof(OnInvoked), this);
#endif
			
			OnInvoked = null;
		}

		private void Reset()
		{
			OnInvoked = null;
		}
	}

#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Runtime Event", menuName = "Aurora Punks/Scriptable Values/Events/Runtime Event")]
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