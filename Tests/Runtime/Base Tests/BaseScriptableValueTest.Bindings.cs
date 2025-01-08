#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Tests
{
	partial class BaseScriptableValueTest<TType, TValue>
	{
		[Test]
		public void Binding_IsReadOnly_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(nameof(ScriptableValue.IsReadOnly), false, true);
		}

		[Test]
		public void Binding_IsReadOnly_ChangesHashCode()
		{
			AssertHashCodeCHanged(nameof(ScriptableValue.IsReadOnly), false, true);
		}

		[Test]
		public void Binding_ResetValueOnStart_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(nameof(ScriptableValue.ResetValueOnStart), false, true);
		}

		[Test]
		public void Binding_ResetValueOnStart_ChangesHashCode()
		{
			AssertHashCodeCHanged(nameof(ScriptableValue.ResetValueOnStart), false, true);
		}

		[Test]
		public void Binding_SetEqualityCheck_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(nameof(ScriptableValue.SetEqualityCheck), false, true);
		}

		[Test]
		public void Binding_SetEqualityCheck_ChangesHashCode()
		{
			AssertHashCodeCHanged(nameof(ScriptableValue.SetEqualityCheck), false, true);
		}

		[Test]
		public void Binding_Value_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(nameof(ScriptableValue<TType>.Value), default, MakeDifferentValue(default));
		}

		[Test]
		public void Binding_Value_ChangesHashCode()
		{
			AssertHashCodeCHanged(nameof(ScriptableValue<TType>.Value), default, MakeDifferentValue(default));
		}

		[Test]
		public void Binding_PreviousValue_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(nameof(ScriptableValue<TType>.PreviousValue), default, MakeDifferentValue(default));
		}

		[Test]
		public void Binding_PreviousValue_ChangesHashCode()
		{
			AssertHashCodeCHanged(nameof(ScriptableValue<TType>.PreviousValue), default, MakeDifferentValue(default));
		}

		[Test]
		public void Binding_DefaultValue_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(nameof(ScriptableValue<TType>.DefaultValue), default, MakeDifferentValue(default));
		}

		[Test]
		public void Binding_DefaultValue_ChangesHashCode()
		{
			AssertHashCodeCHanged(nameof(ScriptableValue<TType>.DefaultValue), default, MakeDifferentValue(default));
		}

		private void AssertNotifyPropertyChangedCalled<T>(string propertyName, T defaultValue, T newValue)
		{
			// Arrange
			Visitor<T> visitor = new Visitor<T>();

			TType instance = CreateInstance<TType>();
			PropertyContainer.Accept(visitor, ref instance);

			visitor.SetPropertyValue(instance, propertyName, defaultValue);

			bool eventInvoked = false;
			((INotifyBindablePropertyChanged) instance).propertyChanged += (sender, args) =>
			{
				eventInvoked = true;
				Assert.That(args.propertyName.ToString(), Is.SameAs(propertyName),
					$"propertyName should be the same ({propertyName} == {args.propertyName.ToString()}).");
			};

			// Act
			visitor.SetPropertyValue(instance, propertyName, newValue);

			// Assert
			Assert.That(eventInvoked, Is.True, "propertyChanged should be invoked.");
		}

		private void AssertHashCodeCHanged<T>(string propertyName, T defaultValue, T newValue)
		{
			// Arrange
			Visitor<T> visitor = new Visitor<T>();

			TType instance = CreateInstance<TType>();
			PropertyContainer.Accept(visitor, ref instance);

			visitor.SetPropertyValue(instance, propertyName, defaultValue);

			long originalHashCode = ((IDataSourceViewHashProvider) instance).GetViewHashCode();

			// Act
			visitor.SetPropertyValue(instance, propertyName, newValue);

			// Assert
			Assert.That(originalHashCode, Is.Not.EqualTo(((IDataSourceViewHashProvider) instance).GetViewHashCode()), "HashCode should have changed.");
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
					properties.Add(property.Name, UnsafeUtility.As<Property<TContainer, TPropValue>, Property<TType, TVal>>(ref property));
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