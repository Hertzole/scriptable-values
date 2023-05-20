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
			set { SetValue(value); }
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
					throw new ArgumentOutOfRangeException(nameof(valueType), $"No supported value type for {valueType}.");
			}
		}

		private void SetValue(T value)
		{
			T previousValue = Value;
			if (EqualityHelper.Equals(previousValue, value))
			{
				return;
			}

			switch (valueType)
			{
				case ValueReferenceType.Constant:
					OnValueChangingInternal?.Invoke(previousValue, value);
					constantValue = value;
					OnValueChangedInternal?.Invoke(previousValue, value);
					break;
				case ValueReferenceType.Reference:
					referenceValue.Value = value;
					break;
#if SCRIPTABLE_VALUES_ADDRESSABLES
				case ValueReferenceType.Addressable:
					if (AssetHandle.IsValid() && AssetHandle.IsDone && AssetHandle.Result != null)
					{
						AssetHandle.Result.Value = value;
					}

					break;
#endif
			}
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
					referenceValue.OnValueChanging += OnValueChangingInternal;
					referenceValue.OnValueChanged += OnValueChangedInternal;
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
#endif
	}
}