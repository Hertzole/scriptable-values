#if (FISHNET || SCRIPTABLE_VALUES_NGO)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING
using System;
using Hertzole.ScriptableValues.Helpers;

namespace Hertzole.ScriptableValues
{
	public sealed partial class SyncedScriptableValue<T> : IDisposable
	{
		private bool isInitialized;

		private ScriptableValue<T> targetValue;

		public void Initialize(ScriptableValue<T> value)
		{
			ThrowHelper.ThrowIfNull(value, nameof(value));

			targetValue = value;

			targetValue.OnValueChanging += OnValueChanging;

			isInitialized = true;

			OnInitialized();
		}

		partial void OnInitialized();

		partial void OnValueChanging(T previousValue, T newValue);

		private void DisposeScriptableValue()
		{
			if (targetValue != null)
			{
				targetValue.OnValueChanging -= OnValueChanging;
				targetValue = null;
			}

			isInitialized = false;
		}
	}
}
#endif