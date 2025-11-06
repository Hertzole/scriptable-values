using System;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        public void FindLast()
        {
            // Arrange
            list.AddRange(new[] { -1, -2, 3, 4, 5 });

            // Act
            int result = list.FindLast(x => x > 0);

            // Assert
            Assert.AreEqual(5, result);
        }

        [Test]
        public void FindLast_Validations()
        {
            list.AddRange(new[] { -1, -2, 3, 4, 5 });

            AssertThrows<ArgumentNullException>(() => list.FindLast(null!));
        }
    }
}