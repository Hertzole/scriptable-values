#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Tests
{
	partial class BaseTest
	{
		private readonly List<string> collectedProperties = new List<string>();

		[Obsolete("Use 'AssertPropertyChangesAreInvoked' instead.")]
		protected void AssertNotifyPropertyChangedCalled<TInstance>(string propertyName,
			Action<TInstance> setValue,
			[CanBeNull] Action<TInstance> setDefaultValue = null) where TInstance : ScriptableObject
		{
			TInstance instance = CreateInstance<TInstance>();
			AssertNotifyPropertyChangedCalled(instance, propertyName, setValue, setDefaultValue);
		}

		[Obsolete("Use 'AssertPropertyChangesAreInvoked' instead.")]
		protected void AssertNotifyPropertyChangedCalled<TInstance>(TInstance instance,
			string propertyName,
			Action<TInstance> setValue,
			[CanBeNull] Action<TInstance> setDefaultValue = null) where TInstance : ScriptableObject
		{
			// Arrange
			collectedProperties.Clear();
			setDefaultValue?.Invoke(instance);

			bool eventInvoked = false;
			((INotifyBindablePropertyChanged) instance).propertyChanged += (sender, args) =>
			{
				eventInvoked = true;
				collectedProperties.Add(args.propertyName.ToString());
			};

			// Act
			setValue(instance);

			// Assert
			Assert.That(eventInvoked, Is.True, "propertyChanged should be invoked.");
			Assert.That(collectedProperties, Contains.Item(propertyName), $"propertyChanged should be invoked for {propertyName}.");
		}

		protected void AssertHashCodeChanged<TInstance>(Action<TInstance> setValue,
			[CanBeNull] Action<TInstance> setDefaultValue = null) where TInstance : ScriptableObject
		{
			TInstance instance = CreateInstance<TInstance>();
			AssertHashCodeChanged(instance, setValue, setDefaultValue);
		}

		protected void AssertHashCodeChanged<TInstance>(TInstance instance,
			Action<TInstance> setValue,
			[CanBeNull] Action<TInstance> setDefaultValue = null) where TInstance : ScriptableObject
		{
			// Arrange
			setDefaultValue?.Invoke(instance);
			long originalHashCode = ((IDataSourceViewHashProvider) instance).GetViewHashCode();

			// Act
			setValue(instance);

			// Assert
			Assert.That(originalHashCode, Is.Not.EqualTo(((IDataSourceViewHashProvider) instance).GetViewHashCode()), "HashCode should have changed.");
		}
	}
}
#endif