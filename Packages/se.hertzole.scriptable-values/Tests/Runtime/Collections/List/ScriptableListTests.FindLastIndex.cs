using System;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void FindLastIndex()
		{
			// Arrange
			list.AddRange(new[] { -1, -2, 3, 4, 5 });

			// Act
			int result = list.FindLastIndex(x => x > 0);

			// Assert
			Assert.AreEqual(4, result);
		}

		[Test]
		public void FindLastIndex_Validations()
		{
			list.AddRange(new[] { -1, -2, 3, 4, 5 });

			AssertThrows<ArgumentNullException>(() => list.FindLastIndex(null!));
		}

		[Test]
		public void FindLastIndex_StartIndex()
		{
			// Arrange
			list.AddRange(new[] { 10, -1, -2, 3, 4, 5 });

			// Act
			int result = list.FindLastIndex(3, x => x > 0);

			// Assert
			Assert.AreEqual(3, result);
		}

		[Test]
		public void FindLastIndex_StartIndex_Validations()
		{
			// Arrange
			list.AddRange(new[] { -1, -2, 3, 4, 5 });

			AssertThrows<ArgumentOutOfRangeException>(() => list.FindLastIndex(-1, x => x > 0));
			AssertThrows<ArgumentOutOfRangeException>(() => list.FindLastIndex(6, x => x > 0));
			AssertThrows<ArgumentNullException>(() => list.FindLastIndex(0, null!));
		}

		[Test]
		public void FindLastIndex_StartIndex_Count()
		{
			// Arrange
			list.AddRange(new[] { 10, -1, -2, 10, 9, 8, 3 });

			// Act
			int result = list.FindLastIndex(list.Count - 1, 4, x => x > 0);

			// Assert
			Assert.AreEqual(6, result);
		}

		[Test]
		public void FindLastIndex_StartIndex_Count_Validations()
		{
			// Arrange
			list.AddRange(new[] { -1, -2, 10, 9, 8, 3 });

			AssertThrows<ArgumentOutOfRangeException>(() => list.FindLastIndex(-1, 4, x => x > 0));
			AssertThrows<ArgumentOutOfRangeException>(() => list.FindLastIndex(1, -1, x => x > 0));
			AssertThrows<ArgumentOutOfRangeException>(() => list.FindLastIndex(1, 6, x => x > 0));
			AssertThrows<ArgumentNullException>(() => list.FindLastIndex(0, 4, null!));
		}
	}
}