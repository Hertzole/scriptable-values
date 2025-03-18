#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void Sort_SortsItems()
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);

			// Act
			list.Sort();

			// Assert
			AssertIsSorted(list, 0, items.Length);
		}

		[Test]
		public void Sort_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.Sort();

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			AssertIsArgsSorted(items, list, args);
		}

		[Test]
		public void Sort_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.Sort();

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			AssertIsArgsSorted(items, list, args);
		}

		[Test]
		public void Sort_ReadOnly_ThrowsReadOnlyException_DoesNotInvokeEvents([Values] EventType eventType)
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			list.IsReadOnly = true;

			// Assert
			AssertThrowsReadOnlyExceptionAndNotInvoked(list, eventType, x => x.Sort());
		}

		[Test]
		public void Sort_EmptyList_DoesNotInvokeEvents([Values] EventType eventType)
		{
			// Arrange
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType, list);

			// Act
			list.Sort();

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void Sort_WithComparer_SortsItems()
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);

			// Act
			list.Sort(ReverseComparer.Instance);

			// Assert
			AssertIsSorted(list, 0, items.Length, ReverseComparer.Instance);
		}

		[Test]
		public void Sort_WithComparer_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.Sort(ReverseComparer.Instance);

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			AssertIsArgsSorted(items, list, args);
		}

		[Test]
		public void Sort_WithComparer_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.Sort(ReverseComparer.Instance);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			AssertIsArgsSorted(items, list, args);
		}

		[Test]
		public void Sort_WithComparer_ReadOnly_ThrowsReadOnlyException_DoesNotInvokeEvents([Values] EventType eventType)
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			list.IsReadOnly = true;

			// Assert
			AssertThrowsReadOnlyExceptionAndNotInvoked(list, eventType, x => x.Sort(ReverseComparer.Instance));
		}

		[Test]
		public void Sort_WithComparer_EmptyList_DoesNotInvokeEvents([Values] EventType eventType)
		{
			// Arrange
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType, list);

			// Act
			list.Sort(0, 0);

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void Sort_Range_SortsItems()
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);

			// Act
			list.Sort(2, 5);

			// Assert
			AssertIsSorted(list, 2, 5);
		}

		[Test]
		public void Sort_Range_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.Sort(2, 5);

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action);
			Assert.AreEqual(5, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(5, args.NewItems.Length, "The new items length is not correct.");

			for (int i = 0; i < 5; i++)
			{
				Assert.AreEqual(items[i + 2], args.OldItems.Span[i]);
				Assert.AreEqual(list[i + 2], args.NewItems.Span[i]);
			}
		}

		[Test]
		public void Sort_Range_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.Sort(2, 5);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action);
			Assert.AreEqual(5, args.OldItems.Count, "The old items length is not correct.");
			Assert.AreEqual(5, args.NewItems.Count, "The new items length is not correct.");

			for (int i = 0; i < 5; i++)
			{
				Assert.AreEqual(items[i + 2], (int) args.OldItems[i]);
				Assert.AreEqual(list[i + 2], (int) args.NewItems[i]);
			}
		}

		[Test]
		public void Sort_Range_ReadOnly_ThrowsReadOnlyException_DoesNotInvokeEvents([Values] EventType eventType)
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			list.IsReadOnly = true;

			// Assert
			AssertThrowsReadOnlyExceptionAndNotInvoked(list, eventType, x => x.Sort(2, 5));
		}

		[Test]
		public void Sort_Range_EmptyList_DoesNotInvokeEvents([Values] EventType eventType)
		{
			// Arrange
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType, list);

			// Act
			list.Sort(0, 0);

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		[Test]
		public void Sort_Comparison_SortsItems()
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			Comparison<int> comparison = (x, y) => x - y;

			// Act
			list.Sort(comparison);

			// Assert
			AssertIsSorted(list, comparison);
		}

		[Test]
		public void Sort_Comparison_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			Comparison<int> comparison = (x, y) => x - y;
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.Sort(comparison);

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			AssertIsArgsSorted(items, list, args);
		}

		[Test]
		public void Sort_Comparison_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			Comparison<int> comparison = (x, y) => x - y;
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.Sort(comparison);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			AssertIsArgsSorted(items, list, args);
		}

		[Test]
		public void Sort_Comparison_ReadOnly_ThrowsReadOnlyException_DoesNotInvokeEvents([Values] EventType eventType)
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);
			list.IsReadOnly = true;
			Comparison<int> comparison = (x, y) => x - y;

			// Assert
			AssertThrowsReadOnlyExceptionAndNotInvoked(list, eventType, x => x.Sort(comparison));
		}

		[Test]
		public void Sort_Comparison_EmptyList_DoesNotInvokeEvents([Values] EventType eventType)
		{
			// Arrange
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType, list);
			Comparison<int> comparison = (x, y) => x - y;

			// Act
			list.Sort(comparison);

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		private static void AssertIsArgsSorted<T>(T[] original, IList<T> newList, CollectionChangedArgs<T> args)
		{
			Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
			Assert.AreEqual(original.Length, args.OldItems.Length, "The old items length is not correct.");
			Assert.AreEqual(original.Length, args.NewItems.Length, "The new items length is not correct.");

			for (int i = 0; i < original.Length; i++)
			{
				Assert.AreEqual(original[i], args.OldItems.Span[i]);
				Assert.AreEqual(newList[i], args.NewItems.Span[i]);
			}
		}

		private static void AssertIsArgsSorted<T>(T[] original, IList<T> newList, NotifyCollectionChangedEventArgs args)
		{
			Assert.AreEqual(NotifyCollectionChangedAction.Replace, args.Action, "The action is not correct.");
			Assert.AreEqual(original.Length, args.OldItems.Count, "The old items length is not correct.");
			Assert.AreEqual(original.Length, args.NewItems.Count, "The new items length is not correct.");

			for (int i = 0; i < original.Length; i++)
			{
				Assert.AreEqual(original[i], (T) args.OldItems[i]);
				Assert.AreEqual(newList[i], (T) args.NewItems[i]);
			}
		}

		private static void AssertThrowsReadOnlyExceptionAndNotInvoked<T>(ScriptableList<T> list, EventType eventType, Action<ScriptableList<T>> action)
		{
			// Arrange
			CollectionEventTracker<T> tracker = new CollectionEventTracker<T>(list, eventType, list);

			// Act
			AssertThrows<ReadOnlyException>(() => action.Invoke(list));

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		private static T[] GetShuffledArray<T>(int count = 1000)
		{
			T[] items = Enumerable.Range(0, count).Select(x => (T) Convert.ChangeType(x, typeof(T))).ToArray();
			ShuffleArray(items);
			return items;
		}

		private class ReverseComparer : IComparer<int>
		{
			public static ReverseComparer Instance { get; } = new ReverseComparer();

			public int Compare(int x, int y)
			{
				return y - x;
			}
		}
	}
}