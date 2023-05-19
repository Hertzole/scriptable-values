using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Application = UnityEngine.Device.Application;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests.Editor
{
	public sealed class ValueReferenceEditorTest : BaseEditorTest
	{
		[Test]
		public void OnValueChanging_NewValue_EditMode()
		{
			Assert.IsFalse(Application.isPlaying);
			
			bool eventInvoked = false;
			
			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanging += (oldValue, newValue) =>
			{
				eventInvoked = true;
			};
			
			reference.SetPreviousValue();

			reference.constantValue = 1;
			
			reference.SetEditorValue();

			Assert.IsFalse(eventInvoked);
		}
		
		[Test]
		public void OnValueChanging_SameValue_EditMode()
		{
			Assert.IsFalse(Application.isPlaying);

			bool eventInvoked = false;
			
			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanging += (oldValue, newValue) =>
			{
				eventInvoked = true;
			};
			
			reference.SetPreviousValue();
			
			reference.constantValue = 0;
			
			reference.SetEditorValue();
			
			Assert.IsFalse(eventInvoked);
		}
		
		[Test]
		public void OnValueChanged_NewValue_EditMode()
		{
			Assert.IsFalse(Application.isPlaying);
			
			bool eventInvoked = false;
			
			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanged += (oldValue, newValue) =>
			{
				eventInvoked = true;
			};
			
			reference.SetPreviousValue();

			reference.constantValue = 1;
			
			reference.SetEditorValue();
			
			Assert.IsFalse(eventInvoked);
		}
		
		[Test]
		public void OnValueChanged_SameValue_EditMode()
		{
			Assert.IsFalse(Application.isPlaying);

			bool eventInvoked = false;
			
			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanged += (oldValue, newValue) =>
			{
				eventInvoked = true;
			};
			
			reference.SetPreviousValue();
			
			reference.constantValue = 0;
			
			reference.SetEditorValue();
			
			Assert.IsFalse(eventInvoked);
		}
		
		[UnityTest]
		public IEnumerator OnValueChanging_NewValue_PlayMode()
		{
			yield return new EnterPlayMode(false);

			bool eventInvoked = false;

			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanging += (oldValue, newValue) =>
			{
				Assert.AreEqual(0, oldValue);
				Assert.AreEqual(1, newValue);

				eventInvoked = true;
			};

			reference.SetPreviousValue();

			reference.constantValue = 1;

			reference.SetEditorValue();

			Assert.IsTrue(eventInvoked);
		}

		[UnityTest]
		public IEnumerator OnValueChanging_SameValue_PlayMode()
		{
			yield return new EnterPlayMode(false);

			bool eventInvoked = false;

			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanging += (oldValue, newValue) => { eventInvoked = true; };

			reference.SetPreviousValue();

			reference.constantValue = 0;

			reference.SetEditorValue();

			Assert.IsFalse(eventInvoked);
		}

		[UnityTest]
		public IEnumerator OnValueChanged_NewValue_PlayMode()
		{
			yield return new EnterPlayMode(false);

			bool eventInvoked = false;

			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanged += (oldValue, newValue) =>
			{
				Assert.AreEqual(0, oldValue);
				Assert.AreEqual(1, newValue);

				eventInvoked = true;
			};

			reference.SetPreviousValue();

			reference.constantValue = 1;

			reference.SetEditorValue();

			Assert.IsTrue(eventInvoked);
		}

		[UnityTest]
		public IEnumerator OnValueChanged_SameValue_PlayMode()
		{
			yield return new EnterPlayMode(false);

			bool eventInvoked = false;

			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanged += (oldValue, newValue) => { eventInvoked = true; };

			reference.SetPreviousValue();

			reference.constantValue = 0;

			reference.SetEditorValue();

			Assert.IsFalse(eventInvoked);
		}
	}
}