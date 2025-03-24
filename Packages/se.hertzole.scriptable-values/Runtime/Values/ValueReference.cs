#nullable enable

using System;
using Hertzole.ScriptableValues.Helpers;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
#if SCRIPTABLE_VALUES_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

namespace Hertzole.ScriptableValues
{
	public enum ValueReferenceType
	{
		Constant = 0,
		Reference = 1,
#if SCRIPTABLE_VALUES_ADDRESSABLES
		Addressable = 2
#endif
	}

	/// <summary>
	///     Allows you to reference a ScriptableValue or a constant value.
	/// </summary>
	/// <typeparam name="T">The type that the value should be.</typeparam>
	[Serializable]
#if ODIN_INSPECTOR
	[Sirenix.OdinInspector.DrawWithUnity]
#endif
	public class ValueReference<T>
	{
		[SerializeField]
		internal T constantValue = default!;
		[SerializeField]
		internal ScriptableValue<T> referenceValue = null!;
#if SCRIPTABLE_VALUES_ADDRESSABLES
		[SerializeField]
		internal AssetReferenceT<ScriptableValue<T>> addressableReference = null!;
#endif
		[SerializeField]
		internal ValueReferenceType valueType;

#if UNITY_EDITOR
		[NonSerialized]
		internal T oldValue = default!;
#endif

		public T Value
		{
			get { return GetValue(); }
			set { SetValue(value, true); }
		}

		// These are only used for constant values, and as a buffer for addressable assets that are not loaded yet.
		internal readonly DelegateHandlerList<ScriptableValue<T>.OldNewValue<T>, T, T> onValueChangingInternal =
			new DelegateHandlerList<ScriptableValue<T>.OldNewValue<T>, T, T>();
		internal readonly DelegateHandlerList<ScriptableValue<T>.OldNewValue<T>, T, T> onValueChangedInternal =
			new DelegateHandlerList<ScriptableValue<T>.OldNewValue<T>, T, T>();

		public event ScriptableValue<T>.OldNewValue<T> OnValueChanging
		{
			add { RegisterValueChanging(value); }
			remove { UnregisterValueChanging(value); }
		}

		public event ScriptableValue<T>.OldNewValue<T> OnValueChanged
		{
			add { RegisterValueChanged(value); }
			remove { UnregisterValueChanged(value); }
		}

		public ValueReference()
		{
			valueType = ValueReferenceType.Reference;
			constantValue = default;
			referenceValue = null;
#if SCRIPTABLE_VALUES_ADDRESSABLES
			addressableReference = null;
#endif
		}

		public ValueReference(T constantValue)
		{
			valueType = ValueReferenceType.Constant;
			this.constantValue = constantValue;
		}

		public ValueReference(ScriptableValue<T> referenceValue)
		{
			valueType = ValueReferenceType.Reference;
			this.referenceValue = referenceValue;
		}

#if SCRIPTABLE_VALUES_ADDRESSABLES
		public ValueReference(AssetReferenceT<ScriptableValue<T>> addressableReference)
		{
			valueType = ValueReferenceType.Addressable;
			this.addressableReference = addressableReference;
		}
#endif

		private T GetValue()
		{
			switch (valueType)
			{
				case ValueReferenceType.Constant:
					return constantValue;
				case ValueReferenceType.Reference:
					return referenceValue.Value;
#if SCRIPTABLE_VALUES_ADDRESSABLES
				case ValueReferenceType.Addressable:
					if (AssetHandle.IsValid() && AssetHandle.IsDone && AssetHandle.Result != null)
					{
						return AssetHandle.Result.Value;
					}

					throw new InvalidOperationException(
						$"Addressable asset is not loaded yet. Make sure you've called {nameof(LoadAddressableAssetAsync)} before trying to access the value.");
#endif
				default:
					throw new NotSupportedException($"No supported value type for {valueType}.");
			}
		}

