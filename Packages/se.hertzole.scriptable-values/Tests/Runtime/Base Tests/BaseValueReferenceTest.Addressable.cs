#if SCRIPTABLE_VALUES_ADDRESSABLES && UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	partial class BaseValueReferenceTest<TType, TValue> where TType : ScriptableValue<TValue>
	{
		private AsyncOperationHandle<ScriptableValue<TValue>> loadHandle;

		protected override void OnSetup()
		{
			if (loadHandle.IsValid() && loadHandle.Result != null)
			{
				loadHandle.Result.ClearSubscribers();
				loadHandle.Result.ResetValue();
			}
			
			if (loadHandle.IsValid())
			{
				Addressables.Release(loadHandle);
			}
		}

		[Test]
		public void Create_Addressable()
		{
			AssetReferenceT<ScriptableValue<TValue>> assetReference = new AssetReferenceT<ScriptableValue<TValue>>(Guid.Empty.ToString());

			ValueReference<TValue> instance = new ValueReference<TValue>(assetReference);

			Assert.AreEqual(default, instance.constantValue);
			Assert.IsNull(instance.referenceValue);
			Assert.AreEqual(ValueReferenceType.Addressable, instance.valueType);
			Assert.AreEqual(instance.addressableReference, assetReference);
		}

		[UnityTest]
		public IEnumerator LoadAndReleaseAddressable()
		{
			AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync("TestObject");
			yield return locationHandle;

			string guid = GetAddressableGuid(locationHandle.Result);
			Addressables.Release(locationHandle);

			ValueReference<TValue> instance = new ValueReference<TValue>(new AssetReferenceT<ScriptableValue<TValue>>(guid));

			loadHandle = instance.LoadAddressableAssetAsync();
			yield return loadHandle;

			Assert.IsNotNull(instance.referenceValue);
			Assert.IsTrue(loadHandle.IsValid());

			instance.ReleaseAddressableAsset();

			Assert.IsNull(instance.referenceValue);
			Assert.IsFalse(loadHandle.IsValid());
		}

		[UnityTest]
		public IEnumerator SetValue_Addressable([ValueSource(nameof(StaticsValue))] TValue value)
		{
			AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync("TestObject");
			yield return locationHandle;

			string guid = GetAddressableGuid(locationHandle.Result);
			Addressables.Release(locationHandle);

			ValueReference<TValue> instance = new ValueReference<TValue>(new AssetReferenceT<ScriptableValue<TValue>>(guid));
			Assert.AreEqual(ValueReferenceType.Addressable, instance.valueType);

			loadHandle = instance.LoadAddressableAssetAsync();
			yield return loadHandle;

			instance.Value = value;

			Assert.AreEqual(default, instance.constantValue);
			Assert.AreEqual(value, instance.Value);
			Assert.AreEqual(value, instance.AssetHandle.Result.Value);

			instance.ReleaseAddressableAsset();
		}
		
		[UnityTest]
		public IEnumerator SetValueWithoutNotify_Addressable([ValueSource(nameof(StaticsValue))] TValue value)
		{
			AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync("TestObject");
			yield return locationHandle;

			string guid = GetAddressableGuid(locationHandle.Result);
			Addressables.Release(locationHandle);

			ValueReference<TValue> instance = new ValueReference<TValue>(new AssetReferenceT<ScriptableValue<TValue>>(guid));
			Assert.AreEqual(ValueReferenceType.Addressable, instance.valueType);
			
			instance.OnValueChanged += (previousValue, newValue) => { NUnit.Framework.Assert.Fail("OnValueChanged should not be invoked."); };
			instance.OnValueChanging += (previousValue, newValue) => { NUnit.Framework.Assert.Fail("OnValueChanging should not be invoked."); };

			loadHandle = instance.LoadAddressableAssetAsync();
			yield return loadHandle;

			instance.SetValueWithoutNotify(value);

			Assert.AreEqual(default, instance.constantValue);
			Assert.AreEqual(value, instance.Value);
			Assert.AreEqual(value, instance.AssetHandle.Result.Value);

			instance.ReleaseAddressableAsset();
		}

		[UnityTest]
		public IEnumerator SetValue_Addressable_NotLoaded([ValueSource(nameof(StaticsValue))] TValue value)
		{
			AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync("TestObject");
			yield return locationHandle;

			string guid = GetAddressableGuid(locationHandle.Result);
			Addressables.Release(locationHandle);

			ValueReference<TValue> instance = new ValueReference<TValue>(new AssetReferenceT<ScriptableValue<TValue>>(guid));
			Assert.AreEqual(ValueReferenceType.Addressable, instance.valueType);

			bool getError = false;

			try
			{
				instance.Value = value;
			}
			catch (InvalidOperationException)
			{
				getError = true;
			}

			Assert.IsTrue(getError);
		}
		
		[UnityTest]
		public IEnumerator OnValueChanging_Addressable_BeforeLoad()
		{
			yield return TestValueChange(true, false);
		}
		
		[UnityTest]
		public IEnumerator OnValueChanging_Addressable_AfterLoad()
		{
			yield return TestValueChange(false, false);
		}
		
		[UnityTest]
		public IEnumerator OnValueChanged_Addressable_BeforeLoad()
		{
			yield return TestValueChange(true, true);
		}
		
		[UnityTest]
		public IEnumerator OnValueChanged_Addressable_AfterLoad()
		{
			yield return TestValueChange(false, true);
		}

		private IEnumerator TestValueChange(bool beforeLoad, bool changed)
		{
			bool eventInvoked = false;
			
			AsyncOperationHandle<IList<IResourceLocation>> locationHandle = Addressables.LoadResourceLocationsAsync("TestObject");
			yield return locationHandle;

			string guid = GetAddressableGuid(locationHandle.Result);
			Addressables.Release(locationHandle);
			
			ValueReference<TValue> instance = new ValueReference<TValue>(new AssetReferenceT<ScriptableValue<TValue>>(guid));
			TValue oldValue = default;
			TValue valueToSet = default;

			if (beforeLoad)
			{
				if (changed)
				{
					instance.OnValueChanged += ValueChange;
				}
				else
				{
					instance.OnValueChanging += ValueChange;
				}
			}

			loadHandle = instance.LoadAddressableAssetAsync();
			yield return loadHandle;
			
			if (!beforeLoad)
			{
				if (changed)
				{
					instance.OnValueChanged += ValueChange;
				}
				else
				{
					instance.OnValueChanging += ValueChange;
				}
			}

			valueToSet = MakeDifferentValue(instance.Value);
			
			
			oldValue = instance.Value;
			
			instance.Value = valueToSet;

			Assert.AreEqual(valueToSet, instance.referenceValue.Value);
			Assert.AreEqual(valueToSet, instance.Value);
			Assert.IsTrue(eventInvoked, "Event was not invoked");

			eventInvoked = false;

			if (changed)
			{
				instance.OnValueChanged -= ValueChange;
			}
			else
			{
				instance.OnValueChanging -= ValueChange;
			}

			valueToSet = MakeDifferentValue(instance.Value);
			
			instance.Value = valueToSet;
			
			Assert.AreEqual(valueToSet, instance.referenceValue.Value);
			Assert.AreEqual(valueToSet, instance.Value);
			Assert.IsFalse(eventInvoked);
			
			instance.referenceValue.ResetValue();
			
			instance.ReleaseAddressableAsset();

			yield return null;

			if (loadHandle.IsValid())
			{
				Addressables.Release(loadHandle);
			}
			
			void ValueChange(TValue previousValue, TValue newValue)
			{
				Assert.AreEqual(oldValue, previousValue, $"Expected previous value to be {oldValue} but it was {previousValue}");
				Assert.AreEqual(valueToSet, newValue);
				eventInvoked = true;
			}
		}

		private static string GetAddressableGuid(IEnumerable<IResourceLocation> locations)
		{
			foreach (IResourceLocation location in locations)
			{
				if (location.ResourceType == typeof(TType))
				{
					return AssetDatabase.GUIDFromAssetPath(location.InternalId).ToString();
				}
			}

			return null;
		}
	}
}
#endif