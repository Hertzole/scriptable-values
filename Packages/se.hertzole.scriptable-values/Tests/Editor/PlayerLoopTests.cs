using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues.Tests.Editor
{
    public class PlayerLoopTests : BaseEditorTest
    {
        [UnityTest]
        public IEnumerator EnabledCallbacks_CalledOnEnterPlayMode()
        {
            // Arrange
            CallbackObject instance = CreateInstance<CallbackObject>();
            // Just make sure it's not called before entering play mode.
            Assert.That(instance.onPreStartCalled, Is.False);
            Assert.That(instance.onStartCalled, Is.False);

            // Act
            yield return new EnterPlayMode();

            // Assert
            Assert.That(instance.onPreStartCalled, Is.True);
            Assert.That(instance.onStartCalled, Is.True);
        }

        [UnityTest]
        public IEnumerator DisabledCallbacks_CalledOnExitPlayMode()
        {
            // Arrange
            CallbackObject instance = CreateInstance<CallbackObject>();
            yield return new EnterPlayMode();
            // Just make sure it's not called before exiting play mode.
            Assert.That(instance.onPreDisabledCalled, Is.False);
            Assert.That(instance.onDisabledCalled, Is.False);

            // Act
            yield return new ExitPlayMode();

            // Assert
            Assert.That(instance.onPreDisabledCalled, Is.True);
            Assert.That(instance.onDisabledCalled, Is.True);
        }
    }
}