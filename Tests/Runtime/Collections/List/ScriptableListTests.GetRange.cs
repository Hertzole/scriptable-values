using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void GetRange()
		{
			// Arrange
			list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

			// Act
			List<int> range = list.GetRange(2, 5);

			// Assert
			Assert.AreEqual(5, range.Count);
			Assert.AreEqual(3, range[0]);
			Assert.AreEqual(4, range[1]);
			Assert.AreEqual(5, range[2]);
			Assert.AreEqual(6, range[3]);
			Assert.AreEqual(7, range[4]);
		}

		[Test]
		public void GetRange_Validations()
		{
			// Arrange
			list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

			// Act & Assert
			AssertThrows<ArgumentOutOfRangeException>(() => list.GetRange(-1, 5));
			AssertThrows<ArgumentOutOfRangeException>(() => list.GetRange(0, -1));
			AssertThrows<ArgumentException>(() => list.GetRange(0, 11));
			AssertThrows<ArgumentException>(() => list.GetRange(5, 6));
			AssertThrows<ArgumentException>(() => list.GetRange(11, 0));
			AssertThrows<ArgumentException>(() => list.GetRange(10, 1));
		}

		[Test]
		[TestCaseSource(nameof(DestinationLists))]
		public void GetRange_DestinationList(IList<int> destinationList)
		{
			// Arrange
			list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
			// Add dummy items to make sure the list is cleared first.
			for (int i = 0; i < 100; i++)
			{
				destinationList.Add(i);
			}

			// Act
			list.GetRange(2, 5, destinationList);

			// Assert
			Assert.AreEqual(5, destinationList.Count);
			Assert.AreEqual(3, destinationList[0]);
			Assert.AreEqual(4, destinationList[1]);
			Assert.AreEqual(5, destinationList[2]);
			Assert.AreEqual(6, destinationList[3]);
			Assert.AreEqual(7, destinationList[4]);
		}

		[Test]
		[TestCaseSource(nameof(DestinationLists))]
		public void GetRange_DestinationList_Validations(IList<int> destinationList)
		{
			// Arrange
			list.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

			// Act & Assert
			AssertThrows<ArgumentOutOfRangeException>(() => list.GetRange(-1, 5, destinationList));
			AssertThrows<ArgumentOutOfRangeException>(() => list.GetRange(0, -1, destinationList));
			AssertThrows<ArgumentException>(() => list.GetRange(0, 11, destinationList));
			AssertThrows<ArgumentException>(() => list.GetRange(5, 6, destinationList));
			AssertThrows<ArgumentException>(() => list.GetRange(11, 0, destinationList));
			AssertThrows<ArgumentException>(() => list.GetRange(10, 1, destinationList));
			AssertThrows<ArgumentNullException>(() => list.GetRange(0, 1, null!));
		}
	}
}