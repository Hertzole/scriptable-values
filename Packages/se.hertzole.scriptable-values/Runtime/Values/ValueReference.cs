using System;
using Hertzole.ScriptableValues.Helpers;
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

		internal event ScriptableValue<T>.OldNewValue<T> OnValueChangingInternal;
		internal event ScriptableValue<T>.OldNewValue<T> OnValueChangedInternal;

		public event ScriptableValue<T>.OldNewValue<T> OnValueChanging
		{
			add
			{
				if (valueType == ValueReferenceType.Reference)
				{
					referenceValue.OnValueChanging += value;
				}
#if SCRIPTABLE_VALUES_ADDRESSABLES
				else if (valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.Result != null)
				{
					AssetHandle.Result.OnValueChanging += value;
				}
#endif
				else
				{
					OnValueChangingInternal += value;
				}
			}
			remove
			{
				if (valueType == ValueReferenceType.Reference)
				{
					referenceValue.OnValueChanging -= value;
				}
#if SCRIPTABLE_VALUES_ADDRESSABLES
				else if (valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.Result != null)
				{
					AssetHandle.Result.OnValueChanging -= value;
				}
#endif
				else
				{
					OnValueChangingInternal -= value;
				}
			}
		}

		public event ScriptableValue<T>.OldNewValue<T> OnValueChanged
		{
			add
			{
				if (valueType == ValueReferenceType.Reference)
				{
					referenceValue.OnValueChanged += value;
				}
#if SCRIPTABLE_VALUES_ADDRESSABLES
				else if (valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.Result != null)
				{
					AssetHandle.Result.OnValueChanged += value;
				}
#endif
				else
				{
					OnValueChangedInternal += value;
				}
			}
			remove
			{
				if (valueType == ValueReferenceType.Reference)
				{
					referenceValue.OnValueChanged -= value;
				}
#if SCRIPTABLE_VALUES_ADDRESSABLES
				else if (valueType == ValueReferenceType.Addressable && AssetHandle.IsValid() && AssetHandle.Result != null)
				{
					AssetHandle.Result.OnValueChanged -= value;
				}
#endif
				else
				{
					OnValueChangedInternal -= value;
				}
			}
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
				OnValueChangingInternal?.Invoke(previousValue, value);
			}

			constantValue = value;

			if (notify)
			{
				OnValueChangedInternal?.Invoke(previousValue, value);
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

		public void SetValueWithoutNotify(T value)
		{
			SetValue(value, false);
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

			OnValueChangingInternal?.Invoke(oldValue, Value);
			OnValueChangedInternal?.Invoke(oldValue, Value);

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
					if (OnValueChangingInternal != null)
					{
						var invocations = OnValueChangingInternal.GetInvocationList();
						for (int i = 0; i < invocations.Length; i++)
						{
							referenceValue.OnValueChanging += (ScriptableValue<T>.OldNewValue<T>)invocations[i];
						}
					}
					

					if (OnValueChangedInternal != null)
					{
						var invocations = OnValueChangedInternal.GetInvocationList();
						for (int i = 0; i < invocations.Length; i++)
						{
							referenceValue.OnValueChanged += (ScriptableValue<T>.OldNewValue<T>)invocations[i];
						}
					}
				}

				onLoaded?.Invoke(handle);
			};

			return AssetHandle;
		}

		public void ReleaseAddressableAsset()
		{
			if (AssetHandle.IsValid())
			{
				Addressables.Release(AssetHandle);
				referenceValue = null;
			}
		}
		
		private void OnAddressableResultValueChanging(T previousValue, T newValue)
		{
			OnValueChangingInternal?.Invoke(previousValue, newValue);
		}
		
		private void OnAddressableResultValueChanged(T previousValue, T newValue)
		{
			OnValueChangedInternal?.Invoke(previousValue, newValue);
		}
#endif
	}
}