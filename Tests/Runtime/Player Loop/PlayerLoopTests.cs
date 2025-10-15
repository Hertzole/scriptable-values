using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues.Tests
{
    public class PlayerLoopTests : BaseRuntimeTest
    {
        [UnityTest]
        public IEnumerator Instantiate_KeepsEvents()
        {
            // Arrange
            bool invoked = false;
            ScriptableBool instance = CreateInstance<ScriptableBool>();
            instance.OnValueChanged += InstanceOnOnValueChanged;

            // Act
            yield return null;
            instance.Value = !instance.Value;

            // Assert
            Assert.That(invoked, Is.True);

            // Cleanup
            instance.OnValueChanged -= InstanceOnOnValueChanged;

            void InstanceOnOnValueChanged(bool oldValue, bool newValue)
            {
                invoked = true;
            }
        }

        [Test]
        public void Instantiate_CallsEnableCallbacks()
        {
            // Act
            CallbackObject instance = CreateInstance<CallbackObject>();

            // Assert
            Assert.That(instance.onPreStartCalled, Is.True);
            Assert.That(instance.onStartCalled, Is.True);
        }

        [UnityTest]
        public IEnumerator Destroy_CallsDisableCallbacks()
        {
            // Arrange
            CallbackObject instance = CreateInstance<CallbackObject>();

            // Act
            Destroy(instance);
            yield return null;

            // Assert
            Assert.That(instance.onPreDisabledCalled, Is.True);
            Assert.That(instance.onDisabledCalled, Is.True);
        }
    }
}