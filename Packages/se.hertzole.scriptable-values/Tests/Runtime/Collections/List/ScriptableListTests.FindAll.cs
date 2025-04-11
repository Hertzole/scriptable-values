using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void FindAll_VerifyVanilla()
		{
			// Arrange
			list.AddRange(new[] { 1, 2, 3, -4, -5 });
			Predicate<int> predicate = i => i >= 0;

			// Act
			List<int> result = list.FindAll(predicate);

			// Assert
			Assert.AreEqual(3, result.Count);
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(2, result[1]);
			Assert.AreEqual(3, result[2]);
		}

		[Test]
		public void FindAll_VerifyDuplicates()
		{
			// Arrange
			list.AddRange(new[] { 1, 2, 3, -4, -5, 1, 2, 3 });
			Predicate<int> predicate = i => i >= 0;

			// Act
			List<int> result = list.FindAll(predicate);

			// Assert
			Assert.AreEqual(6, result.Count);
			Assert.AreEqual(1, result[0]);
			Assert.AreEqual(2, result[1]);
			Assert.AreEqual(3, result[2]);
			Assert.AreEqual(1, result[3]);
			Assert.AreEqual(2, result[4]);
			Assert.AreEqual(3, result[5]);
		}

		[Test]
		[TestCaseSource(nameof(DestinationLists))]
		public void FindAll_VerifyVanilla_Destination(IList<int> destinationList)
		{
			// Arrange
			list.AddRange(new[] { 1, 2, 3, -4, -5 });
			Predicate<int> predicate = i => i >= 0;

			// Act
			list.FindAll(destinationList, predicate);

			// Assert
			Assert.AreEqual(3, destinationList.Count);
			Assert.AreEqual(1, destinationList[0]);
			Assert.AreEqual(2, destinationList[1]);
			Assert.AreEqual(3, destinationList[2]);
		}

		[Test]
		[TestCaseSource(nameof(DestinationLists))]
		public void FindAll_VerifyDuplicates_Destination(IList<int> destinationList)
		{
			// Arrange
			list.AddRange(new[] { 1, 2, 3, -4, -5, 1, 2, 3 });
			Predicate<int> predicate = i => i >= 0;

			// Act
			list.FindAll(destinationList, predicate);

			// Assert
			Assert.AreEqual(6, destinationList.Count);
			Assert.AreEqual(1, destinationList[0]);
			Assert.AreEqual(2, destinationList[1]);
			Assert.AreEqual(3, destinationList[2]);
			Assert.AreEqual(1, destinationList[3]);
			Assert.AreEqual(2, destinationList[4]);
			Assert.AreEqual(3, destinationList[5]);
		}

		[Test]
		public void FindAll_Validations()
		{
			AssertThrows<ArgumentNullException>(() => list.FindAll(null!));
		}

		[Test]
		public void FindAll_List_Validations()
		{
			AssertThrows<ArgumentNullException>(() => list.FindAll(new List<int>(), null!));
			AssertThrows<ArgumentNullException>(() => list.FindAll(null!, i => i >= 0));
		}
	}
}