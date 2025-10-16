#nullable enable

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
    ///     A <see cref="ScriptableValue{T}" /> that holds a value of type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
#if ODIN_INSPECTOR
	[Sirenix.OdinInspector.DrawWithUnity]
#endif
    public abstract partial class ScriptableValue<T> : ScriptableValue
    {
        [SerializeField]
        [EditorTooltip("The current value. This can be changed at runtime.")]
#if SCRIPTABLE_VALUES_PROPERTIES
        [DontCreateProperty]
#endif
        internal T value = default!;
        [SerializeField]
        [EditorTooltip("The default value. This is used when the value is reset.")]
#if SCRIPTABLE_VALUES_PROPERTIES
        [DontCreateProperty]
#endif
        internal T defaultValue = default!;
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

        private T previousValue = default!;

        // This is mainly use for OnValidate weirdness.
        // We need to have another value that is the value right before it gets modified.
        private T temporaryValue = default!;

        /// <summary>
        ///     The current value. This can be changed at runtime.
        /// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
        [CreateProperty]
#endif
        //TODO: Mark this as nullable once Unity fixes properties properly supporting nullable generics.
        public T Value
        {
            get { return value; }
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
#if SCRIPTABLE_VALUES_PROPERTIES_SUPPORTS_READONLY
        [CreateProperty(ReadOnly = true)]
#else
        [CreateProperty]
#endif // SCRIPTABLE_VALUES_PROPERTIES_SUPPORTS_READONLY
#endif // SCRIPTABLE_VALUES_PROPERTIES
        //TODO: Mark this as nullable once Unity fixes properties properly supporting nullable generics.
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
        public event ValueEventHandler<T>? OnValueChanging;
        /// <summary>
        ///     Called after the current value is set.
        /// </summary>
        public event ValueEventHandler<T>? OnValueChanged;

        /// <summary>
        ///     Sets the current value to a new value.
        /// </summary>
        /// <param name="newValue">The new value that should be set.</param>
        /// <param name="notify">If true, the OnValueChanging/Changed events are invoked.</param>
        private void SetValue(T newValue, bool notify)
        {
            // If the game is playing, we don't want to set the value if it's read only.
            ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

            // If the equality check is enabled, we don't want to set the value if it's the same as the current value.
            if (SetEqualityCheck && IsSameValue(newValue, value))
            {
                return;
            }

            if (!OnBeforeSetValue(value, newValue))
            {
                return;
            }

            PreviousValue = temporaryValue;
            // Invoke the OnValueChanging event if notify is true.
            if (notify)
            {
                onValueChanging.Invoke(PreviousValue, newValue);
                OnValueChanging?.Invoke(PreviousValue, newValue);
                NotifyPropertyChanging(valueChangingArgs);
            }

            // Update the value.
            value = newValue;
            temporaryValue = newValue;

            valueIsDefault = EqualityHelper.Equals(value, default);

            OnAfterSetValue(PreviousValue, newValue);

            // Invoke the OnValueChanged event if notify is true.
            if (notify)
            {
                onValueChanged.Invoke(PreviousValue, newValue);
                OnValueChanged?.Invoke(PreviousValue, Value);
                NotifyPropertyChanged(valueChangedArgs);
            }
        }

        /// <summary>
        ///     Called before the value is set. This can be used to prevent the value from being set.
        /// </summary>
        /// <param name="oldValue">The current value.</param>
        /// <param name="newValue">The new value that is going to be set.</param>
        /// <returns><c>true</c> if the value can be set; otherwise, <c>false</c>.</returns>
        /// <remarks>
        ///     This is called before the <see cref="OnValueChanging" /> event. This is also called no matter what `notify` is
        ///     set as.
        /// </remarks>
        protected virtual bool OnBeforeSetValue(T oldValue, T newValue)
        {
            return true;
        }

        /// <summary>
        ///     Called when the value is set. This can be used to do something when the value is set.
        /// </summary>
        /// <param name="oldValue">The old value that was set.</param>
        /// <param name="newValue">The new value that was set.</param>
        /// <remarks>
        ///     This is called before the <see cref="OnValueChanged" /> event. This is also called no matter what `notify` is
        ///     set as.
        /// </remarks>
        protected virtual void OnAfterSetValue(T oldValue, T newValue) { }

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
        ///     Sets the current value without invoking the <see cref="OnValueChanging" /> and <see cref="OnValueChanged" />
        ///     events.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public void SetValueWithoutNotify(T newValue)
        {
            AddStackTrace();

            SetValue(newValue, false);
        }

        /// <inheritdoc />
        protected override void OnPreStart()
        {
            // If the value should be reset, and it isn't a read only value, we set the value to the default value.
            if (ResetValueOnStart && !IsReadOnly)
            {
                ResetValue();
            }

            // Remove any subscribers that are left over from play mode.
            // Don't warn if there are any subscribers left over because we already do that in OnExitPlayMode.
            ClearSubscribers();
        }

        /// <inheritdoc />
        protected override void OnDisabled()
        {
            WarnIfLeftOverSubscribers();
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
            EventHelper.WarnIfLeftOverSubscribers(OnValueChanging, nameof(OnValueChanging), this);
            EventHelper.WarnIfLeftOverSubscribers(OnValueChanged, nameof(OnValueChanged), this);
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

            OnValueChanging = null;
            OnValueChanged = null;
        }

#if UNITY_INCLUDE_TESTS
        /// <summary>
        ///     A test only check to see if <see cref="OnValueChanging" /> has subscribers.
        /// </summary>
        internal bool ValueChangingHasSubscribers
        {
            get { return OnValueChanging != null; }
        }
        /// <summary>
        ///     A test only check to see if <see cref="OnValueChanged" /> has subscribers.
        /// </summary>
        internal bool ValueChangedHasSubscribers
        {
            get { return OnValueChanged != null; }
        }
#endif

#if UNITY_EDITOR
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

        #region Obsolete
#if UNITY_EDITOR
        [Obsolete("Use 'Hertzole.ScriptableValues.ValueEventHandler<T>' instead. This will be removed in build.", true)]
        public delegate void OldNewValue<in TValue>(TValue previousValue, TValue newValue);
#endif
        #endregion
    }
}