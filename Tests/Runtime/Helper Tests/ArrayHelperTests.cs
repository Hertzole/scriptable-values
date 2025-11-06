using System;
using System.Buffers;
using System.Linq;
using Hertzole.ScriptableValues.Helpers;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    public class ArrayHelperTests : BaseRuntimeTest
    {
        [Test]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(300)]
        public void EnsureCapacity_Success(int capacity)
        {
            // Arrange
            int startingCapacity = capacity / 2;
            int[] array = ArrayPool<int>.Shared.Rent(startingCapacity);

            for (int i = 0; i < startingCapacity; i++)
            {
                array[i] = GetRandomNumber();
            }

            int[] originalArray = array.ToArray();

            // Act
            ArrayHelpers.EnsureCapacity(ref array, capacity);

            // Assert
            Assert.That(array.Length, Is.AtLeast(capacity));
            for (int i = 0; i < startingCapacity; i++)
            {
                Assert.That(array[i], Is.EqualTo(originalArray[i]), $"Array value at index {i} should be the same.");
            }
        }

        [Test]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(300)]
        public void SequenceEquals_Success(int length)
        {
            // Arrange
            int[] left = new int[length];
            int[] right = new int[length];

            for (int i = 0; i < length; i++)
            {
                left[i] = GetRandomNumber();
                right[i] = left[i];
            }

            // Act
            bool result = ArrayHelpers.SequenceEquals<int>(left.AsSpan(), right.AsSpan());

            // Assert
            Assert.That(result, Is.True, "Arrays should be equal.");
        }

        [Test]
        public void SequenceEquals_Failure_DifferentLength()
        {
            // Arrange
            int[] left = new int[10];
            int[] right = new int[20];

            // Act
            bool result = ArrayHelpers.SequenceEquals<int>(left.AsSpan(), right.AsSpan());

            // Assert
            Assert.That(result, Is.False, "Arrays should not be equal.");
        }

        [Test]
        public void SequenceEquals_Failure_DifferentContent()
        {
            // Arrange
            int[] left = GetShuffledArray<int>(10_000);
            int[] right = GetShuffledArray<int>(10_000);

            // Act
            bool result = ArrayHelpers.SequenceEquals<int>(left.AsSpan(), right.AsSpan());

            // Assert
            Assert.That(result, Is.False, "Arrays should not be equal.");
        }
    }
}