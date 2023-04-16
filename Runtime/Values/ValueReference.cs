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
		private T constantValue = default;
		[SerializeField]
		private ScriptableValue<T> referenceValue = default;
#if SCRIPTABLE_VALUES_ADDRESSABLES
		[SerializeField]
		private AssetReferenceT<ScriptableValue<T>> addressableReference = default;
#endif
		[SerializeField]
		private ValueReferenceType valueType = ValueReferenceType.Reference;

		public T Value
		{
			get
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

						throw new InvalidOperationException($"Addressable asset is not loaded yet. Make sure you've called {nameof(LoadAddressableAssetAsync)} before trying to access the value.");
#endif
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set
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
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public event ScriptableValue<T>.OldNewValue<T> OnValueChangingInternal;
		public event ScriptableValue<T>.OldNewValue<T> OnValueChangedInternal;

		public event ScriptableValue<T>.OldNewValue<T> OnValueChanging
		{
			add
			{
				if (valueType == ValueReferenceType.Reference)
				{
					referenceValue.OnValueChanging += value;
				}
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
				else
				{
					OnValueChangedInternal -= value;
				}
			}
		}

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
			}
		}
#endif
	}
}