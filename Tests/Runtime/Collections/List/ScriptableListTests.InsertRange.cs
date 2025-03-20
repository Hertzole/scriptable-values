#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;
using Random = UnityEngine.Random;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void InsertRange_InsertsItems()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			int[] insertItems = { 6, 7, 8, 9, 10 };
			int index = Random.Range(1, 4);
			list.AddRange(items);

			// Act
			list.InsertRange(index, insertItems);

			// Assert
			Assert.AreEqual(items.Length + insertItems.Length, list.Count,
				$"The count is not correct. Expected {items.Length + insertItems.Length} but got {list.Count}");

			List<int> expected = new List<int>(items.Length + insertItems.Length);
			expected.AddRange(items);
			expected.InsertRange(index, insertItems);
			for (int i = 0; i < expected.Count; i++)
			{
				Assert.AreEqual(expected[i], list[i], $"The item at index {i} is not correct. Expected {expected[i]} but got {list[i]}");
			}
		}

		[Test]
		public void InsertRange_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			int[] insertItems = { 6, 7, 8, 9, 10 };
			int index = Random.Range(1, 4);
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.InsertRange(index, insertItems);

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked());

			Assert.AreEqual(index, args.NewIndex);
			AssertArraysAreEqual(insertItems, args.NewItems.ToArray());

			Assert.AreEqual(0, args.OldItems.Length);
			Assert.AreEqual(-1, args.OldIndex);

			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action,
				$"The action is not correct. Expected {NotifyCollectionChangedAction.Add} but got {args.Action}");
		}

		[Test]
		public void InsertRange_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			int[] insertItems = { 6, 7, 8, 9, 10 };
			int index = Random.Range(1, 4);
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.InsertRange(index, insertItems);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked());

			Assert.AreEqual(index, args.NewStartingIndex);
			AssertArraysAreEqual(insertItems, args.NewItems.Cast<int>().ToArray());

			Assert.IsNull(args.OldItems);
			Assert.AreEqual(-1, args.OldStartingIndex);

			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action,
				$"The action is not correct. Expected {NotifyCollectionChangedAction.Add} but got {args.Action}");
		}

		[Test]
		public void InsertRange_OutOfBounds_ThrowsArgumentOutOfRangeException()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			int[] insertItems = { 6, 7, 8, 9, 10 };
			int index = items.Length + 1;
			list.AddRange(items);

			// Assert
			AssertThrows<ArgumentOutOfRangeException>(() => list.InsertRange(index, insertItems));
		}

		[Test]
		public void InsertRange_Null_ThrowsArgumentNullException()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			int index = Random.Range(1, 4);
			list.AddRange(items);

			// Assert
			AssertThrows<ArgumentNullException>(() => list.InsertRange(index, null!));
		}

		[Test]
		public void InsertRange_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			int[] insertItems = { 6, 7, 8, 9, 10 };
			int index = Random.Range(1, 4);
			list.AddRange(items);
			list.IsReadOnly = true;

			// Assert
			AssertThrows<ReadOnlyException>(() => list.InsertRange(index, insertItems));
			AssertThrowsReadOnlyException(list, x => list.InsertRange(index, insertItems));
		}

		[Test]
		public void InsertRange_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			int[] insertItems = { 6, 7, 8, 9, 10 };
			int index = Random.Range(1, 4);
			list.AddRange(items);
			list.IsReadOnly = true;

			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(list, eventType, x => list.InsertRange(index, insertItems), true);
		}

		[Test]
		public void InsertRange_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			int[] insertItems = { 6, 7, 8, 9, 10 };
			int index = Random.Range(1, 4);
			list.AddRange(items);
			list.IsReadOnly = true;

			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(list, x => list.InsertRange(index, insertItems), true);
		}

		[Test]
		public void InsertRange_Empty_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int index = Random.Range(1, 4);
			list.AddRange(GetShuffledArray<int>());

			// Act & Assert
			AssertDoesNotInvokeCollectionChanged(list, eventType, x => list.InsertRange(index, Array.Empty<int>()));
		}

		[Test]
		public void InsertRange_Empty_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			int index = Random.Range(1, 4);
			list.AddRange(GetShuffledArray<int>());

			// Act & Assert
			AssertDoesNotInvokeINotifyCollectionChanged(list, x => list.InsertRange(index, Array.Empty<int>()));
		}
	}
}