#if (FISHNET || SCRIPTABLE_VALUES_NGO)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING
using System;

namespace Hertzole.ScriptableValues
{
	public sealed partial class SyncedScriptableValue<T> : IDisposable
	{
		private ScriptableValue<T> targetValue;

		public void Initialize(ScriptableValue<T> value)
		{
			targetValue = value;
			
			targetValue.OnValueChanging += OnValueChanging;
		}

		partial void OnValueChanging(T previousValue, T newValue);

		private void DisposeScriptableValue()
		{
			targetValue.OnValueChanging -= OnValueChanging;
		}
	}
}
#endif