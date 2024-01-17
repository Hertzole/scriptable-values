using System;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	public abstract partial class BaseValueReferenceTest<TType, TValue> : BaseRuntimeTest where TType : ScriptableValue<TValue>
	{
		private static TValue[] values;

		public static TValue[] StaticsValue
		{
			get { return TestHelper.FindValues(typeof(BaseTest), ref values); }
		}

		[Test]
		public void Create_EmptyConstructor()
		{
			ValueReference<TValue> instance = new ValueReference<TValue>();

			Assert.AreEqual(default, instance.constantValue);
			Assert.IsNull(instance.referenceValue);
			Assert.AreEqual(ValueReferenceType.Reference, instance.valueType);
		}

		[Test]
		public void Create_Constant([ValueSource(nameof(StaticsValue))] TValue value)
		{
			ValueReference<TValue> instance = new ValueReference<TValue>(value);

			Assert.AreEqual(value, instance.constantValue);
			Assert.AreEqual(value, instance.Value);
			Assert.IsNull(instance.referenceValue);
			Assert.AreEqual(ValueReferenceType.Constant, instance.valueType);
		}

		[Test]
		public void Create_Reference([ValueSource(nameof(StaticsValue))] TValue value)
		{
			TType scriptableValue = CreateInstance<TType>();
			scriptableValue.Value = value;

			ValueReference<TValue> instance = new ValueReference<TValue>(scriptableValue);

			Assert.AreEqual(default, instance.constantValue);
			Assert.AreEqual(value, instance.Value);
			Assert.AreEqual(scriptableValue, instance.referenceValue);
			Assert.AreEqual(ValueReferenceType.Reference, instance.valueType);
		}

		[Test]
		public void SetValue_Constant([ValueSource(nameof(StaticsValue))] TValue value)
		{
			ValueReference<TValue> instance = new ValueReference<TValue>(default(TValue));
			instance.Value = value;

			Assert.AreEqual(value, instance.constantValue);
			Assert.AreEqual(value, instance.Value);
			Assert.IsNull(instance.referenceValue);
			Assert.AreEqual(ValueReferenceType.Constant, instance.valueType);
		}

		[Test]
		public void SetValue_Reference([ValueSource(nameof(StaticsValue))] TValue value)
		{
			TType scriptableValue = CreateInstance<TType>();

			ValueReference<TValue> instance = new ValueReference<TValue>(scriptableValue);
			instance.Value = value;

			Assert.AreEqual(default, instance.constantValue);
			Assert.AreEqual(value, instance.Value);
			Assert.AreEqual(value, scriptableValue.Value);
			Assert.AreEqual(scriptableValue, instance.referenceValue);
			Assert.AreEqual(ValueReferenceType.Reference, instance.valueType);
		}

		[Test]
		public void OnValueChanging_Constant([ValueSource(nameof(StaticsValue))] TValue value)
		{
			bool eventInvoked = false;
			
			ValueReference<TValue> instance = new ValueReference<TValue>(MakeDifferentValue(value));
			instance.OnValueChanging += ValueChanging;
			
			instance.Value = value;

			Assert.AreEqual(value, instance.constantValue);
			Assert.AreEqual(value, instance.Value);
			Assert.IsTrue(eventInvoked);

			eventInvoked = false;
			
			instance.OnValueChanging -= ValueChanging;

			instance.Value = MakeDifferentValue(value);
			
			Assert.AreEqual(MakeDifferentValue(value), instance.constantValue);
			Assert.AreEqual(MakeDifferentValue(value), instance.Value);
			Assert.IsFalse(eventInvoked);
			
			void ValueChanging(TValue previousValue, TValue newValue)
			{
				Assert.AreEqual(MakeDifferentValue(value), previousValue);
				Assert.AreEqual(value, newValue);
				eventInvoked = true;
			}
		}

		[Test]
		public void OnValueChanging_Reference([ValueSource(nameof(StaticsValue))] TValue value)
		{
			bool eventInvoked = false;
			
			TType scriptableValue = CreateInstance<TType>();
			scriptableValue.Value = MakeDifferentValue(value);
			
			ValueReference<TValue> instance = new ValueReference<TValue>(scriptableValue);
			instance.OnValueChanging += ValueChanging;
			
			instance.Value = value;

			Assert.AreEqual(default, instance.constantValue);
			Assert.AreEqual(value, instance.Value);
			Assert.AreEqual(value, scriptableValue.Value);
			Assert.IsTrue(eventInvoked);

			eventInvoked = false;
			
			instance.OnValueChanging -= ValueChanging;

			instance.Value = MakeDifferentValue(value);
			
			Assert.AreEqual(default, instance.constantValue);
			Assert.AreEqual(MakeDifferentValue(value), instance.Value);
			Assert.AreEqual(MakeDifferentValue(value), scriptableValue.Value);
			Assert.IsFalse(eventInvoked);
			
			void ValueChanging(TValue previousValue, TValue newValue)
			{
				Assert.AreEqual(MakeDifferentValue(value), previousValue);
				Assert.AreEqual(value, newValue);
				eventInvoked = true;
			}
		}
		
		[Test]
		public void OnValueChanged_Constant([ValueSource(nameof(StaticsValue))] TValue value)
		{
			bool eventInvoked = false;
			
			ValueReference<TValue> instance = new ValueReference<TValue>(MakeDifferentValue(value));
			instance.OnValueChanged += ValueChanged;
			
			instance.Value = value;

			Assert.AreEqual(value, instance.constantValue);
			Assert.AreEqual(value, instance.Value);
			Assert.IsTrue(eventInvoked);

			eventInvoked = false;
			
			instance.OnValueChanged -= ValueChanged;

			instance.Value = MakeDifferentValue(value);
			
			Assert.AreEqual(MakeDifferentValue(value), instance.constantValue);
			Assert.AreEqual(MakeDifferentValue(value), instance.Value);
			Assert.IsFalse(eventInvoked);
			
			void ValueChanged(TValue previousValue, TValue newValue)
			{
				Assert.AreEqual(MakeDifferentValue(value), previousValue);
				Assert.AreEqual(value, newValue);
				eventInvoked = true;
			}
		}
		
		[Test]
		public void OnValueChanged_Reference([ValueSource(nameof(StaticsValue))] TValue value)
		{
			bool eventInvoked = false;
			
			TType scriptableValue = CreateInstance<TType>();
			scriptableValue.Value = MakeDifferentValue(value);
			
			ValueReference<TValue> instance = new ValueReference<TValue>(scriptableValue);
			instance.OnValueChanged += ValueChanged;
			
			instance.Value = value;

			Assert.AreEqual(default, instance.constantValue);
			Assert.AreEqual(value, instance.Value);
			Assert.AreEqual(value, scriptableValue.Value);
			Assert.IsTrue(eventInvoked);

			eventInvoked = false;
			
			instance.OnValueChanged -= ValueChanged;

			instance.Value = MakeDifferentValue(value);
			
			Assert.AreEqual(default, instance.constantValue);
			Assert.AreEqual(MakeDifferentValue(value), instance.Value);
			Assert.AreEqual(MakeDifferentValue(value), scriptableValue.Value);
			Assert.IsFalse(eventInvoked);
			
			void ValueChanged(TValue previousValue, TValue newValue)
			{
				Assert.AreEqual(MakeDifferentValue(value), previousValue);
				Assert.AreEqual(value, newValue);
				eventInvoked = true;
			}
		}

		[Test]
		public void InvalidType_Get()
		{
			ValueReference<TValue> instance = new ValueReference<TValue>(default(TValue))
			{
				valueType = (ValueReferenceType) int.MaxValue
			};

			bool gotError = false;
			try
			{
				TValue temp = instance.Value;
			}
			catch (NotSupportedException)
			{
				gotError = true;
			}
			
			Assert.IsTrue(gotError);
		}
		
		[Test]
		public void InvalidType_Set()
		{
			ValueReference<TValue> instance = new ValueReference<TValue>(default(TValue))
			{
				valueType = (ValueReferenceType) int.MaxValue
			};

			bool gotError = false;
			try
			{
				instance.Value = MakeDifferentValue(default);
			}
			catch (NotSupportedException)
			{
				gotError = true;
			}
			
			Assert.IsTrue(gotError);
		}

		[Test]
		public void SetValueWithoutNotify_Constant()
		{
			bool eventInvoked = false;
			
			ValueReference<TValue> instance = new ValueReference<TValue>(default(TValue));
			instance.OnValueChanged += ValueChanged;
			
			instance.SetValueWithoutNotify(MakeDifferentValue(default));

			Assert.AreEqual(MakeDifferentValue(default), instance.constantValue);
			Assert.AreEqual(MakeDifferentValue(default), instance.Value);
			Assert.IsFalse(eventInvoked);

			eventInvoked = false;
			
			instance.OnValueChanged -= ValueChanged;

			TValue differentValue = MakeDifferentValue(instance.Value);
			
			instance.SetValueWithoutNotify(differentValue);
			
			Assert.AreEqual(differentValue, instance.constantValue);
			Assert.AreEqual(differentValue, instance.Value);
			Assert.IsFalse(eventInvoked);
			
			void ValueChanged(TValue previousValue, TValue newValue)
			{
				Assert.AreEqual(MakeDifferentValue(default), previousValue);
				Assert.AreEqual(MakeDifferentValue(default), newValue);
				eventInvoked = true;
			}
		}
		
		[Test]
		public void SetValueWithoutNotify_Reference()
		{
			bool eventInvoked = false;
			
			TType scriptableValue = CreateInstance<TType>();
			scriptableValue.Value = default;
			
			ValueReference<TValue> instance = new ValueReference<TValue>(scriptableValue);
			instance.OnValueChanged += ValueChanged;
			
			instance.SetValueWithoutNotify(MakeDifferentValue(default));

			Assert.AreEqual(default, instance.constantValue);
			Assert.AreEqual(MakeDifferentValue(default), instance.Value);
			Assert.AreEqual(MakeDifferentValue(default), scriptableValue.Value);
			Assert.IsFalse(eventInvoked);

			eventInvoked = false;
			
			instance.OnValueChanged -= ValueChanged;

			TValue differentValue = MakeDifferentValue(instance.Value);
			
			instance.SetValueWithoutNotify(differentValue);
			
			Assert.AreEqual(default, instance.constantValue);
			Assert.AreEqual(differentValue, instance.Value);
			Assert.AreEqual(differentValue, scriptableValue.Value);
			Assert.IsFalse(eventInvoked);
			
			void ValueChanged(TValue previousValue, TValue newValue)
			{
				Assert.AreEqual(MakeDifferentValue(default), previousValue);
				Assert.AreEqual(MakeDifferentValue(default), newValue);
				eventInvoked = true;
			}
		}

		protected abstract TValue MakeDifferentValue(TValue value);
	}
}