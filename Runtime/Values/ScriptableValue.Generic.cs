using System;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Events;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif

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

		internal readonly DelegateHandlerList<OldNewValue<T>, T, T> onValueChangingEvents = new DelegateHandlerList<OldNewValue<T>, T, T>();
		internal readonly DelegateHandlerList<OldNewValue<T>, T, T> onValueChangedEvents = new DelegateHandlerList<OldNewValue<T>, T, T>();

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
			internal set { SetField(ref previousValue, value, previousValueChangingArgs, previousValueChangedArgs); }
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
			set { SetField(ref defaultValue, value, defaultValueChangingArgs, defaultValueChangedArgs); }
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
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

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
				NotifyPropertyChanging(valueChangingArgs);
			}

			// Update the value.
			value = newValue;
			temporaryValue = newValue;

			valueIsDefault = EqualityHelper.Equals(value, default);

			// Invoke the OnValueChanged event if notify is true.
			if (notify)
			{
				onValueChanged.Invoke(PreviousValue, newValue);
				onValueChangedEvents.Invoke(PreviousValue, Value);
				NotifyPropertyChanged(valueChangedArgs);
			}
		}

		/// <summary>
		///     Registers a callback to be called before <see cref="Value"/> changes.
		/// </summary>
		/// <param name="callback">The callback method to call.</param>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>.</exception>
		public void RegisterValueChangingListener(OldNewValue<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangingEvents.RegisterCallback(callback);
		}

		/// <summary>
		///     Registers a callback to be called before <see cref="Value" /> changes with additional context.
		/// </summary>
		/// <remarks>This method can be used to avoid closure allocations on your events.</remarks>
		/// <param name="callback">The callback method to call.</param>
		/// <param name="context">The context to pass to the callback.</param>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>. Or <paramref name="context" /> is <c>null</c>.</exception>
		public void RegisterValueChangingListener<TContext>(Action<T, T, TContext> callback, TContext context)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));
			ThrowHelper.ThrowIfNull(context, nameof(context));

			onValueChangingEvents.RegisterCallback(callback, context);
		}

		/// <summary>
		///     Unregisters a callback from the <see cref="Value" /> changing event.
		/// </summary>
		/// <param name="callback">The callback method to unregister.</param>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>.</exception>
		public void UnregisterValueChangingListener(OldNewValue<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangingEvents.RemoveCallback(callback);
		}

		/// <summary>
		///     Unregisters a callback with context from the <see cref="Value" /> changing event.
		/// </summary>
		/// <param name="callback">The callback method to unregister.</param>
		/// <typeparam name="TContext">The type of the context that was used in the callback.</typeparam>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>.</exception>
		public void UnregisterValueChangingListener<TContext>(Action<T, T, TContext> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangingEvents.RemoveCallback(callback);
		}

		/// <summary>
		///     Registers a callback to be called after <see cref="Value"/> has been changed.
		/// </summary>
		/// <param name="callback">The callback method to call.</param>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>.</exception>
		public void RegisterValueChangedListener(OldNewValue<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangedEvents.RegisterCallback(callback);
		}

		/// <summary>
		///     Registers a callback to be called after <see cref="Value" /> has been changed with additional context.
		/// </summary>
		/// <remarks>This method can be used to avoid closure allocations on your events.</remarks>
		/// <param name="callback">The callback method to call.</param>
		/// <param name="context">The context to pass to the callback.</param>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>. Or <paramref name="context" /> is <c>null</c>.</exception>
		public void RegisterValueChangedListener<TContext>(Action<T, T, TContext> callback, TContext context)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));
			ThrowHelper.ThrowIfNull(context, nameof(context));

			onValueChangedEvents.RegisterCallback(callback, context);
		}

		/// <summary>
		///     Unregisters a callback from the <see cref="Value" /> changed event.
		/// </summary>
		/// <param name="callback">The callback method to unregister.</param>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>.</exception>
		public void UnregisterValueChangedListener(OldNewValue<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangedEvents.RemoveCallback(callback);
		}

		/// <summary>
		///     Unregisters a callback with context from the <see cref="Value" /> changed event.
		/// </summary>
		/// <param name="callback">The callback method to unregister.</param>
		/// <typeparam name="TContext">The type of the context that was used in the callback.</typeparam>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <c>null</c>.</exception>
		public void UnregisterValueChangedListener<TContext>(Action<T, T, TContext> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onValueChangedEvents.RemoveCallback(callback);
		}

		/// <summary>
		///     Helper method to check if the new value is the same as the old value.
		/// </summary>
		/// <param name="newValue">Thew new value.</param>
		/// <param name="oldValue">The old value.</param>
		/// <returns><c>true</c> if the values are the same; otherwise, <c>false</c>.</returns>
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
		///     Sets the current value without invoking the <see cref="OnValueChanging"/> and <see cref="OnValueChanged"/> events.
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
		///     Warns if there are any left-over subscribers to the events.
		/// </summary>
		/// <remarks>This will only be called in the Unity editor and builds with the DEBUG flag.</remarks>
		protected override void WarnIfLeftOverSubscribers()
		{
			base.WarnIfLeftOverSubscribers();
			EventHelper.WarnIfLeftOverSubscribers(onValueChangingEvents, nameof(OnValueChanging), this);
			EventHelper.WarnIfLeftOverSubscribers(onValueChangedEvents, nameof(OnValueChanged), this);
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
				WarnIfLeftOverSubscribers();
			}
#endif

			onValueChangingEvents.Reset();
			onValueChangedEvents.Reset();
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
			WarnIfLeftOverSubscribers();
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