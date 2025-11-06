using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    public abstract class ScriptableValueTest<TType, TValue> : BaseScriptableValueTest<TType, TValue> where TType : ScriptableValue<TValue>
    {
        private static TValue[] values;

        public static TValue[] StaticsValue
        {
            get { return TestHelper.FindValues(typeof(BaseTest), ref values); }
        }

        [Test]
        public void SetValue([ValueSource(nameof(StaticsValue))] TValue value)
        {
            TestSetValue(value, MakeDifferentValue(value));
        }

        [Test]
        public void SetValue_WithoutNotify([ValueSource(nameof(StaticsValue))] TValue value)
        {
            TestSetValue_WithoutNotify(value, MakeDifferentValue(value));
        }

#if UNITY_EDITOR
        [Test]
        public void SetValue_OnValidate([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(StaticsValue))] TValue value)
        {
            TestSetValue_OnValidate(equalsCheck, value, MakeDifferentValue(value));
        }
#endif // UNITY_EDITOR

        [Test]
        public void SetValue_ReadOnly_ThrowsException()
        {
            TestSetValue_ReadOnly_ThrowsException();
        }

        [Test]
        public void SetValue_SameValue([ValueSource(nameof(StaticsValue))] TValue value)
        {
            TestSetValue_SameValue(value, value);
        }

        [Test]
        public void SetValue_SameValue_NoEqualsCheck([ValueSource(nameof(StaticsValue))] TValue value)
        {
            TestSetValue_SameValue_NoEqualsCheck(value, value);
        }

        [Test]
        public void OverrideMethod_OnBeforeSetValue_IsCalled()
        {
            // Arrange
            OverrideScriptableValue instance = CreateInstance<OverrideScriptableValue>();
            instance.shouldBlock = false;
            int targetValue = MakeDifferentValue(instance.Value);

            // Act
            instance.Value = targetValue;

            // Assert
            Assert.That(instance.calledOnBeforeSetValue, Is.True);
            Assert.That(instance.beforeNewValue, Is.EqualTo(targetValue));
        }

        [Test]
        public void OverrideMethod_OnBeforeSetValue_Blocked()
        {
            // Arrange
            OverrideScriptableValue instance = CreateInstance<OverrideScriptableValue>();
            instance.shouldBlock = true;
            int oldValue = instance.Value;

            // Act
            instance.Value = MakeDifferentValue(instance.Value);

            // Assert
            Assert.That(instance.calledOnBeforeSetValue, Is.True);
            Assert.That(oldValue, Is.EqualTo(instance.Value));
            Assert.That(instance.calledOnAfterSetValue, Is.False);
        }

        [Test]
        public void OverrideMethod_OnAfterSetValue_IsCalled()
        {
            // Arrange
            OverrideScriptableValue instance = CreateInstance<OverrideScriptableValue>();
            int targetValue = MakeDifferentValue(instance.Value);

            // Act
            instance.Value = targetValue;

            // Assert
            Assert.That(instance.calledOnAfterSetValue, Is.True);
            Assert.That(instance.afterOldValue, Is.EqualTo(instance.PreviousValue));
            Assert.That(instance.afterNewValue, Is.EqualTo(targetValue));
        }
    }

    public class OverrideScriptableValue : ScriptableValue<int>
    {
        public bool shouldBlock = false;

        public bool calledOnBeforeSetValue = false;
        public bool calledOnAfterSetValue = false;

        public int beforeNewValue;
        public int afterOldValue;
        public int afterNewValue;

        /// <inheritdoc />
        protected override bool OnBeforeSetValue(int oldValue, int newValue)
        {
            calledOnBeforeSetValue = true;

            beforeNewValue = newValue;
            return !shouldBlock;
        }

        /// <inheritdoc />
        protected override void OnAfterSetValue(int oldValue, int newValue)
        {
            calledOnAfterSetValue = true;

            afterOldValue = oldValue;
            afterNewValue = newValue;
        }
    }
}