#if SCRIPTABLE_VALUES_PROPERTIES
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Properties;

namespace Hertzole.ScriptableValues.Tests
{
	partial class BaseScriptableValueTest<TType, TValue>
	{
		private readonly GetPropertiesVisitor propertiesVisitor = new GetPropertiesVisitor();

		public static readonly string[] bannedProperties =
		{
			nameof(ScriptableValue.isReadOnly),
			nameof(ScriptableValue.resetValueOnStart),
			nameof(ScriptableValue.setEqualityCheck),
			nameof(ScriptableValue<TType>.value),
			nameof(ScriptableValue<TType>.defaultValue),
			nameof(ScriptableValue<TType>.onValueChanging),
			nameof(ScriptableValue<TType>.onValueChanged),
			nameof(ScriptableValue<TType>.OnValueChanging),
			nameof(ScriptableValue<TType>.OnValueChanged),
			nameof(ScriptableValue<TType>.ValueChangingHasSubscribers),
			nameof(ScriptableValue<TType>.ValueChangedHasSubscribers),
#if UNITY_EDITOR
			nameof(RuntimeScriptableObject.CollectStackTraces),
			nameof(RuntimeScriptableObject.CollectStackTraces)
#endif
		};

		public static readonly string[] requiredProperties =
		{
			nameof(ScriptableValue.IsReadOnly),
			nameof(ScriptableValue.ResetValueOnStart),
			nameof(ScriptableValue.SetEqualityCheck),
			nameof(ScriptableValue<TType>.Value),
			nameof(ScriptableValue<TType>.PreviousValue),
			nameof(ScriptableValue<TType>.DefaultValue)
		};

		[Test]
		public void HasProperty([ValueSource(nameof(requiredProperties))] string property)
		{
			TType target = CreateInstance<TType>();

			propertiesVisitor.Reset();

			PropertyContainer.Accept(propertiesVisitor, ref target);
			Assert.That(propertiesVisitor.HasProperty(property), Is.True);
		}

		[Test]
		public void DoesNotHaveProperty([ValueSource(nameof(bannedProperties))] string property)
		{
			TType target = CreateInstance<TType>();

			propertiesVisitor.Reset();

			PropertyContainer.Accept(propertiesVisitor, ref target);
			Assert.That(propertiesVisitor.HasProperty(property), Is.False);
		}

		private sealed class GetPropertiesVisitor : PropertyVisitor
		{
			private readonly HashSet<string> properties = new HashSet<string>();

			public void Reset()
			{
				properties.Clear();
			}

			public bool HasProperty(string name)
			{
				return properties.Contains(name);
			}

			protected override void VisitProperty<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
			{
				base.VisitProperty(property, ref container, ref value);
				properties.Add(property.Name);
			}
		}
	}
}
#endif