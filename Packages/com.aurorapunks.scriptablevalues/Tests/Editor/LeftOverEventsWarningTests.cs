using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public class LeftOverEventsWarningTests
	{
		private static readonly Regex leftOverWarningRegex = new Regex(
			@"(On)([A-za-z]*) in object [A-Za-z ]*\(.*\) has some left over subscribers:.*",
			RegexOptions.Compiled);

		[UnityTest]
		public IEnumerator ScriptableValue_Bool()
		{
			yield return new LeftOverWarningTest<ScriptableBool>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_Byte()
		{
			yield return new LeftOverWarningTest<ScriptableByte>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_SByte()
		{
			yield return new LeftOverWarningTest<ScriptableSByte>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_Short()
		{
			yield return new LeftOverWarningTest<ScriptableShort>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_UShort()
		{
			yield return new LeftOverWarningTest<ScriptableUShort>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_Int()
		{
			yield return new LeftOverWarningTest<ScriptableInt>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_UInt()
		{
			yield return new LeftOverWarningTest<ScriptableUInt>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_Long()
		{
			yield return new LeftOverWarningTest<ScriptableLong>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_ULong()
		{
			yield return new LeftOverWarningTest<ScriptableULong>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_Float()
		{
			yield return new LeftOverWarningTest<ScriptableFloat>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_Double()
		{
			yield return new LeftOverWarningTest<ScriptableDouble>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_Decimal()
		{
			yield return new LeftOverWarningTest<ScriptableDecimal>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableValue_String()
		{
			yield return new LeftOverWarningTest<ScriptableString>(2, i =>
			{
				i.OnValueChanging += (value, newValue) => { };
				i.OnValueChanged += (value, newValue) => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_Bool()
		{
			yield return new LeftOverWarningTest<ScriptableBoolEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_Byte()
		{
			yield return new LeftOverWarningTest<ScriptableByteEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_SByte()
		{
			yield return new LeftOverWarningTest<ScriptableSByteEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_Short()
		{
			yield return new LeftOverWarningTest<ScriptableShortEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_UShort()
		{
			yield return new LeftOverWarningTest<ScriptableUShortEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_Int()
		{
			yield return new LeftOverWarningTest<ScriptableIntEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_UInt()
		{
			yield return new LeftOverWarningTest<ScriptableUIntEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_Long()
		{
			yield return new LeftOverWarningTest<ScriptableLongEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_ULong()
		{
			yield return new LeftOverWarningTest<ScriptableULongEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_Float()
		{
			yield return new LeftOverWarningTest<ScriptableFloatEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_Double()
		{
			yield return new LeftOverWarningTest<ScriptableDoubleEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_Decimal()
		{
			yield return new LeftOverWarningTest<ScriptableDecimalEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent_String()
		{
			yield return new LeftOverWarningTest<ScriptableStringEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableEvent()
		{
			yield return new LeftOverWarningTest<ScriptableEvent>(1, i => { i.OnInvoked += (sender, arg) => { }; });
		}

		[UnityTest]
		public IEnumerator ScriptableList_Events()
		{
			yield return new LeftOverWarningTest<TestScriptableList>(6, i =>
			{
				i.OnAdded += i1 => { };
				i.OnInserted += (i1, i2) => { };
				i.OnAddedOrInserted += (i1, i2) => { };
				i.OnSet += (i1, i2, arg3) => { };
				i.OnRemoved += (i1, i2) => { };
				i.OnCleared += () => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableDictionary_Events()
		{
			yield return new LeftOverWarningTest<TestScriptableDictionary>(4, i =>
			{
				i.OnAdded += (i1, i2) => { };
				i.OnSet += (i1, i2, arg3) => { };
				i.OnRemoved += (i1, i2) => { };
				i.OnCleared += () => { };
			});
		}

		[UnityTest]
		public IEnumerator ScriptableList_Objects()
		{
			TestScriptableList instance = ScriptableObject.CreateInstance<TestScriptableList>();
			instance.hideFlags = HideFlags.DontSave;
			instance.name = "Instance";

			yield return new EnterPlayMode(false);

			instance.Add(10);

			LogAssert.Expect(LogType.Warning, $"There are left over objects in the scriptable list {instance.name}. You should clear the list before leaving play mode.");

			yield return new ExitPlayMode();
		}

		[UnityTest]
		public IEnumerator ScriptableDictionary_Objects()
		{
			TestScriptableDictionary instance = ScriptableObject.CreateInstance<TestScriptableDictionary>();
			instance.hideFlags = HideFlags.DontSave;
			instance.name = "Instance";

			yield return new EnterPlayMode(false);

			instance.Add(10, 42);

			LogAssert.Expect(LogType.Warning, $"There are left over objects in the scriptable dictionary {instance.name}. You should clear the dictionary before leaving play mode.");

			yield return new ExitPlayMode();
		}

		[UnityTest]
		[TestMustExpectAllLogs]
		public IEnumerator ScriptableDictionary_Objects_NoClear()
		{
			TestScriptableDictionary instance = ScriptableObject.CreateInstance<TestScriptableDictionary>();
			instance.hideFlags = HideFlags.DontSave;
			instance.name = "Instance";
			instance.ClearOnStart = false;

			yield return new EnterPlayMode(false);

			instance.Add(10, 42);

			LogAssert.NoUnexpectedReceived();

			yield return new ExitPlayMode();
		}

		private class LeftOverWarningTest<T> : IEditModeTestYieldInstruction where T : RuntimeScriptableObject
		{
			private readonly int warningAmount;
			private readonly Action<T> subscribeToEvent;

			public bool ExpectDomainReload { get { return false; } }
			public bool ExpectedPlaymodeState { get; private set; }

			public LeftOverWarningTest(int warningAmount, Action<T> subscribeToEvent)
			{
				this.warningAmount = warningAmount;
				this.subscribeToEvent = subscribeToEvent;
			}

			public IEnumerator Perform()
			{
				T instance = ScriptableObject.CreateInstance<T>();
				instance.name = "Test Instance";
				// Set hide flags to DontSave so it doesn't get deleted when exiting play mode.
				instance.hideFlags = HideFlags.DontSave;

				Assert.IsFalse(Application.isPlaying, "Already in play mode when entering play mode.");

				ExpectedPlaymodeState = true;
				EditorApplication.isPlaying = true;

				while (!Application.isPlaying)
				{
					yield return null;
				}

				try
				{
					Assert.IsTrue(Application.isPlaying, "Didn't enter play mode.");
				}
				catch (AssertionException)
				{
					Object.DestroyImmediate(instance);
					EditorApplication.isPlaying = false;
					yield break;
				}

				subscribeToEvent.Invoke(instance);

				try
				{
					Assert.IsTrue(Application.isPlaying, "Not in play mode when exiting play mode.");
					for (int i = 0; i < warningAmount; i++)
					{
						LogAssert.Expect(LogType.Warning, leftOverWarningRegex);
					}
				}
				catch (AssertionException)
				{
					Object.DestroyImmediate(instance);
					EditorApplication.isPlaying = false;
					yield break;
				}

				ExpectedPlaymodeState = false;
				EditorApplication.isPlaying = false;

				while (Application.isPlaying)
				{
					yield return null;
				}

				try
				{
					Assert.IsFalse(Application.isPlaying, "Didn't exit play mode.");
				}
				catch (AssertionException)
				{
					Object.DestroyImmediate(instance);
					EditorApplication.isPlaying = false;
					yield break;
				}

				Object.DestroyImmediate(instance);
			}
		}
	}
}