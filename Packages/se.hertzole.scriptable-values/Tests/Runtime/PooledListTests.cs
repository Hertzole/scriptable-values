using System;
using NUnit.Framework;
using AssertionException = UnityEngine.Assertions.AssertionException;

namespace Hertzole.ScriptableValues.Tests
{
	public class PooledListTests
	{
		private PooledList<int> list;

		[SetUp]
		public void SetUp()
		{
			list = new PooledList<int>();
		}

		[TearDown]
		public void TearDown()
		{
			list.Dispose();
			list = null;
		}

		[Test]
		public void Add_AddsItem()
		{
			// Act
			list.Add(1);

			// Assert
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
		}

		[Test]
		public void AddMany_ResizesItemsArray()
		{
			// Arrange
			list.Add(byte.MaxValue);
			int[] initialArray = list.items;

			// Act
			for (int i = 0; i < initialArray.Length * 2; i++)
			{
				list.Add(i);
			}

			// Assert
			Assert.IsTrue(list.items.Length > initialArray.Length);
			Assert.AreEqual(byte.MaxValue, list[0]);
			for (int i = 0; i < initialArray.Length * 2; i++)
			{
				Assert.AreEqual(i, list[i + 1]);
			}
		}

		[Test]
		public void AddFrom_AddsItemsFromOtherList()
		{
			// Arrange
			using PooledList<int> other = new PooledList<int>();
			other.Add(1);
			other.Add(2);

			// Act
			list.AddFrom(other);

			// Assert
			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
		}

		[Test]
		public void AddFrom_NullList_ThrowsException()
		{
			// Act
			Assert.Throws<AssertionException>(() => list.AddFrom(null));
		}

		[Test]
		public void AddFrom_EmptyList_DoesNothing()
		{
			// Arrange
			using PooledList<int> other = new PooledList<int>();

			// Act
			list.AddFrom(other);

			// Assert
			Assert.AreEqual(0, list.Count);
			Assert.AreEqual(0, list.items.Length);
		}

		[Test]
		public void Remove_RemovesItem()
		{
			// Arrange
			list.Add(1);

			// Act
			list.Remove(1);

			// Assert
			Assert.AreEqual(0, list.Count);
		}

		[Test]
		public void Remove_ItemNotFound_DoesNothing()
		{
			// Arrange
			list.Add(1);

			// Act
			list.Remove(2);

			// Assert
			Assert.AreEqual(1, list.Count);
		}

		[Test]
		public void Remove_EmptyList_DoesNothing()
		{
			// Act
			list.Remove(1);

			// Assert
			Assert.AreEqual(0, list.Count);
		}

		[Test]
		public void Remove_ClearsReferenceValue()
		{
			// Arrange
			using PooledList<object> objList = new PooledList<object>();
			object obj = new object();
			objList.Add(obj);

			// Act
			objList.Remove(obj);

			// Assert
			Assert.AreEqual(0, objList.Count);
			Assert.AreEqual(null, objList.items[0]);
		}

		[Test]
		public void RemoveFrom_RemovesItemsFromOtherList()
		{
			// Arrange
			list.Add(1);
			list.Add(2);

			using PooledList<int> other = new PooledList<int>();
			other.Add(1);
			other.Add(2);
			other.Add(3);

			// Act
			list.RemoveFrom(other);

			// Assert
			Assert.AreEqual(0, list.Count);
		}

		[Test]
		public void RemoveFrom_NullList_ThrowsException()
		{
			// Act
			Assert.Throws<AssertionException>(() => list.RemoveFrom(null));
		}

		[Test]
		public void RemoveFrom_EmptyList_DoesNothing()
		{
			// Arrange
			list.Add(1);

			using PooledList<int> other = new PooledList<int>();

			// Act
			list.RemoveFrom(other);

			// Assert
			Assert.AreEqual(1, list.Count);
		}

		[Test]
		public void RemoveFrom_EmptySourceList_DoesNothing()
		{
			// Arrange
			using PooledList<int> other = new PooledList<int>();
			other.Add(1);

			// Act
			list.RemoveFrom(other);

			// Assert
			Assert.AreEqual(0, list.Count);
		}

		[Test]
		public void Clear_SetsCountToZero()
		{
			// Arrange
			list.Add(1);
			list.Add(2);

			// Act
			list.Clear();

			// Assert
			Assert.AreEqual(0, list.Count);
		}

		[Test]
		public void Clear_ClearsReferenceValues()
		{
			// Arrange
			using PooledList<object> objList = new PooledList<object>();
			object obj = new object();
			object obj2 = new object();
			objList.Add(obj);
			objList.Add(obj2);

			// Act
			objList.Clear();

			// Assert
			Assert.AreEqual(0, objList.Count);
			Assert.AreEqual(null, objList.items[0]);
			Assert.AreEqual(null, objList.items[1]);
		}

		[Test]
		public void AsSpan()
		{
			// Arrange
			list.Add(1);
			list.Add(2);
			list.Add(3);

			// Act
			ReadOnlySpan<int> span = list.AsSpan();

			// Assert
			Assert.AreEqual(3, span.Length);
			Assert.AreEqual(1, span[0]);
			Assert.AreEqual(2, span[1]);
			Assert.AreEqual(3, span[2]);
		}
	}
}