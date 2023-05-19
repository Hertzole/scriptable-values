using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests.Editor
{
	public sealed class ValueReferenceEditorTest : BaseEditorTest
	{
		[Test]
		public void OnValueChanging_NewValue()
		{
			bool eventInvoked = false;
			
			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanging += (oldValue, newValue) =>
			{
				Assert.AreEqual(0, oldValue);
				Assert.AreEqual(1, newValue);
				
				eventInvoked = true;
			};
			
			reference.Value = 1;
			
			Assert.IsTrue(eventInvoked);
		}
		
		[Test]
		public void OnValueChanging_SameValue()
		{
			bool eventInvoked = false;
			
			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanging += (oldValue, newValue) =>
			{
				eventInvoked = true;
			};
			
			reference.Value = 0;
			
			Assert.IsFalse(eventInvoked);
		}
		
		[Test]
		public void OnValueChanged_NewValue()
		{
			bool eventInvoked = false;
			
			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanged += (oldValue, newValue) =>
			{
				Assert.AreEqual(0, oldValue);
				Assert.AreEqual(1, newValue);
				
				eventInvoked = true;
			};
			
			reference.Value = 1;
			
			Assert.IsTrue(eventInvoked);
		}
		
		[Test]
		public void OnValueChanged_SameValue()
		{
			bool eventInvoked = false;
			
			ValueReference<int> reference = new ValueReference<int>(0);
			reference.OnValueChanged += (oldValue, newValue) =>
			{
				eventInvoked = true;
			};
			
			reference.Value = 0;
			
			Assert.IsFalse(eventInvoked);
		}
	}
}