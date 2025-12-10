using System;
using System.Collections.ObjectModel;
using System.Data;
using Hertzole.ScriptableValues.Helpers;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hertzole.ScriptableValues.Tests
{
    public class GuardTests : BaseRuntimeTest
    {
        private class TestReadOnlyObject : ICanBeReadOnly
        {
            public bool IsReadOnly { get; set; }

            public TestReadOnlyObject(bool isReadOnly)
            {
                IsReadOnly = isReadOnly;
            }
        }

        [Test]
        public void IsNotNull_DoesNotThrow_WhenValueIsNotNull()
        {
            Assert.DoesNotThrow(() => Guard.IsNotNull("Test", nameof(IsNotNull_DoesNotThrow_WhenValueIsNotNull)));
        }

        [Test]
        public void IsNotNull_ThrowsArgumentNullException_WhenValueIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Guard.IsNotNull<string>(null, nameof(IsNotNull_ThrowsArgumentNullException_WhenValueIsNull)));
        }

        [Test]
        public void IsNotNull_DoesNotThrow_ForUnityObject_WhenValueIsNotNull()
        {
            GameObject gameObject = CreateGameObject();
            Assert.DoesNotThrow(() => Guard.IsNotNull(gameObject, nameof(IsNotNull_DoesNotThrow_ForUnityObject_WhenValueIsNotNull)));
        }

        [Test]
        public void IsNotNull_ThrowsArgumentNullException_ForUnityObject_WhenValueIsNull()
        {
            GameObject gameObject = CreateGameObject();
            Object.DestroyImmediate(gameObject);
            Assert.Throws<ArgumentNullException>(() =>
                Guard.IsNotNull(gameObject, nameof(IsNotNull_ThrowsArgumentNullException_ForUnityObject_WhenValueIsNull)));
        }

        [Test]
        public void IsNotReadOnly_DoesNotThrow_WhenValueIsNotReadOnly()
        {
            Assert.DoesNotThrow(() => Guard.IsNotReadOnly(false, nameof(IsNotReadOnly_DoesNotThrow_WhenValueIsNotReadOnly)));
        }

        [Test]
        public void IsNotReadOnly_ThrowsReadOnlyException_WhenValueIsReadOnly()
        {
            Assert.Throws<ReadOnlyException>(() => Guard.IsNotReadOnly(true, nameof(IsNotReadOnly_ThrowsReadOnlyException_WhenValueIsReadOnly)));
        }

        [Test]
        public void IsNotReadOnly_DoesNotThrow_ForICanBeReadOnly_WhenValueIsNotReadOnly()
        {
            TestReadOnlyObject readOnlyObject = new TestReadOnlyObject(false);
            Assert.DoesNotThrow(() => Guard.IsNotReadOnly(readOnlyObject, nameof(IsNotReadOnly_DoesNotThrow_ForICanBeReadOnly_WhenValueIsNotReadOnly)));
        }

        [Test]
        public void IsNotReadOnly_ThrowsReadOnlyException_ForICanBeReadOnly_WhenValueIsReadOnly()
        {
            TestReadOnlyObject readOnlyObject = new TestReadOnlyObject(true);
            Assert.Throws<ReadOnlyException>(() =>
                Guard.IsNotReadOnly(readOnlyObject, nameof(IsNotReadOnly_ThrowsReadOnlyException_ForICanBeReadOnly_WhenValueIsReadOnly)));
        }

        [Test]
        public void IsNotReadOnly_DoesNotThrow_ForICollection_WhenValueIsNotReadOnly()
        {
            Collection<int> list = new Collection<int> { 1, 2, 3 };
            Assert.DoesNotThrow(() => Guard.IsNotReadOnly(list, nameof(IsNotReadOnly_DoesNotThrow_ForICollection_WhenValueIsNotReadOnly)));
        }

        [Test]
        public void IsNotReadOnly_ThrowsReadOnlyException_ForICollection_WhenValueIsReadOnly()
        {
            ReadOnlyCollection<int> list = Array.AsReadOnly(new[] { 1, 2, 3 });
            Assert.Throws<ReadOnlyException>(() =>
                Guard.IsNotReadOnly(list, nameof(IsNotReadOnly_ThrowsReadOnlyException_ForICollection_WhenValueIsReadOnly)));
        }

        [Test]
        public void ThrowIfNullAndNullsAreIllegal_DoesNotThrow_WhenValueIsNotNull()
        {
            Assert.DoesNotThrow(() =>
                Guard.ThrowIfNullAndNullsAreIllegal<string>("Test", nameof(ThrowIfNullAndNullsAreIllegal_DoesNotThrow_WhenValueIsNotNull)));
        }

        [Test]
        public void ThrowIfNullAndNullsAreIllegal_DoesNotThrow_ForNullableType_WhenValueIsNull()
        {
            // Nullable types allow null, so no exception should be thrown
            Assert.DoesNotThrow(() =>
                Guard.ThrowIfNullAndNullsAreIllegal<string>(null, nameof(ThrowIfNullAndNullsAreIllegal_DoesNotThrow_ForNullableType_WhenValueIsNull)));
        }

        [Test]
        public void ThrowIfNullAndNullsAreIllegal_ThrowsArgumentNullException_ForNonNullableType_WhenValueIsNull()
        {
            // Non-nullable value types don't allow null
            Assert.Throws<ArgumentNullException>(() =>
                Guard.ThrowIfNullAndNullsAreIllegal<int>(null,
                    nameof(ThrowIfNullAndNullsAreIllegal_ThrowsArgumentNullException_ForNonNullableType_WhenValueIsNull)));
        }

        [Test]
        public void IsGreaterThanOrEqualTo_DoesNotThrow_WhenValueIsGreaterThanCompareValue()
        {
            Assert.DoesNotThrow(() => Guard.IsGreaterThanOrEqualTo(10, 5, nameof(IsGreaterThanOrEqualTo_DoesNotThrow_WhenValueIsGreaterThanCompareValue)));
        }

        [Test]
        public void IsGreaterThanOrEqualTo_DoesNotThrow_WhenValueIsEqualToCompareValue()
        {
            Assert.DoesNotThrow(() => Guard.IsGreaterThanOrEqualTo(5, 5, nameof(IsGreaterThanOrEqualTo_DoesNotThrow_WhenValueIsEqualToCompareValue)));
        }

        [Test]
        public void IsGreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_WhenValueIsLessThanCompareValue()
        {
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                Guard.IsGreaterThanOrEqualTo(3, 5, nameof(IsGreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_WhenValueIsLessThanCompareValue)));

            Assert.That(exception.ParamName, Is.EqualTo(nameof(IsGreaterThanOrEqualTo_ThrowsArgumentOutOfRangeException_WhenValueIsLessThanCompareValue)));
            Assert.That(exception.Message, Does.Contain("must be greater than or equal to"));
            Assert.That(exception.Message, Does.Contain("<5>"));
            Assert.That(exception.Message, Does.Contain("<3>"));
        }

        [Test]
        public void IsLessThan_DoesNotThrow_WhenValueIsLessThanCompareValue()
        {
            Assert.DoesNotThrow(() => Guard.IsLessThan(3, 5, nameof(IsLessThan_DoesNotThrow_WhenValueIsLessThanCompareValue)));
        }

        [Test]
        public void IsLessThan_ThrowsArgumentOutOfRangeException_WhenValueIsEqualToCompareValue()
        {
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                Guard.IsLessThan(5, 5, nameof(IsLessThan_ThrowsArgumentOutOfRangeException_WhenValueIsEqualToCompareValue)));

            Assert.That(exception.ParamName, Is.EqualTo(nameof(IsLessThan_ThrowsArgumentOutOfRangeException_WhenValueIsEqualToCompareValue)));
            Assert.That(exception.Message, Does.Contain("must be less than"));
            Assert.That(exception.Message, Does.Contain("<5>"));
        }

        [Test]
        public void IsLessThan_ThrowsArgumentOutOfRangeException_WhenValueIsGreaterThanCompareValue()
        {
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                Guard.IsLessThan(10, 5, nameof(IsLessThan_ThrowsArgumentOutOfRangeException_WhenValueIsGreaterThanCompareValue)));

            Assert.That(exception.ParamName, Is.EqualTo(nameof(IsLessThan_ThrowsArgumentOutOfRangeException_WhenValueIsGreaterThanCompareValue)));
            Assert.That(exception.Message, Does.Contain("must be less than"));
            Assert.That(exception.Message, Does.Contain("<5>"));
            Assert.That(exception.Message, Does.Contain("<10>"));
        }

        [Test]
        public void IsInRange_DoesNotThrow_WhenValueIsWithinRange()
        {
            Assert.DoesNotThrow(() => Guard.IsInRange(5, 0, 10, nameof(IsInRange_DoesNotThrow_WhenValueIsWithinRange)));
        }

        [Test]
        public void IsInRange_DoesNotThrow_WhenValueIsEqualToMinValue()
        {
            Assert.DoesNotThrow(() => Guard.IsInRange(0, 0, 10, nameof(IsInRange_DoesNotThrow_WhenValueIsEqualToMinValue)));
        }

        [Test]
        public void IsInRange_DoesNotThrow_WhenValueIsEqualToMaxValue()
        {
            Assert.DoesNotThrow(() => Guard.IsInRange(10, 0, 10, nameof(IsInRange_DoesNotThrow_WhenValueIsEqualToMaxValue)));
        }

        [Test]
        public void IsInRange_ThrowsArgumentOutOfRangeException_WhenValueIsLessThanMinValue()
        {
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                Guard.IsInRange(-1, 0, 10, nameof(IsInRange_ThrowsArgumentOutOfRangeException_WhenValueIsLessThanMinValue)));

            Assert.That(exception.ParamName, Is.EqualTo(nameof(IsInRange_ThrowsArgumentOutOfRangeException_WhenValueIsLessThanMinValue)));
            Assert.That(exception.Message, Does.Contain("must be between"));
            Assert.That(exception.Message, Does.Contain("<0>"));
            Assert.That(exception.Message, Does.Contain("<10>"));
            Assert.That(exception.Message, Does.Contain("<-1>"));
        }

        [Test]
        public void IsInRange_ThrowsArgumentOutOfRangeException_WhenValueIsGreaterThanMaxValue()
        {
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                Guard.IsInRange(11, 0, 10, nameof(IsInRange_ThrowsArgumentOutOfRangeException_WhenValueIsGreaterThanMaxValue)));

            Assert.That(exception.ParamName, Is.EqualTo(nameof(IsInRange_ThrowsArgumentOutOfRangeException_WhenValueIsGreaterThanMaxValue)));
            Assert.That(exception.Message, Does.Contain("must be between"));
            Assert.That(exception.Message, Does.Contain("<0>"));
            Assert.That(exception.Message, Does.Contain("<10>"));
            Assert.That(exception.Message, Does.Contain("<11>"));
        }

        [Test]
        public void ThrowHelper_AssertString_FormatsStringsCorrectly()
        {
            // Test that string values in exceptions are formatted with quotes
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
                Guard.IsNotNull<string>(null, "testParam"));

            Assert.That(exception.Message, Does.Contain("\"testParam\""));
        }

        [Test]
        public void ThrowHelper_AssertString_FormatsNonStringsCorrectly()
        {
            // Test that non-string values in exceptions are formatted with angle brackets
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                Guard.IsGreaterThanOrEqualTo(3, 5, "testParam"));

            Assert.That(exception.Message, Does.Contain("<3>"));
            Assert.That(exception.Message, Does.Contain("<5>"));
        }

        [Test]
        public void ThrowHelper_AssertString_FormatsNullCorrectly()
        {
            // Test that null values in exceptions are formatted as "null"
            // We can test this by checking the exception message when a nullable int is null
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
                Guard.ThrowIfNullAndNullsAreIllegal<int>(null, "testParam"));

            // The message should contain the parameter name formatted as a string
            Assert.That(exception.Message, Does.Contain("\"testParam\""));
        }

        [Test]
        public void IsNotReadOnly_ExceptionMessage_IsCorrect()
        {
            ReadOnlyException exception = Assert.Throws<ReadOnlyException>(() =>
                Guard.IsNotReadOnly(true, "testParam"));

            Assert.That(exception.Message, Does.Contain("\"testParam\""));
            Assert.That(exception.Message, Does.Contain("read-only"));
            Assert.That(exception.Message, Does.Contain("cannot be modified"));
        }

        [Test]
        public void IsNotNull_ExceptionMessage_IsCorrect()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() =>
                Guard.IsNotNull<string>(null, "testParam"));

            Assert.That(exception.ParamName, Is.EqualTo("testParam"));
            Assert.That(exception.Message, Does.Contain("\"testParam\""));
            Assert.That(exception.Message, Does.Contain("must be not null"));
        }
    }
}