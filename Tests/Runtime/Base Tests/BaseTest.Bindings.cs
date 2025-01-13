#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues.Tests
{
	partial class BaseTest
	{
		protected void AssertNotifyPropertyChangedCalled<TInstance>(string propertyName,
			Action<TInstance> setValue,
			[CanBeNull] Action<TInstance> setDefaultValue = null) where TInstance : ScriptableObject
		{
			TInstance instance = CreateInstance<TInstance>();
 			AssertNotifyPropertyChangedCalled(instance, propertyName, setValue, setDefaultValue);
		}
		
		protected void AssertNotifyPropertyChangedCalled<TInstance>(TInstance instance, string propertyName,
			Action<TInstance> setValue,
			[CanBeNull] Action<TInstance> setDefaultValue = null) where TInstance : ScriptableObject
		{
			// Arrange
			setDefaultValue?.Invoke(instance);

			bool eventInvoked = false;
			((INotifyBindablePropertyChanged) instance).propertyChanged += (sender, args) =>
			{
				eventInvoked = true;
				Assert.That(args.propertyName.ToString(), Is.SameAs(propertyName),
					$"propertyName should be the same ({propertyName} == {args.propertyName.ToString()}).");
			};

			// Act
			setValue(instance);

			// Assert
			Assert.That(eventInvoked, Is.True, "propertyChanged should be invoked.");
		}

		protected void AssertHashCodeChanged<TInstance>(Action<TInstance> setValue,
			[CanBeNull] Action<TInstance> setDefaultValue = null) where TInstance : ScriptableObject
		{
			// Arrange
			TInstance instance = CreateInstance<TInstance>();
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