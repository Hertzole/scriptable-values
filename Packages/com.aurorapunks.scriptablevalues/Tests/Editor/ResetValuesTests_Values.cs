using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public partial class ResetValuesTests
	{
		[Test]
		public void ScriptableBool_ResetValue()
		{
			ScriptableValueReset<ScriptableBool, bool>(false, true);
		}

		[Test]
		public void ScriptableByte_ResetValue()
		{
			ScriptableValueReset<ScriptableByte, byte>(0, 1);
		}

		[Test]
		public void ScriptableSByte_ResetValue()
		{
			ScriptableValueReset<ScriptableSByte, sbyte>(0, 1);
		}

		[Test]
		public void ScriptableShort_ResetValue()
		{
			ScriptableValueReset<ScriptableShort, short>(0, 1);
		}

		[Test]
		public void ScriptableUShort_ResetValue()
		{
			ScriptableValueReset<ScriptableUShort, ushort>(0, 1);
		}

		[Test]
		public void ScriptableInt_ResetValue()
		{
			ScriptableValueReset<ScriptableInt, int>(0, 1);
		}

		[Test]
		public void ScriptableUInt_ResetValue()
		{
			ScriptableValueReset<ScriptableUInt, uint>(0, 1);
		}

		[Test]
		public void ScriptableLong_ResetValue()
		{
			ScriptableValueReset<ScriptableLong, long>(0, 1);
		}

		[Test]
		public void ScriptableULong_ResetValue()
		{
			ScriptableValueReset<ScriptableULong, ulong>(0, 1);
		}

		[Test]
		public void ScriptableFloat_ResetValue()
		{
			ScriptableValueReset<ScriptableFloat, float>(-11.1f, 12.2f);
		}

		[Test]
		public void ScriptableDouble_ResetValue()
		{
			ScriptableValueReset<ScriptableDouble, double>(-11.1, 12.2);
		}

		[Test]
		public void ScriptableDecimal_ResetValue()
		{
			ScriptableValueReset<ScriptableDecimal, decimal>(-11.1m, 12.2m);
		}

		[Test]
		public void ScriptableString_ResetValue()
		{
			ScriptableValueReset<ScriptableString, string>("First Value", "Second Value");
		}

		[UnityTest]
		public IEnumerator EnterPlayModeResetsValues()
		{
			TestScriptableObject instance = ScriptableObject.CreateInstance<TestScriptableObject>();

			Assert.IsFalse(EditorApplication.isPlaying);
			Assert.IsFalse(instance.HasBeenReset);

			yield return new EnterPlayMode(false);

			Assert.IsTrue(EditorApplication.isPlaying);
			Assert.IsTrue(instance.HasBeenReset);

			Object.DestroyImmediate(instance);
		}

		[UnityTest]
		public IEnumerator ExitPlayModeCallsExitPlayMode()
		{
			TestScriptableObject instance = ScriptableObject.CreateInstance<TestScriptableObject>();
			// Set DontSave so it doesn't get destroyed when exiting play mode.
			instance.hideFlags = HideFlags.DontSave;
			
			Assert.IsFalse(EditorApplication.isPlaying);
			Assert.IsFalse(instance.HasExitedPlayMode);
			
			yield return new EnterPlayMode(false);
			
			Assert.IsTrue(EditorApplication.isPlaying);
			Assert.IsFalse(instance.HasExitedPlayMode);
			
			yield return new ExitPlayMode();
			
			Assert.IsFalse(EditorApplication.isPlaying);
			Assert.IsTrue(instance.HasExitedPlayMode);

			Object.DestroyImmediate(instance);
		}

		private void ScriptableValueReset<TType, TValue>(TValue initialValue, TValue newValue) where TType : ScriptableValue<TValue>
		{
			TType instance = ScriptableObject.CreateInstance<TType>();

			instance.Value = initialValue;
			instance.DefaultValue = initialValue;

			Assert.AreEqual(initialValue, instance.Value);

			instance.Value = newValue;

			Assert.AreEqual(newValue, instance.Value);

			instance.ResetValues();

			Assert.AreEqual(initialValue, instance.Value);
		}

		private class TestScriptableObject : RuntimeScriptableObject
		{
			public bool HasBeenReset { get; private set; } = false;
			public bool HasExitedPlayMode { get; private set; } = false;

			public override void ResetValues()
			{
				base.ResetValues();;
				HasBeenReset = true;
				HasExitedPlayMode = false;
			}

			protected override void OnExitPlayMode()
			{
				base.OnExitPlayMode();
				HasExitedPlayMode = true;
			}
		}
	}
}