		private void SetValue(in T value, in bool notify)
		{
			T previousValue = Value;
			if (EqualityHelper.Equals(previousValue, value))
			{
				return;
			}

			switch (valueType)
			{
				case ValueReferenceType.Constant:
					SetValueConstant(value, previousValue, notify);
					break;
				case ValueReferenceType.Reference:
					SetValueReference(value, notify);
					break;
#if SCRIPTABLE_VALUES_ADDRESSABLES
				case ValueReferenceType.Addressable:
					SetValueAddressable(value, notify);
					break;
#endif
				default:
					throw new NotSupportedException($"No supported value type for {valueType}.");
			}
		}

		private void SetValueConstant(in T value, in T previousValue, in bool notify)
		{
			if (notify)
			{
				onValueChangingInternal.Invoke(previousValue, value);
			}

			constantValue = value;

			if (notify)
			{
				onValueChangedInternal.Invoke(previousValue, value);
			}
		}

		private void SetValueReference(in T value, in bool notify)
		{
			if (notify)
			{
				referenceValue.Value = value;
			}
			else
			{
				referenceValue.SetValueWithoutNotify(value);
			}
		}

#if SCRIPTABLE_VALUES_ADDRESSABLES
		private void SetValueAddressable(in T value, in bool notify)
		{
			if (AssetHandle.IsValid() && AssetHandle.IsDone && AssetHandle.Result != null)
			{
				if (notify)
				{
					AssetHandle.Result.Value = value;
				}
				else
				{
					AssetHandle.Result.SetValueWithoutNotify(value);
				}
			}
		}
#endif

		/// <inheritdoc cref="ScriptableValue{T}.SetValueWithoutNotify(T)" />
		public void SetValueWithoutNotify(T value)
		{
			SetValue(value, false);
		}

		/// <inheritdoc cref="ScriptableValue{T}.RegisterValueChangingListener" />
		public void RegisterValueChanging(ScriptableValue<T>.OldNewValue<T> callback)
		{
			RegisterEvent<ScriptableValue<T>.OldNewValue<T>, object>(static (value, c, _) => value.RegisterValueChangingListener(c), callback, null,
				onValueChangingInternal);
		}

		/// <inheritdoc cref="ScriptableValue{T}.RegisterValueChangingListener{TArgs}" />
		public void RegisterValueChanging<TArgs>(Action<T, T, TArgs> callback, TArgs args)
		{
			RegisterEvent(static (value, c, a) => value.RegisterValueChangingListener(c!, a), callback, args, onValueChangingInternal);
		}

		/// <inheritdoc cref="ScriptableValue{T}.UnregisterValueChangingListener" />
		public void UnregisterValueChanging(ScriptableValue<T>.OldNewValue<T> callback)
		{
			UnregisterEvent(static (value, c) => value.UnregisterValueChangingListener(c), callback, onValueChangingInternal);
		}

		/// <inheritdoc cref="ScriptableValue{T}.UnregisterValueChangingListener{TArgs}" />
		public void UnregisterValueChanging<TArgs>(Action<T, T, TArgs> callback)
		{
			UnregisterValueChanging(UnsafeUtility.As<Action<T, T, TArgs>, ScriptableValue<T>.OldNewValue<T>>(ref callback));
		}

		/// <inheritdoc cref="ScriptableValue{T}.RegisterValueChangedListener" />
		public void RegisterValueChanged(ScriptableValue<T>.OldNewValue<T> callback)
		{
			RegisterEvent<ScriptableValue<T>.OldNewValue<T>, object>(static (value, c, _) => value.RegisterValueChangedListener(c), callback, null,
				onValueChangedInternal);
		}

		/// <inheritdoc cref="ScriptableValue{T}.RegisterValueChangedListener{TArgs}" />
		public void RegisterValueChanged<TArgs>(Action<T, T, TArgs> callback, TArgs args)
		{
			RegisterEvent(static (value, c, a) => value.RegisterValueChangedListener(c!, a), callback, args, onValueChangedInternal);
		}

		/// <inheritdoc cref="ScriptableValue{T}.UnregisterValueChangedListener" />
		public void UnregisterValueChanged(ScriptableValue<T>.OldNewValue<T> callback)
		{
			UnregisterEvent(static (value, c) => value.UnregisterValueChangedListener(c), callback, onValueChangedInternal);
		}

