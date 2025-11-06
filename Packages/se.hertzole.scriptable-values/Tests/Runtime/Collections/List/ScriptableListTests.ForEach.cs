using System;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptableListTests
    {
        [Test]
        public void ForEach()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            int sum = 0;

            // Act
            list.ForEach(x => sum += x);

            // Assert
            Assert.AreEqual(55, sum);
        }

        [Test]
        public void ForEach_Validations()
        {
            // Arrange
            list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            // Act & Assert
            AssertThrows<ArgumentNullException>(() => list.ForEach(null!));
            AssertThrows<InvalidOperationException>(() => list.ForEach(x => list.Add(0)));
        }
    }
}