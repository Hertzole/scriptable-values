using System.Collections;
using UnityEditor;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public partial class ResetValuesTests : BaseEditorTest
	{
		[UnityTest]
		public IEnumerator EnterPlayModeResetsValues()
		{
			TestScriptableObject instance = CreateInstance<TestScriptableObject>();

			Assert.IsFalse(EditorApplication.isPlaying);
			Assert.IsFalse(instance.HasBeenReset);

			yield return new EnterPlayMode(false);

			Assert.IsTrue(EditorApplication.isPlaying);
			Assert.IsTrue(instance.HasBeenReset);
		}

		[UnityTest]
		public IEnumerator ExitPlayModeCallsExitPlayMode()
		{
			TestScriptableObject instance = CreateInstance<TestScriptableObject>();

			Assert.IsFalse(EditorApplication.isPlaying);
			Assert.IsFalse(instance.HasExitedPlayMode);

			yield return new EnterPlayMode(false);

			Assert.IsTrue(EditorApplication.isPlaying);
			Assert.IsFalse(instance.HasExitedPlayMode);

			yield return new ExitPlayMode();

			Assert.IsFalse(EditorApplication.isPlaying);
			Assert.IsTrue(instance.HasExitedPlayMode);
		}

		private class TestScriptableObject : RuntimeScriptableObject
		{
			public bool HasBeenReset { get; private set; } = false;
			public bool HasExitedPlayMode { get; private set; } = false;

			public override void ResetValues()
			{
				base.ResetValues();
				;
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