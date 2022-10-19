using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public class ScriptableValueTests
	{
		[Test]
		public void ScriptableBool_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableBool, bool>(true, true);
		}

		[Test]
		public void ScriptableByte_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableByte, byte>(1, true);
		}

		[Test]
		public void ScriptableSByte_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableSByte, sbyte>(1, true);
		}

		[Test]
		public void ScriptableShort_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableShort, short>(1, true);
		}

		[Test]
		public void ScriptableUShort_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableUShort, ushort>(1, true);
		}

		[Test]
		public void ScriptableInt_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableInt, int>(1, true);
		}

		[Test]
		public void ScriptableUInt_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableUInt, uint>(1, true);
		}

		[Test]
		public void ScriptableLong_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableLong, long>(1, true);
		}

		[Test]
		public void ScriptableULong_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableULong, ulong>(1, true);
		}

		[Test]
		public void ScriptableFloat_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableFloat, float>(12.2f, true);
		}

		[Test]
		public void ScriptableDouble_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableDouble, double>(12.2, true);
		}

		[Test]
		public void ScriptableDecimal_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableDecimal, decimal>(12.2m, true);
		}

		[Test]
		public void ScriptableString_SetValue_UpdatesValue()
		{
			TestSetValue<ScriptableString, string>("First Value", true);
		}
		
		
		[Test]
		public void ScriptableBool_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableBool, bool>(true, false);
		}

		[Test]
		public void ScriptableByte_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableByte, byte>(1, false);
		}

		[Test]
		public void ScriptableSByte_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableSByte, sbyte>(1, false);
		}

		[Test]
		public void ScriptableShort_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableShort, short>(1, false);
		}

		[Test]
		public void ScriptableUShort_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableUShort, ushort>(1, false);
		}

		[Test]
		public void ScriptableInt_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableInt, int>(1, false);
		}

		[Test]
		public void ScriptableUInt_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableUInt, uint>(1, false);
		}

		[Test]
		public void ScriptableLong_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableLong, long>(1, false);
		}

		[Test]
		public void ScriptableULong_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableULong, ulong>(1, false);
		}

		[Test]
		public void ScriptableFloat_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableFloat, float>(12.2f, false);
		}

		[Test]
		public void ScriptableDouble_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableDouble, double>(12.2, false);
		}

		[Test]
		public void ScriptableDecimal_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableDecimal, decimal>(12.2m, false);
		}

		[Test]
		public void ScriptableString_SetValueWithoutNotify_UpdatesValue()
		{
			TestSetValue<ScriptableString, string>("First Value", false);
		}

		[UnityTest]
		public IEnumerator ScriptableBool_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableBool, bool>(true);
		}

		[UnityTest]
		public IEnumerator ScriptableByte_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableByte, byte>(1);
		}

		[UnityTest]
		public IEnumerator ScriptableSByte_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableSByte, sbyte>(1);
		}

		[UnityTest]
		public IEnumerator ScriptableShort_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableShort, short>(1);
		}

		[UnityTest]
		public IEnumerator ScriptableUShort_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableUShort, ushort>(1);
		}

		[UnityTest]
		public IEnumerator ScriptableInt_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableInt, int>(1);
		}

		[UnityTest]
		public IEnumerator ScriptableUInt_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableUInt, uint>(1);
		}

		[UnityTest]
		public IEnumerator ScriptableLong_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableLong, long>(1);
		}

		[UnityTest]
		public IEnumerator ScriptableULong_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableULong, ulong>(1);
		}

		[UnityTest]
		public IEnumerator ScriptableFloat_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableFloat, float>(12.2f);
		}

		[UnityTest]
		public IEnumerator ScriptableDouble_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableDouble, double>(12.2);
		}

		[UnityTest]
		public IEnumerator ScriptableDecimal_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableDecimal, decimal>(12.2m);
		}

		[UnityTest]
		public IEnumerator ScriptableString_SetValue_IsReadOnly()
		{
			yield return new SetReadOnlyValueTest<ScriptableString, string>("First Value");
		}

		[Test]
		public void SetValue_OnValidate()
		{
			ScriptableInt instance = ScriptableObject.CreateInstance<ScriptableInt>();

			bool changed = false;

			instance.OnValueChanged += (value, newValue) =>
			{
				changed = true;
			};
			
			instance.value = 1;
			instance.SetValueOnValidateInternal();

			Assert.AreEqual(1, instance.Value);
			Assert.IsTrue(changed);

			Object.DestroyImmediate(instance);
		}
		
		private static void TestSetValue<TType, TValue>(TValue value, bool notify) where TType : ScriptableValue<TValue>
		{
			TType scriptableValue = ScriptableObject.CreateInstance<TType>();

			Assert.AreNotEqual(value, scriptableValue.Value, "The value should not be equal to the default value");

			bool changing = false;
			bool changed = false;

			scriptableValue.OnValueChanging += (previousValue, newValue) =>
			{
				changing = true;
			};
			
			scriptableValue.OnValueChanged += (previousValue, newValue) =>
			{
				changed = true;
			};
			
			if (notify)
			{
				scriptableValue.Value = value;

				Assert.IsTrue(changing);
				Assert.IsTrue(changed);
			}
			else
			{
				scriptableValue.SetValueWithoutNotify(value);

				Assert.IsFalse(changing);
				Assert.IsFalse(changed);
			}

			Assert.AreEqual(value, scriptableValue.Value);

			Object.DestroyImmediate(scriptableValue);
		}

		private class SetReadOnlyValueTest<TType, TValue> : IEditModeTestYieldInstruction where TType : ScriptableValue<TValue>
		{
			private readonly TValue value;

			public bool ExpectDomainReload { get { return false; } }
			public bool ExpectedPlaymodeState { get; private set; }

			public SetReadOnlyValueTest(TValue value)
			{
				this.value = value;
			}

			public IEnumerator Perform()
			{
				TType scriptableValue = ScriptableObject.CreateInstance<TType>();
				scriptableValue.name = "Test Scriptable Value";
				scriptableValue.hideFlags = HideFlags.DontSave;
				scriptableValue.IsReadOnly = true;

				try
				{
					Assert.AreNotEqual(value, scriptableValue.Value, "The value should not be equal to the default value");
				}
				catch (AssertionException e)
				{
					Debug.LogAssertion(e);
					EditorApplication.isPlaying = false;
					Object.DestroyImmediate(scriptableValue);
					yield break;
				}

				TValue originalValue = scriptableValue.Value;

				// Enter play mode
				ExpectedPlaymodeState = true;
				EditorApplication.isPlaying = true;
				while (!EditorApplication.isPlaying)
				{
					yield return null;
				}

				try
				{
					Assert.IsTrue(Application.isPlaying);
				}
				catch (AssertionException e)
				{
					Debug.LogAssertion(e);
					EditorApplication.isPlaying = false;
					Object.DestroyImmediate(scriptableValue);
					yield break;
				}
				
				LogAssert.Expect(LogType.Error, $"'{scriptableValue.name}' is marked as read only and cannot be changed at runtime.");

				scriptableValue.Value = value;
				try
				{
					Assert.AreEqual(originalValue, scriptableValue.Value);
				}
				catch (AssertionException e)
				{
					Debug.LogAssertion(e);
					EditorApplication.isPlaying = false;
					Object.DestroyImmediate(scriptableValue);
					yield break;
				}

				// Exit play mode
				ExpectedPlaymodeState = false;
				EditorApplication.isPlaying = false;
				while (EditorApplication.isPlaying)
				{
					yield return null;
				}

				Object.DestroyImmediate(scriptableValue);
			}
		}
	}
}