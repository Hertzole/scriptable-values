using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public class ClearSubscriberTests : BaseEditorTest
	{
		private static readonly Regex leftOverWarningRegex = new Regex(
			@"(On)([A-za-z]*) in object [A-Za-z ]*\(.*\) has some left over subscribers:.*\n.*",
			RegexOptions.Multiline);

		[Test]
		public void ScriptableValue()
		{
			TestClearing<ScriptableBool>(2, i =>
			{
				i.OnValueChanging += (_, _) => { };
				i.OnValueChanged += (_, _) => { };
			}, i => i.ClearSubscribers(true));
		}

		[Test]
		public void ScriptableEvent()
		{
			TestClearing<ScriptableEvent>(1, i => i.OnInvoked += (sender, args) => { }, i => i.ClearSubscribers(true));
		}

		[Test]
		public void ScriptableEvent_WithArgs()
		{
			TestClearing<ScriptableBoolEvent>(1, i => i.OnInvoked += (sender, args) => { }, i => i.ClearSubscribers(true));
		}

		[Test]
		public void ScriptableList()
		{
			TestClearing<TestScriptableList>(7, i =>
			{
				i.OnAdded += i => { };
				i.OnInserted += (i, i1) => { };
				i.OnAddedOrInserted += (i, i1) => { };
				i.OnRemoved += (i, i1) => { };
				i.OnCleared += () => { };
				i.OnSet += (i, i1, arg3) => { };
				i.OnChanged += type => { };
			}, i => i.ClearSubscribers(true));
		}

		[Test]
		public void ScriptableDictionary()
		{
			TestClearing<TestScriptableDictionary>(5, i =>
			{
				i.OnAdded += (i1, i2) => { };
				i.OnRemoved += (i, i1) => { };
				i.OnCleared += () => { };
				i.OnSet += (i, i1, arg3) => { };
				i.OnChanged += type => { };
			}, i => i.ClearSubscribers(true));
		}
		
		[Test]
		public void ScriptablePool()
		{
			TestClearing<TestClassScriptablePool>(4, i =>
			{
				i.OnGetObject += i1 => { };
				i.OnReturnObject += i1 => { };
				i.OnDestroyObject += i1 => { };
				i.OnCreateObject += i1 => { };
			}, i => i.ClearSubscribers(true));
		}

		private void TestClearing<T>(int warnings, Action<T> subscribe, Action<T> clear) where T : RuntimeScriptableObject
		{
			T instance = CreateInstance<T>();
			subscribe(instance);

			for (int i = 0; i < warnings; i++)
			{
				LogAssert.Expect(LogType.Warning, leftOverWarningRegex);
			}

			clear(instance);
		}
	}
}