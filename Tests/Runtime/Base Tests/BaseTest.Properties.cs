#if SCRIPTABLE_VALUES_PROPERTIES
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Properties;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests
{
    partial class BaseTest
    {
        private static readonly GetPropertiesVisitor propertiesVisitor = new GetPropertiesVisitor();

        protected void AssertHasProperty<T>(string propertyName) where T : ScriptableObject
        {
            T target = CreateInstance<T>();

            Assert.That(HasProperty(target, propertyName), Is.True);
        }

        protected void AssertDoesNotHaveProperty<T>(string propertyName) where T : ScriptableObject
        {
            T target = CreateInstance<T>();

            Assert.That(HasProperty(target, propertyName), Is.False);
        }

        private static bool HasProperty<T>(T target, string propertyName) where T : ScriptableObject
        {
            propertiesVisitor.Reset();

            PropertyContainer.Accept(propertiesVisitor, ref target);
            return propertiesVisitor.HasProperty(propertyName);
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

            protected override void VisitProperty<TContainer, TV>(Property<TContainer, TV> property, ref TContainer container, ref TV value)
            {
                base.VisitProperty(property, ref container, ref value);
                properties.Add(property.Name);
            }
        }
    }
}
#endif