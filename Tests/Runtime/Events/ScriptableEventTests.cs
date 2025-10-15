using System;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests.Events
{
    public class ScriptableEventTests : BaseScriptableEventTest<ScriptableEvent>
    {
        [Test]
        public void Invoke_NoSender([Values] EventType eventType)
        {
            // Arrange
            ScriptableEvent instance = CreateInstance<ScriptableEvent>();
            InvokeCountContext context = new InvokeCountContext();
            context.AddArg("sender", instance);

            switch (eventType)
            {
                case EventType.Event:
                    instance.OnInvoked += InstanceOnInvoked;
                    break;
                case EventType.UnityEvent:
                    instance.onInvoked.AddListener(OnUnityInvoked);
                    break;
            }

            // Act
            instance.Invoke();

            // Assert
            Assert.AreEqual(1, context.invokeCount, "Invoke count should be 1.");

            // Arrange removal
            switch (eventType)
            {
                case EventType.Event:
                    instance.OnInvoked -= InstanceOnInvoked;
                    break;
                case EventType.UnityEvent:
                    instance.onInvoked.RemoveListener(OnUnityInvoked);
                    break;
            }

            // Act
            instance.Invoke();

            // Assert
            Assert.AreEqual(1, context.invokeCount, "Invoke count should still be 1.");
            return;

            void InstanceOnInvoked(object sender, EventArgs e)
            {
                context.invokeCount++;
                Assert.AreEqual(sender, instance);
            }

            void OnUnityInvoked()
            {
                context.invokeCount++;
            }
        }

        [Test]
        public void Invoke_WithSender([Values] EventType eventType)
        {
            // Arrange
            ScriptableEvent instance = CreateInstance<ScriptableEvent>();
            GameObject sender = CreateGameObject();
            InvokeCountContext context = new InvokeCountContext();
            context.AddArg("sender", sender);

            switch (eventType)
            {
                case EventType.Event:
                    instance.OnInvoked += InstanceOnInvoked;
                    break;
                case EventType.UnityEvent:
                    instance.onInvoked.AddListener(OnUnityInvoked);
                    break;
            }

            // Act
            instance.Invoke(sender);

            // Assert
            Assert.AreEqual(1, context.invokeCount, "Invoke count should be 1.");

            // Arrange removal
            switch (eventType)
            {
                case EventType.Event:
                    instance.OnInvoked -= InstanceOnInvoked;
                    break;
                case EventType.UnityEvent:
                    instance.onInvoked.RemoveListener(OnUnityInvoked);
                    break;
            }

            // Act
            instance.Invoke(sender);

            // Assert
            Assert.AreEqual(1, context.invokeCount, "Invoke count should still be 1.");
            return;

            void InstanceOnInvoked(object o, EventArgs e)
            {
                Assert.AreEqual(o, sender);
                context.invokeCount++;
            }

            void OnUnityInvoked()
            {
                context.invokeCount++;
            }
        }

        [Test]
        public void OverrideMethod_OnBeforeInvoked_IsCalled()
        {
            // Arrange
            OverrideScriptableEvent instance = CreateInstance<OverrideScriptableEvent>();
            instance.shouldBlock = false;

            // Act
            instance.Invoke();

            // Assert
            Assert.IsTrue(instance.calledOnBeforeSetValue);
        }

        [Test]
        public void OverrideMethod_OnBeforeInvoked_Blocked()
        {
            // Arrange
            OverrideScriptableEvent instance = CreateInstance<OverrideScriptableEvent>();
            instance.shouldBlock = true;

            // Act
            instance.Invoke();

            // Assert
            Assert.IsTrue(instance.calledOnBeforeSetValue);
            Assert.IsFalse(instance.calledOnAfterSetValue);
        }

        [Test]
        public void OverrideMethod_OnAfterInvoked_IsCalled()
        {
            // Arrange
            OverrideScriptableEvent instance = CreateInstance<OverrideScriptableEvent>();

            // Act
            instance.Invoke();

            // Assert
            Assert.IsTrue(instance.calledOnAfterSetValue);
        }
    }

    public class OverrideScriptableEvent : ScriptableEvent
    {
        public bool shouldBlock = false;

        public bool calledOnBeforeSetValue = false;
        public bool calledOnAfterSetValue = false;

        /// <inheritdoc />
        protected override bool OnBeforeInvoked(object sender)
        {
            calledOnBeforeSetValue = true;

            return !shouldBlock;
        }

        /// <inheritdoc />
        protected override void OnAfterInvoked(object sender)
        {
            calledOnAfterSetValue = true;
        }
    }
}