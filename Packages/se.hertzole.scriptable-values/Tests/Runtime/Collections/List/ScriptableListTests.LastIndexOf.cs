using System;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        public void LastIndexOf()
        {
            // Arrange
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(2);

            // Assert
            Assert.AreEqual(3, list.LastIndexOf(2));
            Assert.AreEqual(-1, list.LastIndexOf(4));
        }

        [Test]
        public void LastIndexOf_Index()
        {
            // Arrange
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(2);

            // Act
            int index = list.LastIndexOf(2, 2);

            // Assert
            Assert.AreEqual(1, index);
        }

        [Test]
        public void LastIndexOf_Index_Validations()
        {
            // Arrange
            list.Add(1);
            list.Add(2);
            list.Add(3);

            // Act & Assert
            AssertThrows<ArgumentOutOfRangeException>(() => list.LastIndexOf(2, -1));
            AssertThrows<ArgumentOutOfRangeException>(() => list.LastIndexOf(2, 4));
        }

        [Test]
        public void LastIndexOf_Index_Count()
        {
            // Arrange
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(2);

            // Act
            int index = list.LastIndexOf(2, 3, 3);

            // Assert
            Assert.AreEqual(3, index);
        }

        [Test]
        public void LastIndexOf_Index_Count_Validations()
        {
            // Arrange
            list.Add(1);
            list.Add(2);
            list.Add(3);

            // Act & Assert
            AssertThrows<ArgumentOutOfRangeException>(() => list.LastIndexOf(2, -1, 0));
            AssertThrows<ArgumentOutOfRangeException>(() => list.LastIndexOf(2, 0, -1));
            AssertThrows<ArgumentOutOfRangeException>(() => list.LastIndexOf(2, 0, 4));
            AssertThrows<ArgumentOutOfRangeException>(() => list.LastIndexOf(2, 4, 0));
        }
    }
}