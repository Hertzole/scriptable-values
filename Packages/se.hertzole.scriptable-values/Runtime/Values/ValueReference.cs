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
		internal T constantValue = default;
		[SerializeField]
		internal ScriptableValue<T> referenceValue = default;
#if SCRIPTABLE_VALUES_ADDRESSABLES
		[SerializeField]
		internal AssetReferenceT<ScriptableValue<T>> addressableReference = default;
#endif
		[SerializeField]
		internal ValueReferenceType valueType;

#if UNITY_EDITOR
		[NonSerialized]
		internal T oldValue;
#endif

		public T Value
		{
			get { return GetValue(); }
			set { SetValue(value, true); }
		}

		// These are only used for constant values, and as a buffer for addressable assets that are not loaded yet.
		internal readonly ValueEventList<T> onValueChangingInternal = new ValueEventList<T>();
		internal readonly ValueEventList<T> onValueChangedInternal = new ValueEventList<T>();

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

		/// <inheritdoc cref="ScriptableValue{T}.RegisterValueChanging(ScriptableValue{T}.OldNewValue{T})" />
		public void RegisterValueChanging(ScriptableValue<T>.OldNewValue<T> callback)
		{
			if (valueType == ValueReferenceType.Reference)
			{
				referenceValue.RegisterValueChanging(callback);
			}
#if SCRIPTABLE_VALUES_ADDRESSABLES
			else if (valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.Result != null)
			{
				AssetHandle.Result.RegisterValueChanging(callback);
			}
#endif

			onValueChangingInternal.AddListener(callback);
		}

		/// <inheritdoc cref="ScriptableValue{T}.RegisterValueChanging{TArgs}(Action{T, T, TArgs}, TArgs)" />
		public void RegisterValueChanging<TArgs>(Action<T, T, TArgs> callback, TArgs args)
		{
			if (valueType == ValueReferenceType.Reference)
			{
				referenceValue.RegisterValueChanging(callback, args);
			}
#if SCRIPTABLE_VALUES_ADDRESSABLES
			else if (valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.Result != null)
			{
				AssetHandle.Result.RegisterValueChanging(callback, args);
			}
#endif

			onValueChangingInternal.AddListener(callback, args);
		}

		/// <inheritdoc cref="ScriptableValue{T}.UnregisterValueChanging(ScriptableValue{T}.OldNewValue{T})" />
		public void UnregisterValueChanging(ScriptableValue<T>.OldNewValue<T> callback)
		{
			if (valueType == ValueReferenceType.Reference)
			{
				referenceValue.UnregisterValueChanging(callback);
			}
#if SCRIPTABLE_VALUES_ADDRESSABLES
			else if (valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.Result != null)
			{
				AssetHandle.Result.UnregisterValueChanging(callback);
			}
#endif

			onValueChangingInternal.RemoveListener(callback);
		}

		/// <inheritdoc cref="ScriptableValue{T}.UnregisterValueChanging{TArgs}(Action{T, T, TArgs})" />
		public void UnregisterValueChanging<TArgs>(Action<T, T, TArgs> callback)
		{
			UnregisterValueChanging(UnsafeUtility.As<Action<T, T, TArgs>, ScriptableValue<T>.OldNewValue<T>>(ref callback));
		}

		/// <inheritdoc cref="ScriptableValue{T}.RegisterValueChanged(ScriptableValue{T}.OldNewValue{T})" />
		public void RegisterValueChanged(ScriptableValue<T>.OldNewValue<T> callback)
		{
			if (valueType == ValueReferenceType.Reference)
			{
				referenceValue.RegisterValueChanged(callback);
			}
#if SCRIPTABLE_VALUES_ADDRESSABLES
			else if (valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.Result != null)
			{
				AssetHandle.Result.RegisterValueChanged(callback);
			}
#endif

			onValueChangedInternal.AddListener(callback);
		}

		/// <inheritdoc cref="ScriptableValue{T}.RegisterValueChanged{TArgs}(Action{T, T, TArgs}, TArgs)" />
		public void RegisterValueChanged<TArgs>(Action<T, T, TArgs> callback, TArgs args)
		{
			if (valueType == ValueReferenceType.Reference)
			{
				referenceValue.RegisterValueChanged(callback, args);
			}
#if SCRIPTABLE_VALUES_ADDRESSABLES
			else if (valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.Result != null)
			{
				AssetHandle.Result.RegisterValueChanged(callback, args);
			}
#endif

			onValueChangedInternal.AddListener(callback, args);
		}

		/// <inheritdoc cref="ScriptableValue{T}.UnregisterValueChanged(ScriptableValue{T}.OldNewValue{T})" />
		public void UnregisterValueChanged(ScriptableValue<T>.OldNewValue<T> callback)
		{
			if (valueType == ValueReferenceType.Reference)
			{
				referenceValue.UnregisterValueChanged(callback);
			}
#if SCRIPTABLE_VALUES_ADDRESSABLES
			else if (valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.Result != null)
			{
				AssetHandle.Result.UnregisterValueChanged(callback);
			}
#endif

			onValueChangedInternal.RemoveListener(callback);
		}

		/// <inheritdoc cref="ScriptableValue{T}.UnregisterValueChanged{TArgs}(Action{T, T, TArgs})" />
		public void UnregisterValueChanged<TArgs>(Action<T, T, TArgs> callback)
		{
			UnregisterValueChanged(UnsafeUtility.As<Action<T, T, TArgs>, ScriptableValue<T>.OldNewValue<T>>(ref callback));
		}

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

			AssetHandle.Completed += handle =>
			{
				if (handle.Status == AsyncOperationStatus.Succeeded)
				{
					referenceValue = handle.Result;
					onValueChangingInternal.AddFrom(referenceValue.onValueChangingEvents);
					onValueChangedInternal.AddFrom(referenceValue.onValueChangedEvents);
				}

				onLoaded?.Invoke(handle);
			};

			return AssetHandle;
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