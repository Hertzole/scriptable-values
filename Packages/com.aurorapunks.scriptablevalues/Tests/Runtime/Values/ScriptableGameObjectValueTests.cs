using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableGameObjectValueTests : BaseScriptableValueTest<ScriptableGameObject, GameObject>
	{
		[Test]
		public void SetValue_FromNull()
		{
			GameObject go = CreateGameObject();

			TestSetValue(go);
		}

		[Test]
		public void SetValue_ToNull()
		{
			GameObject go = CreateGameObject();

			TestSetValue(null, go);
		}

		[Test]
		public void SetValue_WithoutNotify_FromNull()
		{
			GameObject go = CreateGameObject();

			TestSetValue_WithoutNotify(go);
		}

		[Test]
		public void SetValue_WithoutNotify_ToNull()
		{
			GameObject go = CreateGameObject();

			TestSetValue_WithoutNotify(null, go);
		}

		[Test]
		public void SetValue_OnValidate([ValueSource(nameof(bools))] bool equalsCheck)
		{
			GameObject go = CreateGameObject();

			TestSetValue_OnValidate(equalsCheck, go);
		}

		[Test]
		public void SetValue_ReadOnly()
		{
			GameObject go = CreateGameObject();

			TestSetValue_ReadOnly(go);
		}

		[Test]
		public void SetValue_SameValue()
		{
			GameObject go = CreateGameObject();

			TestSetValue_SameValue(go, go);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck()
		{
			GameObject go = CreateGameObject();

			TestSetValue_SameValue_NoEqualsCheck(go, go);
		}

		[UnityTest]
		public IEnumerator SetValue_WithoutNotify_Destroyed()
		{
			ScriptableGameObject instance = CreateInstance<ScriptableGameObject>();
			instance.SetEqualityCheck = true;
			instance.ResetValue();

			GameObject go = CreateGameObject();
			GameObject expectedValue = go;

			GameObject originalValue = instance.Value;

			bool valueChangingInvoked = false;
			bool valueChangedInvoked = false;

			instance.OnValueChanging += OnValueChanging;
			instance.OnValueChanged += OnValueChanged;

			instance.Value = go;

			Assert.AreEqual(go, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");

			valueChangingInvoked = false;
			valueChangedInvoked = false;

			Destroy(go);
			expectedValue = null;

			yield return null;

			Assert.IsNull(go);

			instance.Value = null;

			Assert.AreEqual(expectedValue, instance.Value, "Value should be the value being set.");
			Assert.IsTrue(valueChangingInvoked, "OnValueChanging should be invoked.");
			Assert.IsTrue(valueChangedInvoked, "OnValueChanged should be invoked.");

			void OnValueChanging(GameObject oldValue, GameObject newValue)
			{
				Assert.AreEqual(originalValue, oldValue, $"Old value should be the original value ({originalValue}) but was {oldValue}.");
				Assert.AreEqual(expectedValue, newValue, "New value should be the value being set.");
				valueChangingInvoked = true;
			}

			void OnValueChanged(GameObject oldValue, GameObject newValue)
			{
				Assert.AreEqual(originalValue, oldValue, $"Old value should be the original value ({originalValue}) but was {oldValue}.");
				Assert.AreEqual(expectedValue, newValue, "New value should be the value being set.");
				valueChangedInvoked = true;
			}
		}
	}
}