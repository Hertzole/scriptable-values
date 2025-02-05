#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif
using System;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     Base class for a ScriptableValue with a value.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
#if ODIN_INSPECTOR
	[Sirenix.OdinInspector.DrawWithUnity]
#endif
	public abstract partial class ScriptableValue<T> : ScriptableValue
	{
		public delegate void OldNewValue<in TValue>(TValue previousValue, TValue newValue);

		[SerializeField]
		[EditorTooltip("The current value. This can be changed at runtime.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal T value = default;
		[SerializeField]
		[EditorTooltip("The default value. This is used when the value is reset.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal T defaultValue = default;
		[SerializeField]
		[EditorTooltip("Called before the current value is set.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal UnityEvent<T, T> onValueChanging = new UnityEvent<T, T>();
		[SerializeField]
		[EditorTooltip("Called after the current value is set.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal UnityEvent<T, T> onValueChanged = new UnityEvent<T, T>();

		private bool valueIsDefault;

		private T previousValue;

		// This is mainly use for OnValidate weirdness.
		// We need to have another value that is the value right before it gets modified.
		private T temporaryValue = default;

		internal readonly EventHandlerList<T, T> onValueChangingEvents = new EventHandlerList<T, T>();
		internal readonly EventHandlerList<T, T> onValueChangedEvents = new EventHandlerList<T, T>();

		/// <summary>
		///     The current value. This can be changed at runtime.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public T Value
		{
			get { return GetValue(); }
			set
			{
				AddStackTrace(1);

				SetValue(value, true);
			}
		}
		/// <summary>
		///     The previous value before the current value was set.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public T PreviousValue
		{
			get { return previousValue; }
			internal set
			{
				if (!EqualityHelper.Equals(previousValue, value))
				{
					previousValue = value;
					NotifyPropertyChanged();
				}
			}
		}
		/// <summary>
		///     The default value. This is used when the value is reset.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public T DefaultValue
		{
			get { return defaultValue; }
			set
			{
				if (!EqualityHelper.Equals(defaultValue, value))
				{
					defaultValue = value;
					NotifyPropertyChanged();
				}
			}
		}

		/// <summary>
		///     Called before the current value is set.
		/// </summary>
		public event OldNewValue<T> OnValueChanging
		{
			add { RegisterValueChangingListener(value); }
			remove { UnregisterValueChangingListener(value); }
		}
		/// <summary>
		///     Called after the current value is set.
		/// </summary>
		public event OldNewValue<T> OnValueChanged
		{
			add { RegisterValueChangedListener(value); }
			remove { UnregisterValueChangedListener(value); }
		}

		private void OnDestroy()
		{
			onValueChangingEvents.Dispose();
			onValueChangedEvents.Dispose();
		}

		/// <summary>
		///     Returns the current value.
		/// </summary>
		/// <returns>The current value.</returns>
		protected virtual T GetValue()
		{
			return value;
		}

		/// <summary>
		///     Sets the current value to a new value.
		/// </summary>
		/// <param name="newValue">The new value that should be set.</param>
		/// <param name="notify">If true, the OnValueChanging/Changed events are invoked.</param>
		protected virtual void SetValue(T newValue, bool notify)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && IsReadOnly)
			{
				Debug.LogError($"'{name}' is marked as read only and cannot be changed at runtime.");
				return;
			}

			// If the equality check is enabled, we don't want to set the value if it's the same as the current value.
			if (SetEqualityCheck && IsSameValue(newValue, value))
			{
				return;
			}

			PreviousValue = temporaryValue;
			// Invoke the OnValueChanging event if notify is true.
			if (notify)
			{
				onValueChanging.Invoke(PreviousValue, newValue);
				onValueChangingEvents.Invoke(PreviousValue, newValue);
			}

			// Update the value.
			value = newValue;
			temporaryValue = newValue;

			NotifyPropertyChanged(nameof(Value));

			valueIsDefault = EqualityHelper.Equals(value, default);

			// Invoke the OnValueChanged event if notify is true.
			if (notify)
			{
				onValueChanged.Invoke(PreviousValue, newValue);
				onValueChangedEvents.Invoke(PreviousValue, Value);
			}
		}

		/// <summary>
		///     Registers a callback that is invoked before the value is changed.
		/// </summary>
		/// <param name="callback">The callback that is invoked before the value is changed.</param>
		/// '
		/// <exception cref="ArgumentNullException">The callback is null.</exception>
		public void RegisterValueChangingListener(OldNewValue<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangingEvents.AddListener(callback);
		}

		/// <summary>
		///     Registers a callback that is invoked before the value is changed with additional context. The context can be used
		///     to avoid closures.
		/// </summary>
		/// <param name="callback">The callback that is invoked before the value is changed.</param>
		/// <param name="args">The context that is passed to the callback.</param>
		/// <typeparam name="TArgs">The type of the context.</typeparam>
		/// <exception cref="ArgumentNullException">The callback is null.</exception>
		/// <exception cref="ArgumentNullException">The context is null.</exception>
		public void RegisterValueChangingListener<TArgs>(Action<T, T, TArgs> callback, TArgs args)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));
			ThrowHelper.ThrowIfNull(args, nameof(args));

			onValueChangingEvents.AddListener(callback, args);
		}

		/// <summary>
		///     Unregisters a callback from the OnValueChanging event.
		/// </summary>
		/// <param name="callback">The callback that should be unregistered.</param>
		public void UnregisterValueChangingListener(OldNewValue<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangingEvents.RemoveListener(callback);
		}

		/// <summary>
		///     Unregisters a callback from the OnValueChanging event.
		/// </summary>
		/// <param name="callback">The callback that should be unregistered.</param>
		/// <typeparam name="TArgs">The type of the context.</typeparam>
		public void UnregisterValueChangingListener<TArgs>(Action<T, T, TArgs> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangingEvents.RemoveListener(callback);
		}

		/// <summary>
		///     Registers a callback that is invoked after the value is changed.
		/// </summary>
		/// <param name="callback">The callback that is invoked after the value is changed.</param>
		public void RegisterValueChangedListener(OldNewValue<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangedEvents.AddListener(callback);
		}

		/// <summary>
		///     Registers a callback that is invoked after the value is changed with additional context. The context can be used to
		///     avoid closures.
		/// </summary>
		/// <param name="callback">The callback that is invoked after the value is changed.</param>
		/// <param name="args">The context that is passed to the callback.</param>
		/// <typeparam name="TArgs">The type of the context.</typeparam>
		public void RegisterValueChangedListener<TArgs>(Action<T, T, TArgs> callback, TArgs args)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));
			ThrowHelper.ThrowIfNull(args, nameof(args));

			onValueChangedEvents.AddListener(callback, args);
		}

		/// <summary>
		///     Unregisters a callback from the OnValueChanged event.
		/// </summary>
		/// <param name="callback">The callback that should be unregistered.</param>
		public void UnregisterValueChangedListener(OldNewValue<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangedEvents.RemoveListener(callback);
		}

		/// <summary>
		///     Unregisters a callback from the OnValueChanged event.
		/// </summary>
		/// <param name="callback">The callback that should be unregistered.</param>
		/// <typeparam name="TArgs">The type of the context.</typeparam>
		public void UnregisterValueChangedListener<TArgs>(Action<T, T, TArgs> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangedEvents.RemoveListener(callback);
		}

		protected bool IsSameValue(T newValue, T oldValue)
		{
			// If the new value is the default value and the old value isn't, we do allow setting a new value.
			// This is because Unity objects can be destroyed, but this change is not communicated to the ScriptableValue.
			// So if it previously existed but is now null and we want to set it to null, we should allow it.
			if (EqualityHelper.Equals(newValue, default) && !valueIsDefault)
			{
				return false;
			}

			return EqualityHelper.Equals(oldValue, newValue);
		}

		/// <summary>
		///     Sets the current value without invoking the OnValueChanging and OnValueChanged events.
		/// </summary>
		/// <param name="newValue">The new value.</param>
		public void SetValueWithoutNotify(T newValue)
		{
			AddStackTrace();

			SetValue(newValue, false);
		}

		protected override void OnStart()
		{
#if UNITY_EDITOR
			ResetStackTraces();
#endif

			// If the value should be reset, and it isn't a read only value, we set the value to the default value.
			if (ResetValueOnStart && !IsReadOnly)
			{
				ResetValue();
			}

			// Remove any subscribers that are left over from play mode.
			// Don't warn if there are any subscribers left over because we already do that in OnExitPlayMode.
			ClearSubscribers();
		}

		/// <summary>
		///     Resets the value to the default value.
		/// </summary>
		public void ResetValue()
		{
			value = DefaultValue;
			PreviousValue = DefaultValue;
			temporaryValue = DefaultValue;
			valueIsDefault = EqualityHelper.Equals(value, default);
		}

		/// <summary>
		///     Removes any subscribers from the event.
		/// </summary>
		/// <param name="warnIfLeftOver">
		///     If true, a warning will be printed in the console if there are any subscribers.
		///     The warning will only be printed in the editor and debug builds.
		/// </param>
		public void ClearSubscribers(bool warnIfLeftOver = false)
		{
#if DEBUG
			if (warnIfLeftOver)
			{
				EventHelper.WarnIfLeftOverSubscribers(onValueChangingEvents, nameof(OnValueChanging), this);
				EventHelper.WarnIfLeftOverSubscribers(onValueChangedEvents, nameof(OnValueChanged), this);
			}
#endif

			onValueChangingEvents.ClearListeners();
			onValueChangedEvents.ClearListeners();
		}

#if UNITY_INCLUDE_TESTS
		/// <summary>
		///     A test only check to see if <see cref="OnValueChanging" /> has subscribers.
		/// </summary>
		internal bool ValueChangingHasSubscribers
		{
			get { return onValueChangingEvents.ListenersCount > 0; }
		}
		/// <summary>
		///     A test only check to see if <see cref="OnValueChanged" /> has subscribers.
		/// </summary>
		internal bool ValueChangedHasSubscribers
		{
			get { return onValueChangedEvents.ListenersCount > 0; }
		}
#endif

#if UNITY_EDITOR
		protected override void OnExitPlayMode()
		{
			EventHelper.WarnIfLeftOverSubscribers(onValueChangingEvents, nameof(OnValueChanging), this);
			EventHelper.WarnIfLeftOverSubscribers(onValueChangedEvents, nameof(OnValueChanged), this);
		}

		internal void CallOnValidate_TestOnly()
		{
			OnValidate();
		}

		protected virtual void OnValidate()
		{
			SetValueOnValidateInternal();
		}

		private void SetValueOnValidateInternal()
		{
			bool equals = false;
			bool originalSetEqualityCheck = SetEqualityCheck;
			// Only do an equality check if we're supposed to and that determines if we can set the new value or not.
			if (SetEqualityCheck)
			{
				equals = EqualityHelper.Equals(PreviousValue, value);
				// We need to turn off the equality check in OnValidate to make sure the value is set and the events are invoked.
				SetEqualityCheck = false;
			}

			// If we can set the value, set it.
			if (!equals)
			{
				SetValue(value, true);
			}

			// Restore the original value.
			SetEqualityCheck = originalSetEqualityCheck;
		}
#endif
	}
}