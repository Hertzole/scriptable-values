#nullable enable

using System;
using System.Collections.Specialized;
using System.Data;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void AddRange_AddsItems()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };

			// Act
			list.AddRange(items);

			// Assert
			Assert.AreEqual(items.Length, list.Count);
			for (int i = 0; i < items.Length; i++)
			{
				Assert.AreEqual(items[i], list[i]);
			}
		}

		[Test]
		public void AddRange_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("args", default(CollectionChangedArgs<int>));
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
			list.AddRange(items);

			// Assert
			CollectionChangedArgs<int> args = context.GetArg<CollectionChangedArgs<int>>("args");
			Assert.AreEqual(items.Length, args.NewItems.Length);
			for (int i = 0; i < items.Length; i++)
			{
				Assert.AreEqual(items[i], args.NewItems.Span[i]);
			}

			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
			Assert.AreEqual(1, context.invokeCount);
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
		public void AddRange_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int[] items = { 1, 2, 3, 4, 5 };
			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Act
			list.AddRange(items);

			// Assert
			Assert.IsNotNull(args);
			Assert.AreEqual(items.Length, args!.NewItems.Count);
			for (int i = 0; i < items.Length; i++)
			{
				Assert.AreEqual(items[i], args.NewItems[i]);
			}

			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
			Assert.AreEqual(0, args.NewStartingIndex);
			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
		}

		[Test]
		public void AddRange_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			list.IsReadOnly = true;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.AddRange(new[] { 1, 2, 3, 4, 5 }));
		}

		[Test]
		public void AddRange_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			list.IsReadOnly = true;
			int[] items = { 1, 2, 3, 4, 5 };
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("args", default(CollectionChangedArgs<int>));
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

			// Assert
			AssertThrows<ReadOnlyException>(() => list.AddRange(items));
			Assert.AreEqual(0, context.invokeCount);
			Assert.AreEqual(default, context.GetArg<CollectionChangedArgs<int>>("args"));
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
		public void AddRange_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			list.IsReadOnly = true;
			int[] items = { 1, 2, 3, 4, 5 };
			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.AddRange(items));
			Assert.IsNull(args);
			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
		}

		[Test]
		public void AddRange_Null_ThrowsArgumentNullException()
		{
			// Act & Assert
			AssertThrows<ArgumentNullException>(() => list.AddRange(null!));
		}
	}
}