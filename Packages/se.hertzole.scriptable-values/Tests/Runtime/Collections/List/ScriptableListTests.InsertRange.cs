#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
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
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("args", default(CollectionChangedArgs<int>));
			list.AddRange(items);
			switch (eventType)
			{
				case EventType.Event:
					list.OnCollectionChanged += OnCollectionChanged;
					break;
				case EventType.Register:
					list.RegisterChangedListener(OnCollectionChanged);
					break;
				case EventType.RegisterWithContext:
					list.RegisterChangedListener(OnCollectionChangedWithContext, context);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}

			// Act
			list.InsertRange(index, insertItems);

			// Assert
			CollectionChangedArgs<int> args = context.GetArg<CollectionChangedArgs<int>>("args");
			Assert.AreEqual(insertItems.Length, args.NewItems.Length,
				$"The new items length is not correct. Expected {insertItems.Length} but got {args.NewItems.Length}");

			for (int i = 0; i < insertItems.Length; i++)
			{
				Assert.AreEqual(insertItems[i], args.NewItems.Span[i],
					$"The new item at index {i} is not correct. Expected {insertItems[i]} but got {args.NewItems.Span[i]}");
			}

			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action,
				$"The action is not correct. Expected {NotifyCollectionChangedAction.Add} but got {args.Action}");

			Assert.AreEqual(1, context.invokeCount, $"The invoke count is not correct. Expected 1 but got {context.invokeCount}");
			Assert.AreEqual(index, args.NewIndex);
			return;

			void OnCollectionChanged(CollectionChangedArgs<int> e)
			{
				context.invokeCount++;
				context.SetArg("args", e);
			}

			static void OnCollectionChangedWithContext(CollectionChangedArgs<int> e, InvokeCountContext context)
			{
				context.invokeCount++;
				context.SetArg("args", e);
			}
		}

		[Test]
		public void InsertRange_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			int[] insertItems = { 6, 7, 8, 9, 10 };
			int index = Random.Range(1, 4);
			list.AddRange(items);
			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Act
			list.InsertRange(index, insertItems);

			// Assert
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action,
				$"The action is not correct. Expected {NotifyCollectionChangedAction.Add} but got {args.Action}");

			Assert.AreEqual(index, args.NewStartingIndex, $"The new starting index is not correct. Expected {index} but got {args.NewStartingIndex}");
			Assert.AreEqual(insertItems.Length, args.NewItems.Count,
				$"The new items count is not correct. Expected {insertItems.Length} but got {args.NewItems.Count}");

			for (int i = 0; i < insertItems.Length; i++)
			{
				Assert.AreEqual(insertItems[i], args.NewItems[i],
					$"The new item at index {i} is not correct. Expected {insertItems[i]} but got {args.NewItems[i]}");
			}

			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
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
		}

		[Test]
		public void InsertRange_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			int[] insertItems = { 6, 7, 8, 9, 10 };
			int index = Random.Range(1, 4);
			InvokeCountContext context = new InvokeCountContext();
			list.AddRange(items);
			list.IsReadOnly = true;
			switch (eventType)
			{
				case EventType.Event:
					list.OnCollectionChanged += OnCollectionChanged;
					break;
				case EventType.Register:
					list.RegisterChangedListener(OnCollectionChanged);
					break;
				case EventType.RegisterWithContext:
					list.RegisterChangedListener(OnCollectionChangedWithContext, context);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.InsertRange(index, insertItems));
			Assert.AreEqual(0, context.invokeCount, $"The invoke count is not correct. Expected 0 but got {context.invokeCount}");
			return;

			void OnCollectionChanged(CollectionChangedArgs<int> e)
			{
				context.invokeCount++;
			}

			static void OnCollectionChangedWithContext(CollectionChangedArgs<int> e, InvokeCountContext context)
			{
				context.invokeCount++;
			}
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
			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.InsertRange(index, insertItems));
			Assert.IsNull(args, "The collection changed event was invoked.");
			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
		}
	}
}