		/// <inheritdoc cref="ScriptableValue{T}.UnregisterValueChangedListener{TArgs}" />
		public void UnregisterValueChanged<TArgs>(Action<T, T, TArgs> callback)
		{
			UnregisterValueChanged(UnsafeUtility.As<Action<T, T, TArgs>, ScriptableValue<T>.OldNewValue<T>>(ref callback));
		}

		private void RegisterEvent<TEvent, TArgs>(Action<ScriptableValue<T>, TEvent, TArgs?> registerAction,
			TEvent callback,
			TArgs? args,
			IDelegateList<ScriptableValue<T>.OldNewValue<T>> defaultListener) where TEvent : Delegate
		{
			if (valueType == ValueReferenceType.Reference)
			{
				registerAction.Invoke(referenceValue, callback, args);
			}
#if SCRIPTABLE_VALUES_ADDRESSABLES
			else if (IsValidAddressable())
			{
				registerAction.Invoke(AssetHandle.Result, callback, args);
			}
#endif

			defaultListener.RegisterCallback(callback, args);
		}

		private void UnregisterEvent<TEvent>(Action<ScriptableValue<T>, TEvent> unregisterAction,
			TEvent callback,
			IDelegateList<ScriptableValue<T>.OldNewValue<T>> defaultListener)
			where TEvent : Delegate
		{
			if (valueType == ValueReferenceType.Reference)
			{
				unregisterAction.Invoke(referenceValue, callback);
			}
#if SCRIPTABLE_VALUES_ADDRESSABLES
			else if (IsValidAddressable())
			{
				unregisterAction.Invoke(AssetHandle.Result, callback);
			}
#endif

			defaultListener.RemoveCallback(callback);
		}

#if SCRIPTABLE_VALUES_ADDRESSABLES
		private bool IsValidAddressable()
		{
			return valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.IsDone && AssetHandle.Result != null;
		}
#endif

#if UNITY_EDITOR
		internal void SetPreviousValue()
		{
			oldValue = constantValue;
		}

		internal void SetEditorValue()
		{
			if (!Application.isPlaying || EqualityHelper.Equals(oldValue, Value))
			{
				return;
			}

			onValueChangingInternal.Invoke(oldValue, Value);
			onValueChangedInternal.Invoke(oldValue, Value);

			oldValue = Value;
		}
#endif

#if SCRIPTABLE_VALUES_ADDRESSABLES
		public AsyncOperationHandle<ScriptableValue<T>> AssetHandle { get; private set; }

		public AsyncOperationHandle<ScriptableValue<T>> LoadAddressableAssetAsync(Action<AsyncOperationHandle<ScriptableValue<T>>> onLoaded = null)
		{
			AssetHandle = Addressables.LoadAssetAsync<ScriptableValue<T>>(addressableReference);

			AssetHandle.Completed += OnAssetHandleOnCompleted;

			return AssetHandle;

			void OnAssetHandleOnCompleted(AsyncOperationHandle<ScriptableValue<T>> handle)
			{
				if (handle.Status == AsyncOperationStatus.Succeeded)
				{
					referenceValue = handle.Result;
					referenceValue.onValueChangingEvents.AddFrom(onValueChangingInternal);
					referenceValue.onValueChangedEvents.AddFrom(onValueChangedInternal);
				}

				onLoaded?.Invoke(handle);
				// Clean up the event as to not have any memory leaks.
				handle.Completed -= OnAssetHandleOnCompleted;
			}
		}

		public void ReleaseAddressableAsset()
		{
			// Make sure to remove all old listeners.
			if (referenceValue != null)
			{
				referenceValue.onValueChangingEvents.RemoveFrom(onValueChangingInternal);
				referenceValue.onValueChangedEvents.RemoveFrom(onValueChangedInternal);
			}

			if (AssetHandle.IsValid())
			{
				Addressables.Release(AssetHandle);
				referenceValue = null;
			}
		}
#endif
	}
}