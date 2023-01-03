using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public class LeftOverEventsWarningTests : BaseEditorTest
	{
		private static readonly Regex leftOverWarningRegex = new Regex(
			@"(On)([A-za-z]*) in object [A-Za-z ]*\(.*\) has some left over subscribers:.*\n.*",
			RegexOptions.Multiline);

		[Test]
		public void ScriptableValue_Bool()
		{
			TestLeftOverWarning<ScriptableBool>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_Byte()
		{
			TestLeftOverWarning<ScriptableByte>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_SByte()
		{
			TestLeftOverWarning<ScriptableSByte>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_Short()
		{
			TestLeftOverWarning<ScriptableShort>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_UShort()
		{
			TestLeftOverWarning<ScriptableUShort>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_Int()
		{
			TestLeftOverWarning<ScriptableInt>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_UInt()
		{
			TestLeftOverWarning<ScriptableUInt>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_Long()
		{
			TestLeftOverWarning<ScriptableLong>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_ULong()
		{
			TestLeftOverWarning<ScriptableULong>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_Float()
		{
			TestLeftOverWarning<ScriptableFloat>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_Double()
		{
			TestLeftOverWarning<ScriptableDouble>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_Decimal()
		{
			TestLeftOverWarning<ScriptableDecimal>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableValue_String()
		{
			TestLeftOverWarning<ScriptableString>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			});
		}

		[Test]
		public void ScriptableEvent_Bool()
		{
			TestLeftOverWarning<ScriptableBoolEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_Byte()
		{
			TestLeftOverWarning<ScriptableByteEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_SByte()
		{
			TestLeftOverWarning<ScriptableSByteEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_Short()
		{
			TestLeftOverWarning<ScriptableShortEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_UShort()
		{
			TestLeftOverWarning<ScriptableUShortEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_Int()
		{
			TestLeftOverWarning<ScriptableIntEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_UInt()
		{
			TestLeftOverWarning<ScriptableUIntEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_Long()
		{
			TestLeftOverWarning<ScriptableLongEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_ULong()
		{
			TestLeftOverWarning<ScriptableULongEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_Float()
		{
			TestLeftOverWarning<ScriptableFloatEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_Double()
		{
			TestLeftOverWarning<ScriptableDoubleEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_Decimal()
		{
			TestLeftOverWarning<ScriptableDecimalEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptableEvent_String()
		{
			TestLeftOverWarning<ScriptableStringEvent>(1, i => { i.OnInvoked += (_, _) => { }; });
		}

		[Test]
		public void ScriptablePool()
		{
			TestLeftOverWarning<ScriptableGameObjectPool>(4, i =>
			{
				i.OnCreateObject += _ => { };
				i.OnDestroyObject += _ => { };
				i.OnGetObject += _ => { };
				i.OnReturnObject += _ => { };
			});
		}

		[Test]
		public void ScriptableList_Events()
		{
			TestLeftOverWarning<TestScriptableList>(6, i =>
			{
				i.OnAdded += _ => { };
				i.OnInserted += (_, _) => { };
				i.OnAddedOrInserted += (_, _) => { };
				i.OnSet += (_, _, _) => { };
				i.OnRemoved += (_, _) => { };
				i.OnCleared += () => { };
			});
		}

		[Test]
		public void ScriptableDictionary_Events()
		{
			TestLeftOverWarning<TestScriptableDictionary>(4, i =>
			{
				i.OnAdded += (_, _) => { };
				i.OnSet += (_, _, _) => { };
				i.OnRemoved += (_, _) => { };
				i.OnCleared += () => { };
			});
		}

		[Test]
		public void ScriptableList_Objects()
		{
			TestScriptableList instance = CreateInstance<TestScriptableList>();
			instance.name = "Instance";

			instance.Add(10);

			LogAssert.Expect(LogType.Warning, $"There are left over objects in the scriptable list {instance.name}. You should clear the list before leaving play mode.");

			instance.Test_ExitPlayMode();
		}

		[Test]
		public void ScriptableDictionary_Objects()
		{
			TestScriptableDictionary instance = CreateInstance<TestScriptableDictionary>();
			instance.name = "Instance";

			instance.Add(10, 42);

			LogAssert.Expect(LogType.Warning, $"There are left over objects in the scriptable dictionary {instance.name}. You should clear the dictionary before leaving play mode.");

			instance.Test_ExitPlayMode();
		}

		[Test]
		[TestMustExpectAllLogs]
		public void ScriptableList_Objects_NoClear()
		{
			TestScriptableList instance = CreateInstance<TestScriptableList>();
			instance.name = "Instance";
			instance.ClearOnStart = false;

			instance.Add(10);

			LogAssert.NoUnexpectedReceived();

			instance.Test_ExitPlayMode();
		}

		[Test]
		[TestMustExpectAllLogs]
		public void ScriptableDictionary_Objects_NoClear()
		{
			TestScriptableDictionary instance = CreateInstance<TestScriptableDictionary>();
			instance.name = "Instance";
			instance.ClearOnStart = false;

			instance.Add(10, 42);

			LogAssert.NoUnexpectedReceived();

			instance.Test_ExitPlayMode();
		}

		private void TestLeftOverWarning<T>(int warningAmount, Action<T> subscribeToEvent) where T : RuntimeScriptableObject
		{
			T instance = CreateInstance<T>();
			instance.name = "Test Instance";
			// Set hide flags to DontSave so it doesn't get deleted when exiting play mode.
			instance.hideFlags = HideFlags.DontSave;

			subscribeToEvent.Invoke(instance);

			for (int i = 0; i < warningAmount; i++)
			{
				LogAssert.Expect(LogType.Warning, leftOverWarningRegex);
			}

			instance.Test_ExitPlayMode();
		}
	}
}