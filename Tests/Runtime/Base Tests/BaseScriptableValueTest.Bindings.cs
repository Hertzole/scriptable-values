#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Properties;

namespace Hertzole.ScriptableValues.Tests
{
	partial class BaseScriptableValueTest<TType, TValue>
	{
		[Test]
		public void Binding_IsReadOnly_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalledOnValue(nameof(ScriptableValue.IsReadOnly), false, true);
		}

		[Test]
		public void Binding_IsReadOnly_ChangesHashCode()
		{
			AssertHashCodeChanged(nameof(ScriptableValue.IsReadOnly), false, true);
		}

		[Test]
		public void Binding_ResetValueOnStart_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalledOnValue(nameof(ScriptableValue.ResetValueOnStart), false, true);
		}

		[Test]
		public void Binding_ResetValueOnStart_ChangesHashCode()
		{
			AssertHashCodeChanged(nameof(ScriptableValue.ResetValueOnStart), false, true);
		}

		[Test]
		public void Binding_SetEqualityCheck_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalledOnValue(nameof(ScriptableValue.SetEqualityCheck), false, true);
		}

		[Test]
		public void Binding_SetEqualityCheck_ChangesHashCode()
		{
			AssertHashCodeChanged(nameof(ScriptableValue.SetEqualityCheck), false, true);
		}

		[Test]
		public void Binding_Value_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalledOnValue(nameof(ScriptableValue<TType>.Value), default, MakeDifferentValue<TValue>(default));
		}

		[Test]
		public void Binding_Value_ChangesHashCode()
		{
			AssertHashCodeChanged(nameof(ScriptableValue<TType>.Value), default, MakeDifferentValue<TValue>(default));
		}

		[Test]
		public void Binding_PreviousValue_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalledOnValue(nameof(ScriptableValue<TType>.PreviousValue), default, MakeDifferentValue<TValue>(default));
		}

		[Test]
		public void Binding_PreviousValue_ChangesHashCode()
		{
			AssertHashCodeChanged(nameof(ScriptableValue<TType>.PreviousValue), default, MakeDifferentValue<TValue>(default));
		}

		[Test]
		public void Binding_DefaultValue_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalledOnValue(nameof(ScriptableValue<TType>.DefaultValue), default, MakeDifferentValue<TValue>(default));
		}

		[Test]
		public void Binding_DefaultValue_ChangesHashCode()
		{
			AssertHashCodeChanged(nameof(ScriptableValue<TType>.DefaultValue), default, MakeDifferentValue<TValue>(default));
		}

		private void AssertNotifyPropertyChangedCalledOnValue<T>(string propertyName, T defaultValue, T newValue)
		{
			// Arrange
			Visitor<T> visitor = new Visitor<T>();
			TType instance = CreateInstance<TType>();
			PropertyContainer.Accept(visitor, ref instance);

			// Assert
			AssertNotifyPropertyChangedCalled<TType>(instance, propertyName, instance =>
			{
				visitor.SetPropertyValue(instance, propertyName, newValue);
			}, instance =>
			{
				visitor.SetPropertyValue(instance, propertyName, defaultValue);
			});
		}

		private void AssertHashCodeChanged<T>(string propertyName, T defaultValue, T newValue)
		{
			// Arrange
			Visitor<T> visitor = new Visitor<T>();

			// Assert
			AssertHashCodeChanged<TType>(instance =>
			{
				PropertyContainer.Accept(visitor, ref instance);
				visitor.SetPropertyValue(instance, propertyName, newValue);
			}, instance =>
			{
				PropertyContainer.Accept(visitor, ref instance);
				visitor.SetPropertyValue(instance, propertyName, defaultValue);
			});
		}

		private class Visitor<TVal> : PropertyVisitor
		{
			private readonly Dictionary<string, Property<TType, TVal>> properties = new Dictionary<string, Property<TType, TVal>>();
			
			protected override void VisitProperty<TContainer, TPropValue>(Property<TContainer, TPropValue> property,
				ref TContainer container,
				ref TPropValue value)
			{
				base.VisitProperty(property, ref container, ref value);

				if (typeof(TPropValue) == typeof(TVal))
				{
					properties[property.Name] = UnsafeUtility.As<Property<TContainer, TPropValue>, Property<TType, TVal>>(ref property);
				}
			}

			public void SetPropertyValue(TType instance, string propertyName, TVal value)
			{
				if (!properties.TryGetValue(propertyName, out Property<TType, TVal> property))
				{
					throw new ArgumentException($"Property {propertyName} not found.");
				}

				property.SetValue(ref instance, value);
			}
		}
	}
}
#endif