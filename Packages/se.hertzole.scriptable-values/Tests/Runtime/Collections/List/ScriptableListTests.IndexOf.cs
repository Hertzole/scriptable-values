using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void IndexOf()
		{
			// Arrange
			list.Add(1);
			list.Add(2);
			list.Add(3);

			// Assert
			Assert.AreEqual(1, list.IndexOf(2));
			Assert.AreEqual(-1, list.IndexOf(4));
		}

		[Test]
		public void IndexOf_Object()
		{
			// Arrange
			list.Add(1);
			list.Add(2);
			list.Add(3);

			IList l = list;

			// Assert
			Assert.AreEqual(1, l.IndexOf(2));
			Assert.AreEqual(-1, l.IndexOf(4));
		}

		[Test]
		public void IndexOf_Object_Invalid()
		{
			// Arrange
			list.Add(1);
			list.Add(2);
			list.Add(3);

			IList l = list;

			// Assert
			LogAssert.Expect(LogType.Error, "System.Int32 is not assignable from System.String.");

			Assert.AreEqual(-1, l.IndexOf("invalid"));
		}

		[Test]
		public void IndexOf_Index()
		{
			// Arrange
			list.Add(2);
			list.Add(1);
			list.Add(2);
			list.Add(3);

			// Act
			int index = list.IndexOf(2, 1);

			// Assert
			Assert.AreEqual(2, index);
		}

		[Test]
		public void IndexOf_Index_Validations()
		{
			// Arrange
			list.Add(1);
			list.Add(2);
			list.Add(3);

			// Act & Assert
			AssertThrows<ArgumentOutOfRangeException>(() => list.IndexOf(2, -1));
			AssertThrows<ArgumentOutOfRangeException>(() => list.IndexOf(2, 4));
		}

		[Test]
		public void IndexOf_Index_Count()
		{
			// Arrange
			list.Add(2);
			list.Add(1);
			list.Add(2);
			list.Add(3);

			// Act
			int index = list.IndexOf(2, 0, 3);

			// Assert
			Assert.AreEqual(0, index);
		}

		[Test]
		public void IndexOf_Index_Count_Validations()
		{
			// Arrange
			list.Add(1);
			list.Add(2);
			list.Add(3);

			// Act & Assert
			AssertThrows<ArgumentOutOfRangeException>(() => list.IndexOf(2, -1, 0));
			AssertThrows<ArgumentOutOfRangeException>(() => list.IndexOf(2, 0, -1));
			AssertThrows<ArgumentOutOfRangeException>(() => list.IndexOf(2, 0, 4));
			AssertThrows<ArgumentOutOfRangeException>(() => list.IndexOf(2, 4, 0));
		}
	}
}