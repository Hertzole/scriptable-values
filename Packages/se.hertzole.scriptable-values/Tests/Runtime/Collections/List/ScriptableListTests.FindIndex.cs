using System;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        public void FindIndex()
        {
            // Arrange
            list.AddRange(new[] { -1, -2, 3, 4, 5 });

            // Act
            int result = list.FindIndex(x => x > 0);

            // Assert
            Assert.AreEqual(2, result);
        }

        [Test]
        public void FindIndex_Validations()
        {
            list.AddRange(new[] { -1, -2, 3, 4, 5 });

            AssertThrows<ArgumentNullException>(() => list.FindIndex(null!));
        }

        [Test]
        public void FindIndex_StartIndex()
        {
            // Arrange
            list.AddRange(new[] { -1, -2, 3, 4, 5 });

            // Act
            int result = list.FindIndex(3, x => x > 0);

            // Assert
            Assert.AreEqual(3, result); // Should've found '4' at index 3
        }

        [Test]
        public void FindIndex_StartIndex_Validations()
        {
            // Arrange
            list.AddRange(new[] { -1, -2, 3, 4, 5 });

            AssertThrows<ArgumentOutOfRangeException>(() => list.FindIndex(-1, x => x > 0));
            AssertThrows<ArgumentOutOfRangeException>(() => list.FindIndex(6, x => x > 0));
            AssertThrows<ArgumentNullException>(() => list.FindIndex(0, null!));
        }

        [Test]
        public void FindIndex_StartIndex_Count()
        {
            // Arrange
            list.AddRange(new[] { -1, -2, 10, 9, 8, 3 });

            // Act
            int result = list.FindIndex(1, 4, x => x > 0);

            // Assert
            Assert.AreEqual(2, result); // Should've found '10' at index 2
        }

        [Test]
        public void FindIndex_StartIndex_Count_Validations()
        {
            // Arrange
            list.AddRange(new[] { -1, -2, 10, 9, 8, 3 });

            AssertThrows<ArgumentOutOfRangeException>(() => list.FindIndex(-1, 4, x => x > 0));
            AssertThrows<ArgumentOutOfRangeException>(() => list.FindIndex(1, -1, x => x > 0));
            AssertThrows<ArgumentOutOfRangeException>(() => list.FindIndex(1, 6, x => x > 0));
            AssertThrows<ArgumentNullException>(() => list.FindIndex(0, 4, null!));
        }
    }
}